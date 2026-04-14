# 🚀 DEPI .NET Backend Development Track

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET Core Web API](https://img.shields.io/badge/ASP.NET%20Core-0058A9?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Testing](https://img.shields.io/badge/Unit%20Testing-20232A?style=for-the-badge&logo=testing&logoColor=white)

Welcome to my comprehensive **.NET Backend Development** repository! 

This repository documents my entire journey and skill progression through the DEPI .NET Development track. From foundational C# programming and advanced Object-Oriented paradigms to building production-ready **ASP.NET Core Web APIs** and complex database architectures with **Entity Framework Core**.

---

## 🏗️ Highlighted Systems (Web APIs)
The culmination of this track resulted in three fully functioning, scalable Web API solutions implemented with Clean Architecture principles:

*   🛒 **E-Commerce System Web API**: A complete digital storefront API featuring product cataloging, customer management, secure ordering, and cart functionalities.
*   🏥 **Health Care System Web API**: A robust healthcare management API to coordinate patients, doctors, appointments, and medical records handling.
*   📚 **Library System Web API**: An extensive library management API for tracking book inventories, patron borrowing histories, and librarian administration.

---

## 🛠️ Core Skills & Technologies Demonstrated

### 1. Advanced C# & Object-Oriented Programming (OOP)
*   **Encapsulation, Inheritance, & Polymorphism**: Deep understanding of class hierarchies and interfaces.
*   **Structs, Enums, & Records**: Optimizing memory and data immutability.
*   **Generics & Collections**: `FixedSizeList`, generic constraints, and advanced collection manipulations.
*   **Delegates, Events, & Lambdas**: Implementing robust event-driven architectures.
*   **Operator Overloading & Extension Methods**: Writing cleaner, more expressive, and reusable code (`Maths`, `ThreeDPoint`, `Range`).

### 2. Design Patterns & Architecture
*   **Singleton Pattern**: Implementing thread-safe singletons (`Singleton.cs`).
*   **Repository & Unit of Work Patterns**: Decoupling data access in Web APIs.
*   **N-Tier & Clean Architecture**: Separating concerns across Core, Persistence, and Presentation layers.

### 3. Data Queries & Manipulation (LINQ)
*   Extensive mastery of **Language Integrated Query (LINQ)** to filter, aggregate, group, and project data flows (from Lists to XML parsing like `Customers.xml`).
*   Deferred execution, query operators, and complex joins (`ListGenerator.cs`).

### 4. Database & ORM (Entity Framework Core)
*   **Code-First Approach**: Designing domain models mapping seamlessly to SQL databases.
*   **Fluent API & Data Annotations**: Configuring complex relationships (One-to-Many, Many-to-Many).
*   **Migrations**: Managing version-controlled database schemas.

### 5. ASP.NET Core Web API Development
*   **RESTful Routing & Controllers**: Designing standard HTTP GET/POST/PUT/DELETE interactions.
*   **Dependency Injection (DI)**: Registering application services and enforcing inversion of control.
*   **Data Transfer Objects (DTOs)**: Enforcing clear API contracts and preventing over-posting.
*   **Middleware & Validation**: Request pipelines and systematic input validation.

### 6. Software Testing
*   **Unit Testing**: Isolated business logic testing (`BookTests`, `LibraryEngineTests`, etc.) to guarantee reliability and protect against regressions.

---

## 📈 Learning Journey Breakdown

| Module | Focus & Implementations |
|--------|--------------------------|
| **Sessions 01-05** | **C# Fundamentals & OOP Basics**: Classes, objects, memory management, primary types, inheritance, and struct behaviors (`Dog`, `Duck`, `Point`, `Cat`). |
| **Sessions 06-08** | **Deep OOP & Encapsulation**: Interface implementation (`ICloneable`), Static classes, and advanced data modeling (`Employee`, `HireDate`, `Duration`). |
| **Sessions 09-10** | **Generics & Extensions**: Strong-typed generics, custom collections, and extending existing types cleanly. |
| **Session 11** | **Delegates, Patterns, & Testing**: Function pointers, implementing the Singleton design pattern, and ensuring reliability through automated Unit Testing. |
| **Sessions 12-13** | **LINQ Mastery**: Advanced data generation, declarative data querying, projections, and grouping across large datasets. |
| **Session 14** | **Entity Framework Core**: Designing the data tier for the E-Commerce, Health Care, and Library systems. Mapping models to databases. |
| **Sessions 15-17** | **ASP.NET Core Web APIs**: Constructing the APIs layer. Building out endpoints, enforcing DTOs, dependency injection, and structuring professional backend architectures. |

---

## 💻 Getting Started

1. Clone the repository: 
   ```bash
   git clone https://github.com/your-username/DEPI_.NET_TASKS.git
   ```
2. Open the solution space in **Visual Studio** or **VS Code**.
3. For Web API Projects (Sessions 14-17), assure your local SQL Server is running.
4. Update the `appsettings.json` connection strings appropriately.
5. Open the Package Manager Console or terminal and run:
   ```bash
   dotnet ef database update
   ```
6. Run the project to explore the Swagger UI documentation for the APIs.

---
*Created as part of the DEPI .NET Developer Track. Proving readiness for high-performance software engineering.*
