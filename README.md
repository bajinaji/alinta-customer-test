# Alinta Customer Test Web API Project

This project comprises a simple REST Web API that utilises a generalised Customer repository (implemented as an EF Core in memory database) to provide a simple interface for creating, modifying and deleting customer entities.

## Description

The project is implemented in .Net 5, follows a few simple patterns, and display a reasonable trade off between good practices and simple demonstration of techniques and concepts.

### Dependencies
* .Net 5 developer environment
* Git (for downloading)

### Installing

* Open a command line prompt
* Execute-> "git clone https://github.com/bajinaji/alinta-customer-test"
* Navigate to the project  folder created by git
* Execute-> "dotnet restore"
* Execute-> "dotnet build"

### Executing program

* Execute-> "cd .\CustomerWebAPI\"
* Execute-> "dotnet run"

To run the test suite:
* Open a command line prompt
* Navigate to the project folder
* Execute-> "dotnet test"

## Help

I would advise using postman to test running get, put, post and delete commands to test the web api

## Authors

Rob Clayton bajinaji@gmail.com