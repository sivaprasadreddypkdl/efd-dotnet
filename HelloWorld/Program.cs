using System.Net;
using System.Text;

public class Program
{
    public static string GetGreeting() => "Welcome to EFD C# project!";

    public static string GetPage() => $"""
        <!DOCTYPE html>
        <html lang=\"en\">
        <head>
            <meta charset=\"utf-8\" />
            <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />
            <title>EFD C# Welcome</title>
            <style>
                :root {{ --bg: #07111f; --panel: #ffffff; --accent: #14b8a6; --text: #0f172a; --muted: #64748b; }}
                * {{ box-sizing: border-box; }}
                body {{ margin: 0; font-family: 'Segoe UI', Arial, sans-serif; background: linear-gradient(135deg, var(--bg), #102542); color: var(--text); min-height: 100vh; display: grid; place-items: center; padding: 24px; }}
                .card {{ background: var(--panel); border-radius: 20px; padding: 2.5rem 3rem; box-shadow: 0 20px 50px rgba(0,0,0,0.25); text-align: center; max-width: 560px; width: 100%; }}
                .badge {{ display: inline-block; padding: 0.4rem 0.8rem; border-radius: 999px; background: rgba(20,184,166,0.12); color: var(--accent); font-weight: 700; text-transform: uppercase; letter-spacing: 0.08em; font-size: 0.8rem; margin-bottom: 1rem; }}
                h1 {{ margin: 0 0 0.75rem; font-size: 2rem; color: var(--accent); }}
                p {{ margin: 0; color: var(--muted); line-height: 1.6; }}
            </style>
        </head>
        <body>
            <div class=\"card\">
                <div class=\"badge\">EFD • C#</div>
                <h1>{GetGreeting()}</h1>
                <p>This polished welcome page is served from the C# project and is ready to be viewed in a browser.</p>
            </div>
        </body>
        </html>
        """;

    public static async Task Main()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();
        Console.WriteLine("Server started at http://localhost:8080/");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var html = GetPage();
            var buffer = Encoding.UTF8.GetBytes(html);
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}
