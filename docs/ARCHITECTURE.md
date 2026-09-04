# Architecture Documentation

## 1. Architecture Overview

The project uses **Clean Architecture** with four projects:

```text
PlaylistApi.Domain
        ↑
PlaylistApi.Application
        ↑
PlaylistApi.Infrastructure

PlaylistApi.API ─────→ Application
        │
        └────────────→ Infrastructure
```

### Project Dependencies

- **Infrastructure → Application → Domain**
- **API → Application**
- **API → Infrastructure**
- **Domain** has no dependencies on other projects.

This dependency structure keeps the core business logic independent from technical details such as databases, HTTP, authentication, and external APIs.

---

## 2. Project Responsibilities

### Domain

Contains the core business entities and rules.

- `Playlist`
- `Song`
- `PlaylistSong`

The Domain does not depend on EF Core, SQL Server, ASP.NET Identity, JWT, HTTP, or iTunes.

### Application

Contains the application's business logic and abstractions.

- Services and use cases
- DTOs
- Repository interfaces
- External-service interfaces
- Authentication/identity abstractions
- Custom exceptions
- Mappings

The Application layer depends only on the Domain and does **not** depend on Infrastructure.

### Infrastructure

Contains implementations of technical concerns defined by the Application layer.

- EF Core and SQL Server
- `AppDbContext`
- Repositories
- ASP.NET Core Identity
- JWT generation
- Current-user implementation
- iTunes API integration
- Database configurations and migrations

For example, `IPlaylistRepository` is defined in Application while `PlaylistRepository` implements it in Infrastructure.

### API

The entry point of the application.

- Controllers
- Middleware
- `Program.cs`
- Dependency Injection configuration

Controllers are responsible for HTTP concerns and delegate business logic to Application services.

---

## 3. Dependency Injection

The project uses **Dependency Injection** to connect abstractions with their implementations.

Application defines what it needs through interfaces, while Infrastructure provides the implementations.

This follows the **Dependency Inversion Principle** and makes components easier to replace and test.

---

## 4. DTOs and Mapping

DTOs are used to define the API contract instead of exposing Domain entities directly.

Examples:

- `CreatePlaylistRequest`
- `UpdatePlaylistRequest`
- `PlaylistResponse`
- `AddSongRequest`
- `SongSearchResponse`

This keeps the API contract independent from the internal domain/database models.

---

## 5. Authentication and Authorization

The project uses **ASP.NET Core Identity + JWT**.

- Identity manages users and validates credentials.
- JWT is generated after successful login.
- The token contains claims such as the user's ID and email.
- Protected endpoints require a valid JWT.
- `CurrentUserService` retrieves the authenticated user's ID from the JWT claims.


Identity and JWT implementations are kept in **Infrastructure**, while Application depends only on abstractions such as `IIdentityService` and `IJwtTokenService`.

---

## 6. External Music Service

The application uses the **Apple iTunes Search API** to search for real songs.

The Application layer depends on:

```text
IMusicService
```

Infrastructure provides:

```text
ITunesMusicService
```

This keeps iTunes-specific HTTP requests and response models inside Infrastructure.

If the music provider changes in the future, another implementation of `IMusicService` can be added without changing the Application layer.

---

## 7. Global Exception Handling

`ExceptionHandlingMiddleware` provides centralized exception handling at the API layer.

Application services throw custom exceptions such as:

- `ValidationException` → **400 Bad Request**
- `NotFoundException` → **404 Not Found**
- `ConflictException` → **409 Conflict**
- Unexpected exceptions → **500 Internal Server Error**

This keeps error-handling logic out of individual controllers and provides consistent API responses.

---

## 8. Main Architectural Decisions

| Decision | Reason |
|---|---|
| Clean Architecture | Separates business logic from technical concerns and improves maintainability and testability |
| SQL Server | Strong fit with ASP.NET Core, EF Core and Identity |
| Repository Pattern | Keeps database access out of Application |
| DTOs | Protects domain models and controls the API contract |
| Dependency Injection | Reduces coupling and supports testing/replacement |
| Identity + JWT | Provides user management and stateless API authentication |
| iTunes through `IMusicService` | Keeps external API details isolated from business logic |
| Global Exception Middleware | Centralizes and standardizes error handling |

