# E-commerce Website

The E-commerce Website is a web application built using Blazor WebAssembly (WASM) and utilizes a RESTful API to interact with data. The application's database is built on Microsoft SQL Server. The website provides user management functionality with Identity using JWT tokens and includes the ability to make purchases.

## Features

### 1. Identity with JWT Token

- Register a new account and authenticate user information.
- Log in using a username and password.
- Manage user information such as changing passwords and updating profiles.

### 2. Purchase Functionality

- View a list of available products for purchase.
- Search for products by name, category, or other attributes.
- View detailed information about a product.
- Add products to the shopping cart.
- Manage the shopping cart and update quantities of items.
- Perform payment and place orders.

## Directory Structure

- `/Client`: Blazor WASM source code, containing pages and user interface components.
- `/Server`: RESTful API source code, containing controllers and data processing logic.
- `/Database`: SQL script files for creating the Microsoft SQL Server database.

## Installation

1. Clone this repository to your local machine.
2. Ensure you have .NET 6 SDK or later installed.
3. Navigate to the `/Database` directory and execute the SQL script to create the database.
4. Open the solution in Visual Studio or a similar IDE.
5. Configure the database connection in the `appsettings.json` file of the Server project.
6. Run both the Client and Server projects.

## Technologies Used

- Blazor WASM: Provides the ability to develop single-page web applications (SPA) using .NET in the browser.
- RESTful API: Uses HTTP methods to query and manipulate data with the server.
- Microsoft SQL Server: Relational database for storing user and product information.

## Contributing

If you would like to contribute to this project, follow these steps:

1. Fork the project.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -am 'Add some feature'`).
4. Push your branch to the repository (`git push origin feature/your-feature`).
5. Open a new pull request and provide a detailed description of your changes.

## Author

- Author: [PhamDuong39](https://github.com/PhamDuong39)
- Contact: duongpham3923@gmail.com

Thank you for your interest and contributions to this project!
