# Medical Equipment Inventory Application

This is a desktop application for managing the inventory of medical furniture and equipment in a healthcare environment. It is built with WPF and C# and provides a convenient interface for tracking products, suppliers, materials, and storage locations.

## Features

- User authentication system
- Inventory management with support for categories, shelves, and storage cells
- Product labeling with printable Word document templates
- Supply tracking and history logging
- Document generation for inventory and acceptance acts
- Modular design with separate windows for editing and managing data

## Technologies Used

- C# (.NET Framework)
- WPF (Windows Presentation Foundation)
- Microsoft Word Interop (for document generation)
- SQLite (local database)

## Project Structure

- `Models/` – Data models (Product, Supplier, Category, etc.)
- `Modals/` – Windows for adding and editing entities
- `Pages/` – Main application pages
- `Docs/` – Word document templates used for printing labels and reports
- `MedicalFurnitureAccounting.db` – Local SQLite database
- `WordHelper.cs` – Utilities for generating Word documents

## Getting Started

1. Open the solution file `MedicalFurnitureAccounting.sln` in Visual Studio.
2. Build the project.
3. Run the application.

## License

This project is for educational and internal use.
