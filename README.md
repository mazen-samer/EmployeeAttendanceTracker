# Employee Attendance Tracker

This project is an Employee Attendance Tracker web application built as a technical task for CodeZone LLC. It is developed using ASP.NET Core MVC and follows a strict N-tier architecture to ensure a clean separation of concerns. The application uses Entity Framework Core with an in-memory database for data persistence.

This project was developed by **Mazen**.

---

## Project Explanation & Architecture

The application is structured into three distinct layers, ensuring that all business logic is properly isolated from the presentation and data access layers. This approach makes the application scalable, maintainable, and easy to test.

1.  **Presentation Layer (`EmployeeAttendanceTracker.Presentation`)**

    - An ASP.NET Core MVC project responsible for the User Interface (UI).
    - Contains all Controllers, Views, and ViewModels.
    - Controllers are kept "thin" and do not contain business logic; they only orchestrate calls to the Business Layer.

2.  **Business Layer (`EmployeeAttendanceTracker.Business`)**

    - A .NET Class Library that acts as the core of the application.
    - Contains all business logic, complex validation, and rules within service classes (e.g., `DepartmentService`, `EmployeeService`).
    - This layer is the single source of truth for all business operations and is completely independent of the UI and database technology.

3.  **Data Layer (`EmployeeAttendanceTracker.Data`)**
    - A .NET Class Library responsible for all database handling.
    - Contains the EF Core `DbContext`, data models (Entities), and repositories.
    - The Repository Pattern is used to abstract data access logic, making it easy to swap out the data source in the future without affecting the Business Layer.

Dependency Injection is used throughout the application to decouple these layers.

---

## Setup and Installation

This project is configured to run with a self-seeding, in-memory database, so no external database setup is required.

### Option 1: Using Visual Studio

1.  **Clone the Repository:**
    ```bash
    git clone <your-github-repository-url>
    ```
2.  **Open the Solution:**
    - Navigate to the project folder and open the `EmployeeAttendanceTracker.sln` file in Visual Studio 2022 or later.
3.  **Run the Application:**
    - Set the `EmployeeAttendanceTracker.Presentation` project as the startup project.
    - Press `F5` or the "Run" button in Visual Studio.

### Option 2: Using the Command Line (CLI)

1.  **Clone the Repository:**
    ```bash
    git clone <your-github-repository-url>
    ```
2.  **Navigate to the Solution Directory:**
    - `cd` into the root folder of the solution (the folder containing `EmployeeAttendanceTracker.sln`).
3.  **Run the Application:**
    - Execute the following command:
    ```bash
    dotnet run --project EmployeeAttendanceTracker.Presentation --launch-profile https
    ```

The application will build, launch, and be accessible at `https://localhost:7142` (or a similar address shown in the terminal). The in-memory database will be created and seeded automatically on startup.

---

## Features Implemented

### 1. Department Management

- Full CRUD (Create, Read, Update, Delete) functionality for departments.
- The department list page displays the number of employees in each department.
- Enforces validation rules, including unique names and codes, directly in the service layer.

### 2. Employee Management

- Full CRUD functionality for employees.
- The employee list page displays a summary of each employee's attendance for the current month (Present days, Absent days, and attendance percentage).
- Enforces complex validation rules for employee names and ensures unique email addresses.

### 3. Attendance Management

- **Dynamic Attendance Recording:** A dedicated page allows for marking an employee's attendance as "Present" or "Absent" for a specific date. The status updates instantly on the page without a full page reload, using jQuery and AJAX. Future dates are disabled.
- **Filterable Attendance Report:** A separate report page displays a list of all attendance records. This report can be filtered by department, employee, and a specific date range.

### 4. Bonus Features

- **Partial Views:** Used in the Employee forms to avoid code duplication (`_EmployeeFormFields.cshtml`).
