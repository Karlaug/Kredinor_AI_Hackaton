# TaskApi

Internal REST API for task and project management.

## Stack

* .NET 10
* Entity Framework Core
* SQLite (dev), SQL Server (prod)

## Endpoints

|Method|Path|Description|
|-|-|-|
|GET|/api/tasks|List all tasks|
|GET|/api/tasks/{id}|Get task by id|
|POST|/api/tasks|Create new task|
|PUT|/api/tasks/{id}|Update task|
|DELETE|/api/tasks/{id}|Delete task|
|GET|/api/tasks/search|Search by query string|
|POST|/api/tasks/{id}/assign|Assign task to user|
|GET|/api/tasks/user/{id}|Get tasks for a user|
|POST|/api/tasks/bulk-import|Bulk-create tasks|

## Known issues

See backlog in Azure DevOps.

## Run locally

```

dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate

dotnet ef database update

dotnet restore

dotnet run

```
dotnet restore
dotnet run --project src
```

