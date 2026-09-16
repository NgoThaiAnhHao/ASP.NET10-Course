-- Nuget Install
Npgsql.EntityFrameworkCore.PostgreSQL
Microsoft.EntityFrameworkCore.Tools

-- Migration
Add-Migration "Name Of Migration"
Update-Database

-- Setting Up Authentication
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.IdentityModel.Tokens
System.IdentityModel.Tokens.Jwt
Microsoft.AspNetCore.Identity.EntityFrameworkCore

-- Serilog (Logging)
Serilog
Serilog.AspNetCore
Serilog.Sinks.Console
Serilog.Sinks.File (Add logging to Text File)

- Implement Versioning
Microsoft.AspNetCore.Mvc.Versioning