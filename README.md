# EcommerceBE
This project was generate with .NET Core

##Database
[PostgresSQL](https://www.postgresql.org/)
This project use code-first to define model

add-migrations: `cd Ecommerce.Domain && dotnet ef --startup-project ../Ecommerce.API/ migrations add migration-name`

update-database: `cd Ecommerce.Domain && dotnet ef --startup-project ../Ecommerce.API/ database update`

##Development
`cd Ecommerce.API && dotnet run`
