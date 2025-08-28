using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer
{
    public class WebServer
    {
        private readonly HttpListener _listener;
        private readonly int _port;
        private bool _isRunning;

        public WebServer(int port = 8080)
        {
            _port = port;
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{_port}/");
        }

        public async Task StartAsync()
        {
            try
            {
                _listener.Start();
                _isRunning = true;
                
                Console.WriteLine($"Web server started on http://localhost:{_port}");
                Console.WriteLine("Press Ctrl+C to stop the server...");
                
                while (_isRunning)
                {
                    try
                    {
                        HttpListenerContext context = await _listener.GetContextAsync();
                        
                        _ = Task.Run(() => ProcessRequestAsync(context));
                    }
                    catch (HttpListenerException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error accepting request: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start server: {ex.Message}");
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            try
            {
                LogRequest(request);

                if (request.HttpMethod.ToUpper() == "GET")
                {
                    await HandleGetRequestAsync(request, response);
                }
                else
                {
                    await SendErrorResponseAsync(response, 405, "Method Not Allowed", 
                        $"Method {request.HttpMethod} is not supported. Only GET requests are allowed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request: {ex.Message}");
                
                try
                {
                    await SendErrorResponseAsync(response, 500, "Internal Server Error", 
                        "An internal server error occurred.");
                }
                catch
                {
                }
            }
            finally
            {
                try
                {
                    response.Close();
                }
                catch
                {
                }
            }
        }

        private async Task HandleGetRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            string requestPath = request.Url.LocalPath;
            
            string htmlContent;
            
            switch (requestPath.ToLower())
            {
                case "/":
                case "/index":
                case "/index.html":
                    htmlContent = GenerateWelcomePage();
                    break;
                    
                case "/about":
                    htmlContent = GenerateAboutPage();
                    break;
                    
                case "/status":
                    htmlContent = GenerateStatusPage();
                    break;
                    
                default:
                    await SendErrorResponseAsync(response, 404, "Page Not Found", 
                        $"The requested page '{requestPath}' was not found on this server.");
                    return;
            }

            await SendHtmlResponseAsync(response, htmlContent);
        }

        private string GenerateWelcomePage()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Welcome to My C# Web Server</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f5f5f5;
        }
        .container {
            background-color: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        h1 {
            color: #333;
            text-align: center;
        }
        .welcome {
            color: #666;
            text-align: center;
            font-size: 18px;
            margin: 20px 0;
        }
        .info {
            background-color: #e7f3ff;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
        }
        .nav {
            text-align: center;
            margin: 20px 0;
        }
        .nav a {
            margin: 0 10px;
            color: #007acc;
            text-decoration: none;
        }
        .nav a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <div class='container'>
        <h1>🚀 Welcome to My C# Web Server!</h1>
        <p class='welcome'>This is a simple web server built using only .NET standard libraries.</p>
        
        <div class='info'>
            <h3>Server Information:</h3>
            <ul>
                <li>Server Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"</li>
                <li>Built with: C# and .NET HttpListener</li>
                <li>Port: 8080</li>
                <li>No external dependencies!</li>
            </ul>
        </div>
        
        <div class='nav'>
            <a href='/'>Home</a>
            <a href='/about'>About</a>
            <a href='/status'>Status</a>
        </div>
        
        <p style='text-align: center; color: #888; margin-top: 30px;'>
            
        </p>
    </div>
</body>
</html>";
        }

        private string GenerateAboutPage()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>About - C# Web Server</title>
    <style>
        body { font-family: Arial, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px; background-color: #f5f5f5; }
        .container { background-color: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        h1 { color: #333; text-align: center; }
        .nav { text-align: center; margin: 20px 0; }
        .nav a { margin: 0 10px; color: #007acc; text-decoration: none; }
        .nav a:hover { text-decoration: underline; }
    </style>
</head>
<body>
    <div class='container'>
        <h1>About This Server</h1>
        <p>This web server is a demonstration of creating a simple HTTP server using only .NET standard libraries.</p>
        
        <h3>Features:</h3>
        <ul>
            <li>HTTP GET request handling</li>
            <li>Multiple page routing</li>
            <li>Request logging with client IP and path</li>
            <li>Clean HTML responses</li>
            <li>Error handling (404, 405, 500)</li>
        </ul>
        
        <h3>Technology Stack:</h3>
        <ul>
            <li>C# Programming Language</li>
            <li>.NET HttpListener Class</li>
            <li>Async/Await Pattern</li>
            <li>No external dependencies</li>
        </ul>
        
        <div class='nav'>
            <a href='/'>Home</a>
            <a href='/about'>About</a>
            <a href='/status'>Status</a>
        </div>
    </div>
</body>
</html>";
        }

        private string GenerateStatusPage()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Server Status - C# Web Server</title>
    <style>
        body { font-family: Arial, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px; background-color: #f5f5f5; }
        .container { background-color: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        h1 { color: #333; text-align: center; }
        .status-ok { color: #28a745; font-weight: bold; }
        .nav { text-align: center; margin: 20px 0; }
        .nav a { margin: 0 10px; color: #007acc; text-decoration: none; }
        .nav a:hover { text-decoration: underline; }
    </style>
</head>
<body>
    <div class='container'>
        <h1>Server Status</h1>
        <p class='status-ok'>✅ Server is running normally</p>
        
        <h3>System Information:</h3>
        <ul>
            <li>Current Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"</li>
            <li>Server Uptime: Since server start</li>
            <li>Platform: " + Environment.OSVersion.Platform + @"</li>
            <li>.NET Version: " + Environment.Version + @"</li>
        </ul>
        
        <div class='nav'>
            <a href='/'>Home</a>
            <a href='/about'>About</a>
            <a href='/status'>Status</a>
        </div>
    </div>
</body>
</html>";
        }

        private async Task SendHtmlResponseAsync(HttpListenerResponse response, string htmlContent)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(htmlContent);
            
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            response.StatusCode = 200;
            
            using (Stream output = response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        private async Task SendErrorResponseAsync(HttpListenerResponse response, int statusCode, 
            string statusText, string message)
        {
            string errorHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Error {statusCode} - {statusText}</title>
    <style>
        body {{ font-family: Arial, sans-serif; text-align: center; margin-top: 50px; }}
        .error {{ color: #d32f2f; }}
    </style>
</head>
<body>
    <h1 class='error'>Error {statusCode}</h1>
    <h2>{statusText}</h2>
    <p>{message}</p>
    <a href='/'>Return to Home</a>
</body>
</html>";

            byte[] buffer = Encoding.UTF8.GetBytes(errorHtml);
            
            response.StatusCode = statusCode;
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            
            using (Stream output = response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        private void LogRequest(HttpListenerRequest request)
        {
            string clientIP = request.RemoteEndPoint?.Address?.ToString() ?? "Unknown";
            string requestPath = request.Url?.LocalPath ?? "/";
            string method = request.HttpMethod ?? "Unknown";
            string userAgent = request.UserAgent ?? "Unknown";
            
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                              $"{method} {requestPath} - " +
                              $"Client: {clientIP} - " +
                              $"User-Agent: {userAgent}";
            
            Console.WriteLine(logMessage);
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
            Console.WriteLine("Web server stopped.");
        }

        public void Dispose()
        {
            Stop();
            _listener?.Close();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            WebServer server = null;
            
            try
            {
                server = new WebServer(8080);
                
                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true;
                    server?.Stop();
                };
                
                await server.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }
            finally
            {
                server?.Dispose();
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
