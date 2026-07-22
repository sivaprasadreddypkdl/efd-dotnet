# efd-dotnet

Simple .NET hello-world starter project.

## Build
```bash
dotnet build HelloWorld/HelloWorld.csproj
```

This will compile the project and generate a build output under `HelloWorld/bin/Debug/` or `HelloWorld/bin/Release/`, depending on configuration, which can be deployed as a standard .NET application.

## Run
```bash
dotnet run --project HelloWorld/HelloWorld.csproj
```

Expected output:
```text
Hello, World!
```

## Test
```bash
dotnet test HelloWorld.Tests/HelloWorld.Tests.csproj
```
