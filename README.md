# RentalApp - Peer-to-Peer Rental Marketplace

A .NET MAUI mobile application for peer-to-peer item rental, built for SET09102 Software Engineering coursework.

## Features

- User authentication (login and registration)
- Browse available items for rent
- Create item listings with title, description, daily rate, category and location
- Request rentals with date selection
- Approve or reject incoming rental requests
- View outgoing rental requests and their status
- Rental workflow: Requested → Approved/Rejected
- Double-booking prevention
- Automated price calculation based on daily rate

## Tech Stack

- .NET 9.0 / .NET MAUI
- PostgreSQL 16 (via Docker)
- Entity Framework Core
- xUnit + Moq (testing)
- GitHub Actions (CI/CD)

## Prerequisites

- .NET 9.0 SDK
- Docker Desktop
- Android Emulator (via Android Studio)

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/Jamie-Rutherford/RentalApp.git
cd RentalApp
```

### 2. Start the database
```bash
docker compose up -d
```

### 3. Configure the connection string
Copy `StarterApp.Database/appsettings.json.template` to `StarterApp.Database/appsettings.json` and update:
```json
{
  "ConnectionStrings": {
    "DevelopmentConnection": "Host=localhost;Username=rental_user;Password=rental_pass;Database=rental_db"
  }
}
```

### 4. Run migrations
```bash
dotnet run --project StarterApp.Migrations
```

### 5. Build and run
```bash
dotnet build StarterApp/StarterApp.csproj -f net9.0-android -t:Run
```

## Running Tests
```bash
dotnet test StarterApp.Test/StarterApp.Test.csproj --verbosity normal
```

## Architecture

The app follows a three-layer architecture:
- **Views + ViewModels** (MVVM pattern) — MAUI UI layer
- **Services** (RentalService) — Business logic layer
- **Repositories** (ItemRepository, RentalRepository) — Data access layer
- **PostgreSQL** — Database via Entity Framework Core

## CI/CD

GitHub Actions pipeline automatically builds and runs tests on every push to main.
