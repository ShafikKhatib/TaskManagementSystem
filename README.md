# Task Management System

A comprehensive REST API for managing tasks, employees, and teams built with **ASP.NET Core 8** and **Entity Framework Core**.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [API Endpoints](#api-endpoints)
- [Architecture](#architecture)
- [Models](#models)
- [Contributing](#contributing)
- [License](#license)

---

## 🎯 Overview

The Task Management System is a robust backend API that enables organizations to manage tasks efficiently across teams and employees. It provides functionality for creating, updating, and tracking tasks with notes, generating reports, and managing team hierarchies.

**Repository:** [GitHub - TaskManagementSystem](https://github.com/ShafikKhatib/TaskManagementSystem)

---

## ✨ Features

- ✅ **Task Management** - Create, read, update, and delete tasks
- ✅ **Task Notes** - Add detailed notes and progress tracking to tasks
- ✅ **Employee Management** - Manage employees with roles and team assignments
- ✅ **Team Management** - Organize employees into teams
- ✅ **Task Completion Tracking** - Mark tasks as completed or incomplete
- ✅ **Admin Reports** - Generate reports for due tasks and completed tasks
- ✅ **Manager Dashboard** - View team tasks and progress
- ✅ **Role-Based Management** - Support for Admin, Manager, TeamLead, and Employee roles
- ✅ **REST API** - Full REST API with Swagger/OpenAPI documentation
- ✅ **Error Handling** - Comprehensive error handling and validation
- ✅ **Database Relationships** - Proper foreign key relationships with cascading deletes

---

## 🛠️ Tech Stack

| Component | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 8.0 | Application framework |
| **ASP.NET Core** | 8.0 | Web framework |
| **Entity Framework Core** | 8.0.8 | ORM & Database access |
| **SQL Server** | Latest | Database |
| **Swagger/Swashbuckle** | 6.4.0 | API documentation |
| **JWT Bearer** | 8.0.8 | Authentication |
| **C#** | Latest | Programming language |

---

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Local or Remote)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (Optional but recommended)
- Git

---

## 🚀 Installation

### 1. Clone the Repository

````````
git clone https://github.com/ShafikKhatib/TaskManagementSystem.git
cd TaskManagementSystem
````````

### 2. Restore NuGet Packages

````````
dotnet restore
````````

### 3. Verify Project Structure

Ensure your project structure matches the expected layout:

````
TaskManagementSystem/
│
├──  src/
│    ├──  TaskManagementAPI/         # Web API project
│    ├──  TaskManagement.Core/      # Core library (Entities, Enums, Exceptions)
│    ├──  TaskManagement.Infrastructure/  # Infrastructure (Data, Repositories, Migrations)
│    ├──  TaskManagement.Tests/     # Unit and Integration tests
│
├──  docs/                          # Documentation (API specs, ER diagrams)
│
└──  README.md                     # This file
````

### 4. Update Configuration

- Set up your `appsettings.json` for database connection and other settings.
- Configure JWT settings for authentication.

### 5. Apply Migrations and Update Database

````````
dotnet ef database update
````````

### 6. Run the Application

````````
dotnet run
````````

The API should now be running at `https://localhost:5001` (for example). Use a tool like Postman or cURL to interact with the API.

---

## ⚙️ Configuration

### Update Connection String

Edit `appsettings.json` to configure your SQL Server connection:

````````json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(local);Initial Catalog=TaskManagementDB;Integrated Security=True;TrustServerCertificate=True"
  },
  ...
}
````````

**Connection String Parameters:**
- `Data Source=(local)` - Local SQL Server instance
- `Initial Catalog=TaskManagementDB` - Database name
- `Integrated Security=True` - Windows Authentication
- `TrustServerCertificate=True` - Trust SSL certificate

---

## 💾 Database Setup

### 1. Create Database

Open SQL Server Management Studio and run:

````````
CREATE DATABASE TaskManagementDB;
GO

ALTER DATABASE TaskManagementDB SET RECOVERY SIMPLE;
GO
````````

### 2. Run SQL Script

Execute the provided `CreateDatabaseScript.sql` file to:
- Create all tables (Team, Employee, TaskItem, TaskNotes)
- Create indexes for performance
- Insert sample data (5 teams, 15 employees, 17 tasks)

### 3. Verify Database

Use SSMS to check that the database and objects are created:

````
TaskManagementDB
│
├──  Tables
│    ├──  Team
│    ├──  Employee
│    ├──  TaskItem
│    ├──  TaskNotes
│
├──  Views
│
└──  Stored Procedures
````

---

## 📡 API Endpoints

API documentation is available at `/swagger` after running the application.

Sample endpoints:

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - User login
- `GET /api/tasks` - Get all tasks
- `POST /api/tasks` - Create a new task

Refer to Swagger documentation for full endpoints list and details.

---

## 🏗️ Architecture

### Layered Architecture

````````
TaskManagementSystem/
│
├──  src/
│    ├──  TaskManagementAPI/         # Web API project
│    ├──  TaskManagement.Core/      # Core library (Entities, Enums, Exceptions)
│    ├──  TaskManagement.Infrastructure/  # Infrastructure (Data, Repositories, Migrations)
│    ├──  TaskManagement.Tests/     # Unit and Integration tests
│
├──  docs/                          # Documentation (API specs, ER diagrams)
│
└──  README.md                     # This file
````````

### Domain-Driven Design (DDD)

- **Entities** - Task, Employee, Team
- **Value Objects** - Money (for cost estimation), Address (for location)
- **Aggregates** - Task with nested TaskNotes
- **Repositories** - Interfaces for data access
- **Services** - Business logic for tasks, employees
- **Controllers** - API entry points, model binding, and response formatting

---

## 📚 Models

### TaskItem

Represents a task with properties for:

- Title
- Description
- Due date
- Priority
- Status (Pending, InProgress, Completed)
- Navigation properties for related Employee and Team

### Employee

Represents an employee with properties for:

- Name
- Email
- Role (Admin, Manager, TeamLead, Employee)
- Navigation properties for related Tasks and Team

### Team

Represents a team with properties for:

- Name
- Description
- Navigation properties for related Employees

### TaskNotes

Represents notes for a task with properties for:

- Note content
- Creation date
- Last modified date
- Navigation properties for related TaskItem

Refer to the codebase for detailed model classes and mappings.

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a new branch (`feature/your-feature`)
3. Commit your changes
4. Push to your fork
5. Create a Pull Request

Please ensure your code adheres to the existing style and includes appropriate tests.

---

 
---

## 🚀 Running the Application

### Using Visual Studio 2022

1. Open `TaskManagementSystem.sln`
2. Set `TaskManagementSystem` as startup project
3. Press `F5` or click **Start Debugging**
4. Application will launch at `https://localhost:7001`

### Using Command Line

````````
cd src/TaskManagementAPI
dotnet run
````````

---



