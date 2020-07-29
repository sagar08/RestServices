Create a project in command prompt
dotnet new webapi -o JWTAuthenticationAndAuthorizationWebAPI

Add following Nuget Packages
 - Microsoft.EntityFrameworkCore.SqlServer
 - Microsoft.EntityFrameworkCore.Tools
 - Microsoft.AspNetCore.Identity.EntityFrameworkCore
 - Microsoft.AspNetCore.Identity
 - Microsoft.AspNetCore.Authentication.JwtBearer
 - Microsoft.AspNetCore.Mvc.NewtonsoftJson
 - Microsoft.EntityFrameworkCore.Tools.DotNet
 - AutoMapper.Extensions.Microsoft.DependencyInjection
 - NLog.Extensions.Logging

eg: dotnet add pacakge Microsoft.EntityFrameworkCore.Tools.DotNet

Adding Swagger
dotnet add package Swashbuckle.AspNetCore
dotnet add package Swashbuckle.AspNetCore.Swagger

-------------------------------------------------------------------------------
* To support database migration install tool
dotnet tool install --global dotnet-ef
 
- Adding new table from code to datbase
dotnet ef migrations add AddedUserEntity
dotnet ef database update

- Update existing table 
dotnet ef migrations add ExtendedUserClass


- Revert migrations if database is not updated
dotnet ef migrations remove
-- This will remove the last added/updated tables schema  

- Revert migrations after database update dotnet
dotnet ef migrations database update <LastAddedMigrationName>
Note: This does not support for SQLite

