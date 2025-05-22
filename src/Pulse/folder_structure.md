## Project Structure

```
Pulse/                        # Core library
├── Core/
│   ├── Entities/             # Core entity classes
│   ├── ValueObjects/         # Value objects
│   ├── Enums/                # Core enumerations
│   ├── Exceptions/           # Core exceptions
│   ├── Commands/             # Command definitions
│   ├── Queries/              # Query definitions
│   ├── Validators/           # Validation logic
│   └── Behaviors/            # Cross-cutting behaviors
│
├── Data/
│   ├── Context/              # DbContext implementations
│   ├── Configurations/       # Entity configurations
│   ├── Repositories/         # Repository implementations
│   ├── Migrations/           # EF Core migrations
│   ├── Entities/             # EF Core entity classes
│   └── Seeding/              # Database seeding logic
│
├── Infrastructure/
│   ├── Logging/              # Logging implementations
│   ├── Caching/              # Cache implementations
│   ├── Messaging/            # Message broker integrations
│   ├── Email/                # Email service implementations
│   ├── Storage/              # File storage implementations
│   ├── Security/             # Security services
│   └── ExternalServices/     # Third-party service clients
│
├── Services/
│   ├── Implementations/      # Service implementations
│   ├── Handlers/             # Command/Query handlers
│   ├── Factories/            # Object factories
│   ├── Adapters/             # Service adapters
│   └── Processors/           # Business process handlers
│
├── Events/
│   ├── Definitions/          # Integration event classes
│   ├── Handlers/             # Event handlers
│   ├── Publishers/           # Event publishing logic
│   └── Consumers/            # Event consumers
│
├── Contracts/
│   ├── Repositories/         # Repository interfaces
│   ├── Services/             # Service interfaces
│   ├── Infrastructure/       # Infrastructure interfaces
│   └── Handlers/             # Handler interfaces
│
└── Shared/
    ├── Constants/            # Constant definitions
    ├── Extensions/           # Extension methods
    ├── Helpers/              # Helper classes
    ├── Models/               # Shared models
    └── Utils/                # Utility classes
```