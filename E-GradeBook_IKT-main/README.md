# Управување со ИКТ проекти

### Configuration
1. **Database Connection**
   - To run the project , you need to create an `appsettings.json`file in the `Web` directory. of the project with your own configurations.
     ```json
       {
       "ConnectionStrings": {
         "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;"
       },
     
       "Logging": {
         "LogLevel": {
           "Default": "Information",
           "Microsoft.AspNetCore": "Warning"
         }
       },
       "AllowedHosts": "*"
     }
     ```
  - Replace `YOUR_SERVER` with your own SQL Server instance.
  - Replace `YOUR_DATABASE` with the name of your database.
  
### Setting Up the Database
2. Open your terminal and navigate to the `VehicleRepository` :
3. To set up the initial database schema, run the following commands:
```bash
add-migration init
update-database
```
