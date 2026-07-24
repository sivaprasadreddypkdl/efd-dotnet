# EC2 Runner Infrastructure
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
}

variable "aws_region" {
  default = "us-east-1"
}

variable "instance_type" {
  default = "t2.micro"
}

variable "github_token" {
  sensitive = true
}

variable "repo" {
  description = "GitHub repository (owner/repo)"
}

data "aws_ami" "amazon_linux" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["al2023-ami-*-x86_64"]
  }
}

resource "aws_security_group" "runner" {
  name_prefix = "github-runner-"

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "github-actions-runner"
  }
}

resource "aws_instance" "runner" {
  ami                    = data.aws_ami.amazon_linux.id
  instance_type          = var.instance_type
  vpc_security_group_ids = [aws_security_group.runner.id]

  user_data = <<-EOF
    #!/bin/bash
    set -e

    # Install dependencies
    yum update -y
    yum install -y docker git java-17-amazon-corretto maven

    # Start Docker
    systemctl start docker
    systemctl enable docker

    # Create runner directory
    mkdir -p /opt/actions-runner
    cd /opt/actions-runner

    # Download and extract runner
    curl -o actions-runner-linux-x64.tar.gz -L https://github.com/actions/runner/releases/download/v2.311.0/actions-runner-linux-x64-2.311.0.tar.gz
    tar xzf actions-runner-linux-x64.tar.gz

    # Configure runner
    RUNNER_ALLOW_RUNASROOT=1 ./config.sh --url https://github.com/${var.repo} --token ${var.github_token} --labels self-hosted-${random_id.runner.hex} --unattended --ephemeral

    # Start runner
    RUNNER_ALLOW_RUNASROOT=1 ./run.sh
  EOF

  tags = {
    Name = "github-actions-runner"
  }
}

resource "random_id" "runner" {
  byte_length = 4
}

output "instance_id" {
  value = aws_instance.runner.id
}

output "runner_label" {
  value = "self-hosted-${random_id.runner.hex}"
}
