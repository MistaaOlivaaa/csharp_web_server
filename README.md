# C# Web Server

A simple HTTP web server built using only .NET standard libraries, with no external dependencies.

## Features

- 🚀 Listens on port 8080
- 📡 Handles HTTP GET requests
- 🎨 Serves HTML pages with CSS styling
- 📝 Logs all incoming requests (client IP, method, path, user agent)
- 🛡️ Error handling for 404, 405, and 500 errors
- 🔀 Multiple page routing (/, /about, /status)
- ✨ Clean, well-commented code in a single .cs file

## Requirements

- .NET 6.0 or later
- No external packages required (uses only .NET standard libraries)

## Building and Running

### Option 1: Using .NET CLI (Recommended)

1. Navigate to the project directory:
   ```bash
   cd /home/saad/Bureau/csharp-webserver
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the server:
   ```bash
   dotnet run
   ```

### Option 2: Direct compilation

1. Compile the C# file directly:
   ```bash
   csc WebServer.cs
   ```

2. Run the executable:
   ```bash
   ./WebServer.exe    # On Windows
   mono WebServer.exe # On Linux/macOS with Mono
   ```

## Usage

1. Start the server using one of the methods above
2. Open your web browser and navigate to:
   - `http://localhost:8080/` - Main welcome page
   - `http://localhost:8080/about` - About page
   - `http://localhost:8080/status` - Server status page

3. Watch the console for request logs showing:
   - Timestamp
   - HTTP method
   - Requested path
   - Client IP address
   - User agent

4. Press `Ctrl+C` to stop the server gracefully

## Project Structure

```
csharp-webserver/
├── WebServer.cs      # Main web server implementation (single file)
├── WebServer.csproj  # .NET project file
└── README.md         # This documentation
```

## Code Highlights

- **Single File**: All code is contained in `WebServer.cs` for simplicity
- **Async/Await**: Uses modern async programming patterns
- **HttpListener**: Built on .NET's HttpListener class
- **Request Logging**: Comprehensive logging of all incoming requests
- **Error Handling**: Proper HTTP status codes and error pages
- **Clean HTML**: Well-structured HTML with embedded CSS
- **Multiple Routes**: Supports different pages and paths

## Technical Details

- **Port**: 8080 (configurable in code)
- **Supported Methods**: GET only (405 returned for others)
- **Content Type**: HTML with UTF-8 encoding
- **Logging**: Console output with timestamps
- **Error Pages**: Custom HTML error pages for different status codes

## Example Request Log Output

```
Web server started on http://localhost:8080
Press Ctrl+C to stop the server...
[2025-01-15 14:30:25] GET / - Client: 127.0.0.1 - User-Agent: Mozilla/5.0...
[2025-01-15 14:30:28] GET /about - Client: 127.0.0.1 - User-Agent: Mozilla/5.0...
[2025-01-15 14:30:35] GET /nonexistent - Client: 127.0.0.1 - User-Agent: Mozilla/5.0...
```

## License

This project is provided as-is for educational and demonstration purposes.
