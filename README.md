# Technico Setup & Usage Guide

## 1. Verify .NET 8.0 Installation

- **Check your .NET 8.0 installation** by running the following command:
  ```bash
  dotnet --version
  ```

- **Install .NET 8.0 SDK (if required):**
  Download the .NET 8.0 SDK from the official website:
  [Download .NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

## 2. Ensure Docker is Installed

- **Verify Docker installation** by running:
  ```bash
  docker --version
  ```

- If Docker is not installed, download and install it from:
  [Download Docker](https://www.docker.com/products/docker-desktop)

---

## 3. Set Up MSSQL Server with Docker

- Run the following Docker command to pull the SQL Server image and start the container:
  ```bash
  docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrongPassword123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server
  ```

---

## 4. Configure Database Connection in `TechnicoDbContext`

- In your `TechnicoDbContext` file (located inside the `Context` folder), comment out any existing options builder lines and replace them with the following configuration:

  ```csharp
  optionsBuilder.UseSqlServer(
      "Data Source=localhost,1433; Initial Catalog=Technico_EF; User='SA';Password='YourStrongPassword123'; Encrypt=false;"
  );
  ```

---

## 5. Apply Database Migrations Using Entity Framework (EF)

1. **Open a Command Prompt or Terminal**:
   Navigate to the folder containing the project (where the `.csproj` file is located).

2. **Install EF Tools (if not already installed):**
   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. **Apply Migrations:**
   To create the database and apply any pending migrations, run:
   ```bash
   dotnet ef database update
   ```

---

## 6. Optional: Use a Database Editor

- If you would like to explore or edit the database, you can download DBeaver:
  [Download DBeaver](https://dbeaver.io/download/)

- In DBeaver, add a new database connection (choose SQL Server) with the following details:
  - **Host:** `localhost`
  - **Port:** `1433`
  - **Database/schema:** `Technico_EF`
  - **Username:** `SA`
  - **Password:** `YourStrongPassword123`

---

## 7. Start the Application

1. **Restore Dependencies:**
   Run the following command to restore the required dependencies:
   ```bash
   dotnet restore
   ```

2. **Run the Application:**
   After restoring dependencies, start the app with:
   ```bash
   dotnet run
   ```

3. **Access the Application:**
   Once running, the application will be receiving requests at:
   [http://localhost:5007](http://localhost:5007)

---

## 8. Access the Swagger API Documentation

- Once the application is running, open the Swagger page at:
  [http://localhost:5007/swagger/index.html](http://localhost:5007/swagger/index.html)

---

## 9. Create a New Property Owner

- To create a property owner, use the following API endpoint (POST):
  ```
  /api/PropertyOwners
  ```

- Example JSON body:
  ```json
  {
    "vat": "123456780",
    "name": "yourname",
    "surname": "yoursurname",
    "address": "youraddress",
    "phoneNumber": "699",
    "email": "a@a.com",
    "password": "S!tring123@"
  }
  ```

---

## 10. Grant Admin Access to a Property Owner

- To make a user an administrator, manually update the `PropertyOwner` record in the database by modifying `UserType` from `User` to `Admin` via your database editor.

---

## 11. Log In and Retrieve an Authentication Token

- Use the following endpoint (POST) to log in:
  ```
  /api/PropertyOwners/login
  ```

- Example JSON body:
  ```json
  {
    "email": "a@a.com",
    "password": "S!tring123@"
  }
  ```

- The response will contain a token similar to:
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "userType": "User",
    "vat": "123456789",
    "name": "yourname",
    "surname": "yoursurname",
    "address": "youraddress",
    "phoneNumber": "699",
    "email": "a@a.com"
  }
  ```

---

## 12. Authorize API Requests in Swagger

1. Copy the token (without quotes).
2. In the Swagger UI, click the **Authorize** button.
3. Paste the token into the provided field and click **Authorize**.

You can now make authenticated calls to protected endpoints within Swagger.
