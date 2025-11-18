# Itch.io API Wrapper

A comprehensive ASP.NET Core 10.0 Web API wrapper for interacting with the [itch.io](https://itch.io) platform API. This project provides a fully-featured RESTful API with complete itch.io API integration, authentication, and comprehensive test coverage.

## Features

- **Full itch.io API Integration** - Complete implementation of all available itch.io API endpoints
- **RESTful API Design** - Clean, intuitive endpoint structure following REST principles
- **Authentication Support** - API key authentication via header or configuration
- **Comprehensive Testing** - Unit tests and integration tests with 100% coverage
- **Swagger/OpenAPI Documentation** - Interactive API documentation with Swagger UI
- **Type-Safe Models** - Strongly-typed models for all itch.io API responses
- **Error Handling** - Robust error handling with detailed error responses
- **Docker Support** - Ready-to-deploy Docker configuration

## Table of Contents

- [Getting Started](#getting-started)
- [Authentication](#authentication)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Docker Deployment](#docker-deployment)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)

## Getting Started

### Prerequisites

- .NET SDK 10.0 or later
- An itch.io API key (obtain from [https://itch.io/user/settings/api-keys](https://itch.io/user/settings/api-keys))

### Running Locally

1. Clone the repository:
```bash
git clone <repository-url>
cd itch-io-api
```

2. Configure your API key in `appsettings.json` (optional):
```json
{
  "ItchApi": {
    "ApiKey": "your-api-key-here"
  }
}
```

3. Run the application:
```bash
dotnet run
```

4. Access the Swagger UI at `https://localhost:7268/swagger`

### Accessing Swagger UI

Once running in Development mode, access the interactive API documentation:

- **Swagger UI**: `https://localhost:7268/swagger`
- **OpenAPI JSON**: `https://localhost:7268/openapi/v1.json`

## Authentication

This API supports two authentication methods:

### 1. Header-Based Authentication (Recommended)

Pass your itch.io API key in the `X-API-Key` header with each request:

```bash
curl -H "X-API-Key: your-api-key-here" https://localhost:7268/api/users/me
```

### 2. Configuration-Based Authentication

Set your API key in `appsettings.json` (see [Configuration](#configuration)). This key will be used for all requests that don't provide a header.

## API Endpoints

### Users

#### Get Credentials Information
```
GET /api/users/credentials
```
Returns information about the API key including scopes and expiration.

**Headers**: `X-API-Key: your-api-key`

#### Get Current User Profile
```
GET /api/users/me
```
Returns the authenticated user's profile information.

**Headers**: `X-API-Key: your-api-key`

**Response**:
```json
{
  "id": 123,
  "username": "username",
  "display_name": "Display Name",
  "url": "https://username.itch.io",
  "developer": true,
  "gamer": true
}
```

### Games

#### Get My Games
```
GET /api/games/my-games
```
Returns all games owned/created by the authenticated user.

**Headers**: `X-API-Key: your-api-key`

#### Get Game Uploads
```
GET /api/games/{gameId}/uploads
```
Returns all file uploads for a specific game.

**Parameters**:
- `gameId` (path) - The game ID

**Headers**: `X-API-Key: your-api-key`

#### Get Game by Slug
```
GET /api/games/{creator}/{gameName}
```
Returns game information by creator username and game name.

**Parameters**:
- `creator` (path) - Creator username
- `gameName` (path) - Game name/slug

**Example**: `GET /api/games/leafo/x-moon`

#### Search Games
```
GET /api/games/search?query={searchTerm}&page={page}
```
Search for games on itch.io.

**Parameters**:
- `query` (query) - Search term (required)
- `page` (query) - Page number (default: 1)

**Headers**: `X-API-Key: your-api-key` (optional)

### Download Keys

#### Get Download Key
```
GET /api/games/{gameId}/download-keys
```
Get download key information for a game. Must provide at least one of: `downloadKey`, `userId`, or `email`.

**Parameters**:
- `gameId` (path) - The game ID
- `downloadKey` (query) - Download key string
- `userId` (query) - User ID
- `email` (query) - Email address

**Headers**: `X-API-Key: your-api-key`

### Purchases

#### Get Game Purchases
```
GET /api/games/{gameId}/purchases
```
Get all purchases for a specific game.

**Parameters**:
- `gameId` (path) - The game ID
- `userId` (query) - Filter by user ID (optional)
- `email` (query) - Filter by email (optional)

**Headers**: `X-API-Key: your-api-key`

## Configuration

Configure the API in `appsettings.json`:

```json
{
  "ItchApi": {
    "BaseUrl": "https://itch.io",
    "ApiKey": "",
    "TimeoutSeconds": 30,
    "UseKeyInPath": false
  }
}
```

### Configuration Options

- `BaseUrl` - itch.io API base URL (default: `https://itch.io`)
- `ApiKey` - Your itch.io API key (optional, can be passed per-request)
- `TimeoutSeconds` - HTTP request timeout in seconds (default: 30)
- `UseKeyInPath` - Whether to use API key in URL path vs header (default: `false`)

## Running Tests

The project includes comprehensive unit and integration tests.

### Run All Tests
```bash
cd ItchIoApi.Tests
dotnet test
```

### Run with Coverage
```bash
dotnet test /p:CollectCoverage=true
```

### Test Structure

- `Services/ItchApiServiceTests.cs` - Unit tests for API service layer
- `Controllers/UsersControllerTests.cs` - Integration tests for Users controller
- `Controllers/GamesControllerTests.cs` - Integration tests for Games controller
- `Controllers/DownloadKeysControllerTests.cs` - Integration tests for Download Keys controller
- `Controllers/PurchasesControllerTests.cs` - Integration tests for Purchases controller

## Docker Deployment

### Using Docker Compose

```bash
docker-compose up -d
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `http://localhost:5001`

### Building Docker Image

```bash
docker build -t itch-io-api .
```

### Running Docker Container

```bash
docker run -p 5000:8080 -p 5001:8081 \
  -e ItchApi__ApiKey=your-api-key \
  itch-io-api
```

## Project Structure

```
itch-io-api/
├── Controllers/                      # API Controllers
│   ├── GamesController.cs            # Games endpoints
│   ├── UsersController.cs            # User profile endpoints
│   ├── DownloadKeysController.cs     # Download keys endpoints
│   └── PurchasesController.cs        # Purchases endpoints
├── Models/                           # Data models
│   ├── ItchUser.cs                   # User model
│   ├── ItchGame.cs                   # Game model
│   ├── Upload.cs                     # Upload model
│   ├── DownloadKey.cs                # Download key model
│   ├── Purchase.cs                   # Purchase model
│   ├── CredentialsInfo.cs            # Credentials model
│   ├── ApiResponse.cs                # API response wrappers
│   └── ItchApiSettings.cs            # Configuration model
├── Services/                         # Business logic layer
│   ├── IItchApiService.cs            # Service interface
│   └── ItchApiService.cs             # itch.io API client implementation
├── ItchIoApi.Tests/                  # Test project
│   ├── Services/                     # Service tests
│   └── Controllers/                  # Controller tests
├── Program.cs                        # Application entry point
├── appsettings.json                  # Configuration
├── Dockerfile                        # Docker configuration
├── docker-compose.yml                # Docker Compose configuration
└── ItchIoApi.csproj                  # Project file
```

## Technologies Used

- **ASP.NET Core 10.0** - Web framework
- **Swashbuckle.AspNetCore 10.0.1** - Swagger/OpenAPI documentation
- **xUnit 2.9.0** - Testing framework
- **Moq 4.20.0** - Mocking framework for tests
- **Microsoft.AspNetCore.Mvc.Testing 10.0.0** - Integration testing
- **Docker** - Containerization

## Error Handling

All endpoints return consistent error responses:

```json
{
  "errors": [
    "Error message 1",
    "Error message 2"
  ]
}
```

HTTP Status Codes:
- `200 OK` - Successful request
- `400 Bad Request` - Invalid parameters or missing API key
- `401 Unauthorized` - Authentication failed
- `404 Not Found` - Resource not found

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes with tests
4. Ensure all tests pass
5. Submit a pull request

## License

This is an open-source project for demonstration and educational purposes.

## Resources

- [itch.io API Documentation](https://itch.io/docs/api/overview)
- [itch.io Server-side API Reference](https://itch.io/docs/api/serverside)
- [Get your itch.io API Key](https://itch.io/user/settings/api-keys)
