# Api9-Service-Pattern

## Project Overview

This project is an enterprise-grade Web API built using **.NET 9**, implementing the **Sevice** pattern along with Clean Architecture principles. It provides a scalable, maintainable, and testable structure for complex business applications.

## Technology Stack

- **Language:** C#  
- **Framework:** ASP.NET Core 9  
- **Architecture:** Clean Architecture  
- **Design Pattern:** Service  
- **ORM:** Entity Framework Core    
- **Object Mapping:** AutoMapper  
- **API Documentation:** Swagger / OpenAPI  
- **Database:** SQL Server  

## Project Structure

/Api8-CQRS-Pattern
│
├── /Src
│ ├── /Api # Web API controllers
│ ├── /Application # Business Logic
│ ├── /Domain # Entities, Interfaces, Domain Logic
│ └── /Infrastructure # Data access, Services, Dependencies
│
└── /Test
└── /UnitTests # Unit tests

bash
Copy
Edit

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/RzSjDev/api-.net9.git
Open the solution in Visual Studio 2022 or later.

Configure your SQL Server connection string in appsettings.json.

Apply database migrations to create the necessary tables:

bash
Copy
Edit
dotnet ef database update
Build and run the project.

API Documentation
After running the project, you can access the Swagger UI for API documentation and testing at:

bash
Copy
Edit
https://localhost:{port}/swagger/index.html
Testing
Unit tests are available under /Test/UnitTests and can be run using test runners compatible with xUnit or NUnit.

Contributing
Contributions are welcome! Please open issues or submit pull requests for improvements
