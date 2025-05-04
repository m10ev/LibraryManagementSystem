==================================================
README - ASP.NET Application with SQL Server Express
Test Environment Setup Guide
==================================================

Contents:
---------
1. Prerequisites
2. Database Configuration
3. Seeding the Database
4. Running the App with IIS Express
5. Testing the Application
6. Troubleshooting
7. Additional Notes

--------------------------------------------------
1. PREREQUISITES
--------------------------------------------------

Before you begin, ensure the following software is installed:

- Visual Studio 2022 or newer
- .NET SDK (.NET 9)
- SQL Server Express (Local instance)

--------------------------------------------------
2. DATABASE CONFIGURATION
--------------------------------------------------

Ensure your SQL Server Express instance is running.

Default server name:
    .\SQLEXPRESS

--------------------------------------------------
3. SEEDING THE DATABASE
--------------------------------------------------

You must seed the database BEFORE running the application using Entity Framework Core Migrations
-------------------------------------------------
1. Open Visual Studio.
2. Open the Package Manager Console:
   Tools > NuGet Package Manager > Package Manager Console
3. Run the following command:
   > Update-Database Stable
4. Start WebApp with IIS EXPRESS

--------------------------------------------------
4. RUNNING THE APP WITH IIS EXPRESS
--------------------------------------------------

Once the database is seeded:

1. Open the solution in Visual Studio.
2. Right-click the main project > "Set as Startup Project".
3. In the toolbar, ensure "IIS Express" is selected.
4. Press F5 or click the green Play button.
5. The app should launch at:
   http://localhost:PORT/

Note: Replace PORT with the actual port shown in Visual Studio.

--------------------------------------------------
5. TESTING THE APPLICATION
--------------------------------------------------

After launching:

- Visit the homepage or known routes to check seeded data.
--------------------------------------------------
6. TROUBLESHOOTING
--------------------------------------------------

Issue: Cannot connect to database
- Check if SQL Server Express is running
- Ensure the connection string is correct
- Try "localhost\SQLEXPRESS" if ".\SQLEXPRESS" doesn’t work

Issue: IIS Express fails to start
- Check if the port is in use
- Edit launchSettings.json to change the port

Issue: No seeded data appears
- Verify `Update-Database` was successful
- Check the seeding logic in the code or script

--------------------------------------------------
7. ADDITIONAL NOTES
--------------------------------------------------

- Seed data should only be used in development or testing environments.
- Ensure proper user permissions for local SQL Express access.
- You can add logging in Program.cs or Startup.cs for environment-specific behavior.

--------------------------------------------------

Support:
--------
If you have issues or questions, please contact the project maintainer or create an issue in the repository.

Thank you for using our app!
