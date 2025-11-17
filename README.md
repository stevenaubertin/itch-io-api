# Itch.io API

![.NET CI](https://github.com/stevenaubertin/itch-io-api/workflows/.NET%20CI/badge.svg)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A .NET 10.0 Web API for interacting with the itch.io platform.

## Features

- Built with .NET 10.0 and C# latest version
- RESTful API with controller-based routing
- Swagger/OpenAPI specification for API documentation
- XML documentation comments for detailed API description
- Sample Games controller with CRUD operations

## Getting Started

### Prerequisites

- .NET SDK 10.0 or later

### Running the API

1. Clone or navigate to the project directory
2. Run the application:

```bash
dotnet run
```

3. The API will start on HTTPS (typically `https://localhost:5001`) and HTTP (`http://localhost:5000`)

### Accessing Swagger UI

Once the application is running in Development mode, you can access the interactive Swagger UI documentation at:

```
https://localhost:5001/swagger
```

Or via the OpenAPI endpoint:

```
https://localhost:5001/openapi/v1.json
```

## API Endpoints

### Games

- `GET /api/games` - Get all games (with pagination)
  - Query parameters: `page` (default: 1), `pageSize` (default: 20)
  
- `GET /api/games/{id}` - Get a specific game by ID
  
- `GET /api/games/search` - Search games by title or tags
  - Query parameter: `query` (search term)

## Project Structure

```
itch-io-api/
├── Controllers/
│   ├── GamesController.cs       # Games API endpoints
│   └── WeatherForecastController.cs  # Sample controller (can be removed)
├── Properties/
│   └── launchSettings.json      # Launch configuration
├── appsettings.json             # Application settings
├── Program.cs                   # Application entry point and configuration
└── ItchIoApi.csproj             # Project file
```

## Configuration

The API is configured in `Program.cs` with:
- Controllers for API endpoints
- Swagger/OpenAPI documentation
- HTTPS redirection
- Authorization middleware

## Development

### Adding New Endpoints

1. Create a new controller in the `Controllers/` directory
2. Inherit from `ControllerBase`
3. Add the `[ApiController]` and `[Route("api/[controller]")]` attributes
4. Implement your endpoints with proper HTTP method attributes (`[HttpGet]`, `[HttpPost]`, etc.)
5. Add XML documentation comments for Swagger

### Building

```bash
dotnet build
```

### Publishing

```bash
dotnet publish -c Release
```

## Next Steps

- Implement actual itch.io API integration
- Add authentication/authorization
- Add database support for caching
- Implement additional endpoints for:
  - Game jams
  - User profiles
  - Game uploads
  - Sales and bundles

## Technologies Used

- ASP.NET Core 10.0
- Swashbuckle.AspNetCore 10.0.1 (Swagger/OpenAPI)
- Microsoft.OpenApi 2.3.0

## Contributing

We welcome contributions! Please read our [Contributing Guidelines](CONTRIBUTING.md) for details on:

- Development workflow
- **Merge policy**: All PRs to `main` use **squash merge**
- **Branch deletion**: Branches are automatically deleted after merge
- Code style guidelines
- Testing requirements

### Quick Start for Contributors

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes and commit (`git commit -m 'Add amazing feature'`)
4. Push to your branch (`git push -u origin feature/amazing-feature`)
5. Open a Pull Request

All PRs must pass CI checks before merging.

## CI/CD

This project uses GitHub Actions for continuous integration:

- **Build and Test**: Automatically builds and tests the project on every PR
- **Docker Build**: Validates Docker image builds
- **Status checks required** before merging to `main`

## License

This is a sample project for demonstration purposes.
