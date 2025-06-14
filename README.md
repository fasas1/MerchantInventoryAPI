# MerchantInventoryAPI 

**Full-stack Inventory Management API** built with **.NET 8 Web API**, featuring products, stock, transactions, reporting, and user authentication. Ready to serve your React frontend.

---

## 🧭 Project Overview

This project is the **backend API** for a merchant-focused inventory management system, enabling:

- ✅ Product CRUD (create, read, update, delete)  
- ✅ Stock tracking with thresholds  
- ✅ Sales/restock transaction logging  
- ✅ Reporting (inventory, sales, stock valuation)  
- ✅ User authentication & role-based access  


---

## 🛠 Tech Stack

- **.NET 8 Web API**  
- **Entity Framework Core** (Code First)  
- **SQL Server** (default; can switch to PostgreSQL)  
- **JWT Authentication**  
- **Swagger / OpenAPI** for API exploration

---

## 🚀 Features

- **Products**: Manage product metadata (name, price, image, category).  
- **Stock**: Monitor stock levels, thresholds, and update timestamps.  
- **Transactions**: Log sales and restocks, automatically update stock.  
- **Reporting**:  
  - Inventory overview (status, low-stock alerts)  
  - Sales reports (daily/weekly/monthly)  
  - Stock valuation by current price  
- **User Management**: Support for Admin and Staff roles with JWT-based login.

---

## 🔧 Getting Started

1. **Clone the repo**  
    ```bash
    git clone https://github.com/fasas1/MerchantInventoryAPI.git
    cd MerchantInventoryAPI
    ```

2. **Set up environment variables**  
    Copy `.env.example` to `.env` and add your:
    - `DB_CONNECTION` (e.g. SQL Server connection string)
    - `JWT_SECRET` (for token signing)
    - Optional settings: `JWT_ISSUER`, `JWT_AUDIENCE`, `JWT_EXPIRY_MINUTES`

3. **Install dependencies & apply migrations**  
    ```bash
    dotnet restore
    dotnet ef database update
    ```

---

## ▶️ Running the Project

Start the API with Swagger support:

```bash
dotnet run
