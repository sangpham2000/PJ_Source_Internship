# 🚀 PJ_Source_GV Project

> **A comprehensive guide to building, running, and testing the PJ_Source_GV API**

This document provides step-by-step instructions for setting up and running the PJ_Source_GV API using multiple approaches: JetBrains Rider IDE, command line interface, and Docker for database management.

---

## 📋 Prerequisites

Before you begin, ensure you have the following installed on your system:

| Requirement | Description | Required For |
|-------------|-------------|--------------|
| **🔧 .NET 9 SDK** | This project is built with .NET 9 | All methods |
| **💻 JetBrains Rider** | IDE for .NET development | Method 1 (Optional) |
| **🐳 Docker Desktop** | Container platform | Method 3 (Required) |

---

## 🎯 Method 1: Using JetBrains Rider IDE

JetBrains Rider provides a powerful and integrated environment for .NET development.

### 🔨 Step 1: Build the Solution

1. In the **Solution Explorer**, right-click the `PJ_Source_GV` project
2. Select **Build Solution** from the context menu (or press `Ctrl+Shift+B`)
3. Monitor the progress in the **Build** tab at the bottom of the IDE

> ✅ Rider will compile the project and its dependencies automatically

### ▶️ Step 2: Launch the Project

1. Ensure the `PJ_Source_GV` project is selected in the Solution Explorer
2. Click the **Run** (▶️) or **Debug** (🐞) button in the main toolbar

### 🧪 Step 3: Test and Verify

After a successful launch, the API will be accessible at:

```
🌐 http://localhost:28635
```

Open this URL in your web browser or API testing tool to interact with the API.

---

## 💻 Method 2: Using the Terminal (Command Line)

A universal method that doesn't require a specific IDE.

### 📁 Step 1: Navigate to Project Directory

Open your terminal and navigate to the project root:

```bash
# Example:
cd path/to/your/project/PJ_Source_GV
```

### 🔨 Step 2: Build the Project

Compile the application and restore NuGet packages:

```bash
dotnet build
```

> ✅ Look for "Build succeeded" message upon completion

### ▶️ Step 3: Run the Project

Launch the API server:

```bash
# From project directory
dotnet run

# Or from solution root
dotnet run --project path/to/PJ_Source_GV.csproj
```

### 🧪 Step 4: Test and Verify

The API will be accessible at:

```
🌐 http://localhost:28635
```

> 🛑 To stop the application, press `Ctrl + C` in the terminal

---

## 🐳 Method 3: Using Docker Compose (Database Setup)

The recommended way to set up the Microsoft SQL Server database using Docker.

### 🚀 Step 1: Start the Database Service

From the project root directory (where `docker-compose.yml` is located):

```bash
docker-compose up -d
```

This command will:
- 📥 Download MS SQL Server 2022 image (if not present)
- 🚀 Start the database container in detached mode
- 🔌 Make the database accessible on port **1433**

### 🔐 Database Credentials

| Field | Value |
|-------|--------|
| **Username** | `sa` |
| **Password** | `P@ssw0rd2024` |
| **Port** | `1433` |

> ⚠️ Ensure your API's connection string in `appsettings.json` points to this database instance

### 🛑 Step 2: Stop the Database Service

When finished developing:

```bash
docker-compose down
```

> 💾 Database data will be preserved in the local volume: `~/docker-data/mssql`

---

## 📝 Quick Reference

### 🔧 Build Commands
```bash
# Build with Rider
Ctrl+Shift+B

# Build with CLI
dotnet build
```

### ▶️ Run Commands
```bash
# Run with CLI
dotnet run

# Run specific project
dotnet run --project path/to/PJ_Source_GV.csproj
```

### 🐳 Docker Commands
```bash
# Start database
docker-compose up -d

# Stop database
docker-compose down

# View running containers
docker ps
```

---

## 🌐 Default Endpoints

| Service | URL | Description |
|---------|-----|-------------|
| **API** | `http://localhost:28635` | Main API endpoint |
| **Database** | `localhost:1433` | SQL Server instance |

---

## 🆘 Troubleshooting

- **Build fails**: Ensure .NET 9 SDK is properly installed
- **Port conflicts**: Check if port 28635 is available
- **Database connection**: Verify Docker is running and container is started
- **Permission issues**: Run terminal as administrator (Windows) or with sudo (Linux/Mac)

---

## 📚 Additional Resources

- [.NET 9 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [JetBrains Rider Documentation](https://www.jetbrains.com/help/rider/)
- [Docker Documentation](https://docs.docker.com/)

---

*Last updated: June 2025*