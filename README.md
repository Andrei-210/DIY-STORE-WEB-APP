# DIY STORE

A full-featured e-commerce web application for a DIY (Do It Yourself) retail store, built with ASP.NET Core MVC on .NET 10. The platform supports product browsing, shopping cart management, order placement, user account management, and a complete administrative back-end.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Database](#database)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Default Administrator Account](#default-administrator-account)
- [Author](#author)

---

## Overview

DIY STORE is a multi-layered MVC web application that simulates a real-world home improvement and hardware retail environment. Users can browse products organized by categories and subcategories, view product details, leave reviews, manage a shopping cart, and place orders. Store administrators have access to a dedicated dashboard for managing products, categories, shops, and customer messages.

---

## Features

**Customer-facing**

- Product catalog with category and subcategory navigation
- Product detail pages with image galleries, specifications, pricing (including sale prices), and stock availability
- Customer reviews and ratings on product pages
- Shopping cart with session-based persistence
- Checkout and order confirmation flow
- User registration, login, and profile management (including profile picture upload)
- Favorites list for saving products
- Contact and support form

**Administration**

- Dedicated admin dashboard accessible to users with the Admin role
- Full CRUD management for products, categories, subcategories, and shops
- View and manage customer support messages
- Role-based access control enforced throughout the application

---

## Technology Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Language | C# |
| ORM | Entity Framework Core 10 |
| Database | Microsoft SQL Server (LocalDB for development) |
| Authentication | ASP.NET Core Identity |
| Front-end | Razor Views, Bootstrap 5, jQuery |
| Session | Distributed Memory Cache (server-side sessions) |
| IDE | Visual Studio 2022 |

---

## Architecture

The application follows a layered architecture with clear separation of concerns:

- **Controllers** handle HTTP requests and coordinate responses.
- **Services** contain business logic and are consumed by controllers through interfaces.
- **Repositories** abstract all data access operations and implement the Repository pattern, each backed by a corresponding interface.
- **Models** define the domain entities and view models.
- **Views** are Razor templates organized by controller area.

Dependency injection is configured in `Program.cs`, where all services and repositories are registered with a scoped lifetime.

---

## Project Structure

```
DIY STORE/
  DIY STORE/
    Controllers/        # HTTP request handlers (Account, Admin, Cart, Products, Shops, etc.)
    Data/               # ApplicationDbContext and ApplicationUser definitions
    Migrations/         # Entity Framework Core migration history
    Models/             # Domain entities and view models
    Repositories/       # Data access layer (implementations and interfaces)
    Services/           # Business logic layer (implementations and interfaces)
    Views/              # Razor view templates organized by controller
    wwwroot/            # Static assets (CSS, JavaScript, images, vendor libraries)
    Program.cs          # Application entry point and service registration
    appsettings.json    # Application configuration
```

---

## Database

The application uses SQL Server with Entity Framework Core Code-First migrations. The main entities are:

- `Product` - catalog item with name, price, stock, sale information, slug, manufacturer, and subcategory reference
- `Category` and `Subcategory` - two-level product classification hierarchy
- `Shop` - physical store locations with address, contact, and working hours
- `Order` and `OrderItem` - customer orders and line items
- `Review` - customer product reviews
- `Favorite` - user-saved products
- `ContactMessage` - customer support submissions
- `ProductImage` and `ProductSpecification` - supplementary product data
- `ProductShopAvailability` - per-shop stock availability mapping
- `ApplicationUser` - extended Identity user with profile picture support

Migration history is tracked under the `Migrations/` folder. All schema changes are applied through EF Core migrations.

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- Microsoft SQL Server or SQL Server LocalDB
- Visual Studio 2022 (recommended) or any editor with C# support

### Setup

1. Clone or extract the repository.

2. Open the solution file `DIY STORE.slnx` in Visual Studio, or navigate to the project directory in a terminal.

3. Update the connection string in `appsettings.json` if your SQL Server instance differs from the default LocalDB configuration.

4. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

5. Run the application:

   ```bash
   dotnet run
   ```

   Or press F5 in Visual Studio.

6. Navigate to `https://localhost:{port}` in your browser.

---

## Configuration

The primary configuration file is `appsettings.json`. The key setting is the database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DIYStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

For development overrides, use `appsettings.Development.json`.

Session idle timeout is set to 60 minutes and can be adjusted in `Program.cs`.

---

## Default Administrator Account

On first run, the application seeds two roles (`Admin` and `Customer`) and creates a default administrator account if one does not already exist:

- **Email:** `admin@diystore.com`
- **Password:** `Admin123!`

It is strongly recommended to change these credentials before deploying to any environment beyond local development.

---

## Author

- **Nicoli Andrei - Claudiu**
- **Third-year student at University of Craiova, Faculty of Automatics, Computer Science and Engineering**

---

## License

This project is licensed under the [MIT License](LICENSE).
