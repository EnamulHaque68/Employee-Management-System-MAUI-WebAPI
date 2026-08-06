# 👨‍💼 Employee Management System

<p align="center">
  <img src="./assets/banner.png" alt="Employee Management System Banner" width="100%">
</p>

<p align="center">

![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=for-the-badge\&logo=.net)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core_8-512BD4?style=for-the-badge\&logo=.net)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge\&logo=c-sharp)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge\&logo=microsoftsqlserver)
![REST API](https://img.shields.io/badge/REST_API-009688?style=for-the-badge)

</p>

---

# 📖 Overview

**Employee Management System** is a modern full-stack application built with **.NET MAUI** and **ASP.NET Core 8 Web API**.

The solution follows a client-server architecture where the .NET MAUI application consumes REST APIs exposed by the ASP.NET Core backend to manage employee-related information through a clean and responsive interface.

---

# 🏗 Solution Architecture

```text
EmployeeSln
│
├── EmployeeApi
│     ASP.NET Core 8 Web API
│
└── NextEvdMaui
      .NET MAUI Client
```

---

# ⚙️ System Architecture

```text
.NET MAUI Client
        │
 REST API (HTTP/HTTPS)
        │
        ▼
ASP.NET Core 8 Web API
        │
Business Logic
        │
        ▼
SQL Server Database
```

---

# ✨ Key Features

## 👨‍💼 Employee Management

* Add Employee
* Update Employee
* Delete Employee
* View Employee Details
* Employee List

---

## 🏢 Department Management

* Department Information
* Employee Department Assignment
* Department Selection

---

## 💼 Designation Management

* Designation Information
* Employee Designation Assignment

---

## 📱 Cross Platform Client

* .NET MAUI Application
* Mobile Friendly UI
* Desktop Support
* Responsive Layout

---

## 🌐 REST API

* ASP.NET Core 8 Web API
* JSON Communication
* CRUD Operations
* RESTful Architecture

---

## 🔄 MVVM Pattern

* View
* ViewModel
* Model
* Service Layer
* Clean Separation of Responsibilities

---

# 🔄 Application Workflow

```text
Open Application
        │
        ▼
Load Employees
        │
        ▼
Select Employee
        │
 ┌──────┼─────────┐
 │      │         │
 ▼      ▼         ▼
View   Edit     Delete
 │
 ▼
Save Changes
 │
 ▼
Refresh Employee List
```

---

# 📡 Client–Server Communication

```text
.NET MAUI
      │
      ▼
ApiService
      │
HTTP Request
      │
      ▼
ASP.NET Core Web API
      │
Business Logic
      │
      ▼
SQL Server
      │
JSON Response
      │
      ▼
MAUI UI Update
```

---

# 📂 Project Structure

```text
EmployeeSln
│
├── EmployeeApi
│   ├── Controllers
│   ├── Models
│   ├── Data
│   ├── Migrations
│   ├── Program.cs
│   └── appsettings.json
│
└── NextEvdMaui
    ├── Models
    ├── Services
    ├── ViewModels
    ├── Pages
    ├── Converters
    ├── Resources
    ├── AppShell.xaml
    └── MauiProgram.cs
```

---

# 💻 Technology Stack

| Layer    | Technology             |
| -------- | ---------------------- |
| Client   | .NET MAUI              |
| Backend  | ASP.NET Core 8 Web API |
| Language | C#                     |
| Database | SQL Server             |
| API      | REST API               |
| Pattern  | MVVM                   |

---

# 📱 MAUI Pages

* Dashboard
* Employee Details
* Manage Employee

---

# 🧩 Backend Components

* Controllers
* Models
* Database Context
* REST APIs
* Entity Framework Core
* SQL Server

---

# 🚀 Getting Started

## Clone Repository

```bash
git clone https://github.com/yourusername/employee-management-system.git
```

## Run Backend

```bash
cd EmployeeApi
dotnet restore
dotnet ef database update
dotnet run
```

## Run MAUI Client

```bash
cd NextEvdMaui
dotnet build
dotnet run
```

---

# 📸 Screenshots

| Dashboard      | Employee List  |
| -------------- | -------------- |
| Add Screenshot | Add Screenshot |

| Employee Details | Manage Employee |
| ---------------- | --------------- |
| Add Screenshot   | Add Screenshot  |

---

# 🌟 Project Highlights

* Full Stack Solution
* .NET MAUI
* ASP.NET Core 8 Web API
* MVVM Architecture
* REST API Integration
* SQL Server
* Cross Platform
* Clean Architecture
* Responsive User Interface
* Modular Design

---

# 🔮 Future Improvements

* User Authentication
* Role-Based Authorization
* Dashboard Analytics
* Employee Search
* Employee Filtering
* Image Upload
* Cloud Deployment
* Push Notifications
* Offline Data Synchronization
* Unit Testing

---

# 👨‍💻 Developer

**Md. Enamul Haque**

---

# 📜 License

This project was developed for educational purposes.

---

<p align="center">

### **Modern • Cross-Platform • Scalable • Employee Management Solution**

Built with using **.NET MAUI** & **ASP.NET Core 8 Web API**

</p>
