# StudentsFirst

StudentsFirst is a demo web application made for a fictional school and to be used by fictional teachers and students.

The purpose of this application is to showcase my various full-stack development skills. I'm available for remote full-time or contract hire as a full stack software engineer, and if you're interested you can contact me on my [LinkedIn profile](https://www.linkedin.com/in/saraelsa/).

> This project is currently under construction, but you can still explore it. Some back-end functionality (e.g. relating to users and groups) has already been implemented, and a front-end is planned soon.

## Running

Make sure you have the .NET 5 SDK installed on your system.

Check the `appsettings.json` (and its configuration variants) file in the `StudentsFirst.Api.Monolithic/` and adjust the configuration parameters to suit your environment. Note that although I've checked in my database settings, this database is only accessible locally on my system and does not store any sensitive data.

You can run the API server for development purposes by running `dotnet run` in the `StudentsFirst.Api.Monolithic/` directory.

## Testing

You can test the API server by running `dotnet test` in the `StudentsFirst.Api.Monolithic/` directory.

## Acknowledgements

This project's structure was partly inspired by the [Realworld ASP.NET Core example app](https://github.com/gothinkster/aspnetcore-realworld-example-app).
