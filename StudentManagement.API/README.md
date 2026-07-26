# SM-WebApi
 Student Management Web API
A robust, high-performance, and scalable RESTful Web API for managing student records. Built with .NET Core, this project strictly follows Clean Architecture principles to ensure separation of concerns, maintainability, and testability.

?? Key Features


Clean Architecture (N-Tier): Organized into Domain, Application, and Infrastructure layers.
Manual Authentication System: Secure custom authentication mechanism implemented from scratch without relying on default Identity frameworks.
High Performance with ADO.NET: Direct database operations using ADO.NET for query optimization and maximum performance, avoiding the overhead of ORMs.
In-Memory Caching: Implemented caching to reduce database hits and improve response times for frequently accessed data.
Advanced Logging: Comprehensive logging mechanism to track application flow and errors.
Global Exception Handling: Custom middleware to catch and handle exceptions globally.
Correlation ID Middleware: Injects a unique Correlation ID into every request for easy tracing and debugging across the system.
FluentValidation: Used for robust and clean DTO validation.





??? Tech Stack
Framework: .NET Core Web API
Architecture: Clean Architecture / N-Tier
Database Access: ADO.NET (Raw SQL / Stored Procedures)
Caching: In-Memory Cache (IMemoryCache)
Validation: FluentValidation
Tools: Visual Studio / VS Code, Git, GitHub





?? Project Structure
The solution is divided into the following projects/layers to maintain a clean architecture:



StudentManagement.Solution/??? StudentManagement.Domain/       # Core entities, interfaces, and business logic??? StudentManagement.Infrastructure/ # Database implementations (ADO.NET), caching, external services??? StudentManagement.API/          # Presentation layer (Controllers, Middleware, DTOs, Program.cs)
StudentManagement.Solution/??? StudentManagement.Domain/       # Core entities, interfaces, and business logic??? StudentManagement.Infrastructure/ # Database implementations (ADO.NET), caching, external services??? StudentManagement.API/          # Presentation layer (Controllers, Middleware, DTOs, Program.cs)