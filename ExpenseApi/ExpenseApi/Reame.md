
Install-Package Microsoft.EntityFrameworkCore.Sqlite

Install-Package Microsoft.EntityFrameworkCore.Tools

To create the database you need to run the following commands : 

Update-Database

A postman collection has been created to help you test the api

The database chosen is SQLite because it's very simple to set up. It's automatically seeded when applying Update-Database command. 
I chose to the ORM Entity Framework to manage the database. 

Guid are randomely generated, so I created quickly a user Endpoint to fetch User information.

Then there are 2 endpoints to get expenses and create one. You need the user id to create expenses. 

I chose different design pattern :
- Specification pattern : to handle specific user criteria in requests
- Repository pattern with unit of work to wrap entity framework calls (except in the user Controller)
- Command pattern (Mediatr lib)

I also chose different libraries : 
- Auto Mapper to create custom mapper and avoid repetitive logics
- XUnit for testing