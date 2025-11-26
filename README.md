# Day 5 — Mini Project: Simple Inventory System API

This is a minimal ASP.NET Core Web API (net7.0) implementing:
- Product CRUD
- Category CRUD
- Search endpoint: `/products/search?name=abc`

The API uses EF Core InMemory provider so no external DB is required.

Run (PowerShell):

```powershell
cd "c:\Users\faisalshahid\Desktop\Learning .NET\Day 1\Day5-MiniProject"
# restore and run
dotnet restore
dotnet run
```

When running, Swagger UI will be available at `https://localhost:5001/swagger` (development environment).

Notes:
- `Product` has `CategoryId` and the API validates the category exists on create/update.
- Search is case-insensitive and checks if the product `Name` contains the query string.
