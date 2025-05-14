# TestApp - Agri-Energy Connect

TestApp is an ASP.NET MVC application designed to manage farmers and their products while providing role-based access control for employees. The system features secure login, product management, and user-specific data handling.

### Github Repo:
https://github.com/Darren-Stander/testApp

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
- You’ll land on the Login/Register screen.
- Choose Farmer or Employee when registering.
- Farmer users can add/view their own products.
- Employee users can add Farmers, view all products, and filter/search.

## AI Declartion and References
- https://www.w3schools.com/cs/index.php
- https://www.youtube.com/watch?v=QtiM87MV27w&pp=ygUfYyMgbXZjIHdlYiBhcHBsaWNhdGlvbiB0dXRvcmlhbA%3D%3D
- https://www.youtube.com/watch?v=b1X7T8wNsJE&pp=ygU1aG93IHRvIGdldCBzcWxpdGUgdG8gd29yayB3aXRoIGMjIG12YyB3ZWIgYXBwbGljYXRpb24%3D
- https://www.w3schools.com/css/css3_buttons.asp
- https://www.w3schools.com/css/css_templates.asp
- https://www.w3schools.com/html/html_styles.asp
- https://stackoverflow.com/questions/3309685/understanding-the-mvc-pattern

  #### ChatGPT not paid so model 4 I believe 
- https://chatgpt.com/share/68249f98-4090-800e-95dd-287935d22099
- https://chatgpt.com/share/68249fce-daf4-800e-b027-00eb956a1fb6
- https://chatgpt.com/share/6824a0f9-95f0-800e-87b0-1576074c7227

