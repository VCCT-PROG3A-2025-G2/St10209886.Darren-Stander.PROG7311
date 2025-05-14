# TestApp - Farmer and Employee Management System

TestApp is an ASP.NET MVC application designed to manage farmers and their products while providing role-based access control for employees. The system features secure login, product management, and user-specific data handling.

## Features

### User Roles
 **Farmer:**
   - Can add new products.
   - Can edit and delete their own products.
   - Cannot view product details.

 **Employee:**
   - Can view details of all products.
   - Can filter products based on category, date range, and name.
   - Cannot add, edit, or delete any products.

---

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022
- SQLite Database
- Git for version control

## Functionalities Implemented
### Authentication & Authorization
- User login with session management.
- role-based access via ASP.NET filters.

### Product Management (Farmers)
- Add, edit, and delete products.
- View only their own products.

### Product Viewing (Employees)
- View details of all products.
- Filter by category, date range, and name.

### Data Validation
- Validates product names and categories.
- Validates product creation dates.

### Error Handling
- Catches database errors and shows user-friendly messages.
- Redirects unauthorized access to an Access Denied page.

## How it works
- Run the application by pressing f5
- Open your browser at http://localhost:7130/.
- You’ll land on the Login/Register screen.
- Choose Farmer or Employee when registering.
- Farmer users can add/view their own products.
- Employee users can add Farmers, view all products, and filter/search.

