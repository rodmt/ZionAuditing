# ZionAuditing

A comprehensive auditing system built with .NET 10, implementing Clean Architecture, Domain-Driven Design, and CQRS patterns.

## Architecture

This project follows Clean Architecture principles with the following layers:

### Domain Layer (`ZionAuditing.Domain`)
- **Entities**: Core business entities (AuditLog, AuditEntry)
- **Value Objects**: Immutable objects (AuditAction, EntityName, UserId)
- **Domain Events**: Events that occur within the domain
- **Repositories**: Repository interfaces
- **Common**: Base classes for Entity, AggregateRoot, ValueObject, Result pattern

### Application Layer (`ZionAuditing.Application`)
- **Commands**: Write operations using CQRS pattern
- **Queries**: Read operations using CQRS pattern
- **DTOs**: Data Transfer Objects
- **Behaviors**: MediatR pipeline behaviors (Validation, Logging)
- **Validators**: FluentValidation validators

### Infrastructure Layer (`ZionAuditing.Infrastructure`)
- **Persistence**: Database implementations using Dapper
- **Repositories**: Repository implementations
- **Database**: SQL Server schema and migrations

### API Layer (`ZionAuditing.Api`)
- **Endpoints**: FastEndpoints implementation
- **Configuration**: Application configuration
- **Middleware**: Custom middleware

## Technology Stack

### Core Framework
- **.NET 10**: Latest .NET framework
- **Clean Architecture**: Separation of concerns and dependency inversion
- **Domain-Driven Design (DDD)**: Rich domain models, value objects, aggregates
- **CQRS**: Command Query Responsibility Segregation with MediatR

### Data Access & Persistence
- **Dapper**: Lightweight ORM for data access
- **SQL Server**: Relational database

### API & Web
- **FastEndpoints**: High-performance endpoint library
- **FluentValidation**: Validation library
- **Serilog**: Structured logging
- **Problem Details**: RFC 7807 error handling
- **Swagger/OpenAPI**: API documentation

### Cloud-Native & Orchestration
- **.NET Aspire**: Cloud-native orchestration and service discovery
- **OpenTelemetry**: Observability and distributed tracing
- **Health Checks**: Built-in health monitoring

### Testing
- **xUnit**: Unit testing framework
- **Shouldly**: Fluent assertion library
- **NSubstitute**: Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing
- **Testcontainers**: Container-based integration tests
- **Arrange-Act-Assert Pattern**: Consistent test structure

### Build & Package Management
- **Centralized Package Management**: Directory.Packages.props for version control

## Project Structure

```
ZionAuditing/
├── src/
│   ├── ZionAuditing.Domain/          # Domain layer - business logic
│   │   ├── Common/                   # Base classes and interfaces
│   │   ├── Entities/                 # Domain entities
│   │   ├── ValueObjects/             # Value objects
│   │   ├── Events/                   # Domain events
│   │   ├── Repositories/             # Repository interfaces
│   │   └── Exceptions/               # Domain exceptions
│   │
│   ├── ZionAuditing.Application/     # Application layer - use cases
│   │   ├── Common/                   # CQRS abstractions
│   │   ├── AuditLogs/               # Audit log features
│   │   │   ├── Commands/            # Write operations
│   │   │   ├── Queries/             # Read operations
│   │   │   └── DTOs/                # Data transfer objects
│   │   └── Behaviors/               # MediatR behaviors
│   │
│   ├── ZionAuditing.Infrastructure/  # Infrastructure layer
│   │   ├── Persistence/             # Data access
│   │   │   └── Repositories/        # Repository implementations
│   │   ├── Database/                # SQL scripts
│   │   └── Configuration/           # Infrastructure configuration
│   │
│   ├── ZionAuditing.Api/            # API layer
│   │   ├── Endpoints/               # FastEndpoints
│   │   ├── Configuration/           # API configuration
│   │   └── Middleware/              # Custom middleware
│   │
│   ├── ZionAuditing.AppHost/        # .NET Aspire orchestration
│   │   └── Program.cs               # Service orchestration
│   │
│   └── ZionAuditing.ServiceDefaults/ # Shared Aspire configuration
│       └── Extensions.cs            # Observability & health checks
│
├── test/
│   ├── ZionAuditing.Domain.UnitTests/        # Domain layer tests
│   │   ├── Entities/                         # Entity tests
│   │   ├── ValueObjects/                     # Value object tests
│   │   ├── Common/                           # Common tests
│   │   └── Builders/                         # Test builders
│   │
│   ├── ZionAuditing.Application.UnitTests/   # Application layer tests
│   │   ├── AuditLogs/Commands/              # Command handler tests
│   │   ├── AuditLogs/Queries/               # Query handler tests
│   │   └── Behaviors/                       # Behavior tests
│   │
│   ├── ZionAuditing.Infrastructure.UnitTests/ # Infrastructure tests
│   │   └── Persistence/Repositories/        # Repository tests
│   │
│   └── ZionAuditing.Api.IntegrationTests/   # API integration tests
│       ├── Endpoints/                       # Endpoint tests
│       └── Fixtures/                        # Test fixtures
│
├── Directory.Build.props            # Common build properties
├── Directory.Packages.props         # Centralized package versions
└── .editorconfig                    # Code style configuration
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 or later / Visual Studio Code / Rider

### Database Setup

1. Update the connection string in `src/ZionAuditing.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ZionAuditing;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

2. Run the SQL schema from `src/ZionAuditing.Infrastructure/Database/schema.sql` to create the database tables.

### Running the Application

#### Option 1: Using .NET Aspire (Recommended)

```bash
# Run the Aspire AppHost
cd src/ZionAuditing.AppHost
dotnet run
```

This will start:
- SQL Server container
- API with service discovery
- Aspire Dashboard for monitoring

Access the Aspire Dashboard at the URL shown in the console output.

#### Option 2: Run API Directly

```bash
# Navigate to the API project
cd src/ZionAuditing.Api

# Run the application
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001
- Swagger UI: https://localhost:5001/swagger
- Health Checks: http://localhost:5000/health

## API Endpoints

### Audit Logs

- `POST /api/audit-logs` - Create a new audit log entry
- `GET /api/audit-logs/{id}` - Get audit log by ID

### Request Example

```json
POST /api/audit-logs
{
  "entityName": "User",
  "entityId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "admin@example.com",
  "action": "Update",
  "oldValues": "{\"name\": \"John Doe\", \"email\": \"john@example.com\"}",
  "newValues": "{\"name\": \"John Smith\", \"email\": \"john.smith@example.com\"}",
  "ipAddress": "192.168.1.1",
  "userAgent": "Mozilla/5.0..."
}
```

## Design Patterns

### Domain-Driven Design (DDD)
- **Entities**: Objects with unique identity that persist over time
- **Value Objects**: Immutable objects defined by their attributes
- **Aggregates**: Cluster of entities and value objects with defined boundaries
- **Domain Events**: Events that domain experts care about
- **Repository Pattern**: Abstraction for data access

### CQRS (Command Query Responsibility Segregation)
- **Commands**: Operations that change state
- **Queries**: Operations that read state
- **Handlers**: Process commands and queries
- **MediatR**: In-process messaging for decoupling

### Result Pattern
- Explicit success/failure handling
- Type-safe error handling
- No exceptions for business rule violations

## Code Quality

- **EditorConfig**: Enforced code style and formatting rules
- **Nullable Reference Types**: Enabled for null safety
- **Code Analyzers**: Microsoft.CodeAnalysis.NetAnalyzers enabled
- **Warnings as Errors**: Strict compilation settings

## Logging

Serilog is configured with multiple sinks:
- **Console**: Development-friendly output
- **File**: Rolling file logging in `logs/` directory
- **SQL Server**: Structured logging to database

## Error Handling

The API implements RFC 7807 Problem Details for consistent error responses:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Validation failed",
  "traceId": "0HMQ4V7L6QJ3K:00000001"
}
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License.

## Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [FastEndpoints Documentation](https://fast-endpoints.com/)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Serilog Documentation](https://serilog.net/)

## Running Tests

### Run All Tests

```bash
# Run all tests in solution
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Run Specific Test Projects

```bash
# Domain unit tests
dotnet test test/ZionAuditing.Domain.UnitTests

# Application unit tests
dotnet test test/ZionAuditing.Application.UnitTests

# Infrastructure unit tests
dotnet test test/ZionAuditing.Infrastructure.UnitTests

# API integration tests
dotnet test test/ZionAuditing.Api.IntegrationTests
```

### Test Organization

All tests follow the **Arrange-Act-Assert (AAA)** pattern:

```csharp
[Fact]
public void Method_Scenario_ExpectedBehavior()
{
    // Arrange
    var input = "test data";
    var expected = "expected result";

    // Act
    var actual = MethodUnderTest(input);

    // Assert
    actual.ShouldBe(expected);
}
```

## .NET Aspire Integration

This solution uses .NET Aspire for cloud-native development and orchestration.

### Features

- **Service Orchestration**: Automatically manages service dependencies
- **SQL Server Integration**: Containerized SQL Server with automatic connection string management
- **OpenTelemetry**: Built-in distributed tracing and metrics
- **Health Checks**: Automatic health monitoring for all services
- **Service Discovery**: Automatic service-to-service communication
- **Dashboard**: Real-time monitoring and diagnostics

### Configuration

The AppHost configures:
- SQL Server container with data persistence
- API service with database reference
- Health endpoints (`/health`, `/alive`)
- OpenTelemetry exporters

## Centralized Package Management

This solution uses centralized package management via `Directory.Packages.props`:

### Benefits

- **Single source of truth** for package versions
- **Easier updates** - change version in one place
- **Consistency** across all projects
- **Transitive pinning** prevents version conflicts

### Adding New Packages

1. Add package version to `Directory.Packages.props`:
   ```xml
   <PackageVersion Include="PackageName" Version="x.y.z" />
   ```

2. Reference in project without version:
   ```xml
   <PackageReference Include="PackageName" />
   ```

