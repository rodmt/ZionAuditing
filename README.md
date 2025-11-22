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

- **.NET 10**: Latest .NET framework
- **Clean Architecture**: Separation of concerns and dependency inversion
- **Domain-Driven Design (DDD)**: Rich domain models, value objects, aggregates
- **CQRS**: Command Query Responsibility Segregation with MediatR
- **Dapper**: Lightweight ORM for data access
- **SQL Server**: Relational database
- **FastEndpoints**: High-performance endpoint library
- **FluentValidation**: Validation library
- **Serilog**: Structured logging
- **Problem Details**: RFC 7807 error handling
- **Swagger/OpenAPI**: API documentation

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
│   └── ZionAuditing.Api/            # API layer
│       ├── Endpoints/               # FastEndpoints
│       ├── Configuration/           # API configuration
│       └── Middleware/              # Custom middleware
│
├── build/                           # Build output
├── samples/                         # Sample code
├── test/                            # Unit and integration tests
├── Directory.Build.props            # Common build properties
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
