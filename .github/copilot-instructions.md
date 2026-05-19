# Copilot Instructions — TaskApi

## Build & Run

```bash
cd src
dotnet restore
dotnet run
```

Swagger UI at `https://localhost:5001/swagger`.

### Database setup (first time)

```bash
dotnet tool install --global dotnet-ef
cd src
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Architecture

This is a single-project ASP.NET Core Web API (.NET 10) for task/project management.

- **Controller → Service → EF Core DbContext** — standard layered pattern, all in one project under `src/`.
- **Database**: SQLite in dev (`tasks.db`), SQL Server in prod. EF Core with code-first migrations.
- **Namespaces**: `TaskApi.Controllers`, `TaskApi.Services`, `TaskApi.Models`, `TaskApi.Data`.
- **Models**: `TaskItem`, `User`, `Project` — all defined in `Models.cs`. `TaskItem` is the central entity.
- **NotificationService**: Sends emails via SMTP. Currently not wired into the controller flow (assignment notification is a TODO in `TaskService.AssignTask`).

## Key Conventions

- **Status values are magic strings**: `"Open"`, `"InProgress"`, `"Done"`, `"Closed"` for tasks. Priority: `"Low"`, `"Medium"`, `"High"`, `"Critical"`.
- **Status is not updated via PUT** — `UpdateTask` deliberately skips the `Status` field. A dedicated status endpoint is intended but not yet implemented.
- **New tasks always start as `"Open"`** with `CreatedAt = DateTime.Now` (set in `TaskService.CreateTask`, not the controller).
- **Services are registered as Scoped** in `Program.cs` and injected as concrete types (no interfaces).
- **No authentication or authorization yet** — marked as TODOs in `Program.cs`.
- **No test project exists yet.** When adding tests, follow the .NET convention: create a `tests/TaskApi.Tests/` project using xUnit.

## Known Technical Debt

These are documented in code via TODO/FIXME comments and are part of the backlog:

- `SearchTasks` loads all tasks into memory before filtering (FIXME in `TaskService`).
- SMTP config is hardcoded in `NotificationService` — should move to `appsettings.json`.
- No input validation on the Create endpoint.
- `CountActiveTasks` loads all tasks into memory unnecessarily.
