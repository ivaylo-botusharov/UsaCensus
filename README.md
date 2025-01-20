# Task (for Esri Bulgaria)

*Description:* This is my implementation of a task given by Esri Bulgaria before interview. The task description is located [here](/docs/Task-Esri.md).

## Prerequisites

* .NET version 9.0.102
(currently latest stable release)

* MongoDB Community Server 8.0.4 (current)

* Check (and if needed modify) Connection string to MongoDB database

Before running the applications open [appsettings-shared.json](/src/UsaCensus/UsaCensus.Infrastructure/Configuration/appsettings-shared.json) and in section `UsaCensusDatabase` make sure the value of `ConnectionString` is the one you are using. If needed modify it.

## Instructions

These are the instructions and order of running the applications.

1. Start UsaCensus.BackgroundTasks

Navigate in the terminal to folder [UsaCensus.BackgroundTasks](/src/UsaCensus/UsaCensus.BackgroundTasks/) and run the command:

```bash
dotnet build
```

The project should be built successfully, then run:

```bash
dotnet run
```

Initially, when the application starts DatabaseIntializer will create database `usa_census` (if it does not exist) and will create a collection `demographics`.

Afterwards, on every 15 seconds it makes an HTTP request to ArcGIS Services, gets the US Counties and their population, aggregates the population by county, if there are any documents in `demographics` collection deletes them and then inserts the state demographics data (stateName, population).

2. Start UsaCensus.API

After you have completed step 1, navigate in the terminal to folder [UsaCensus.API](/src/UsaCensus/UsaCensus.API/) and run the command:

```bash
dotnet build
```

The project should be built successfully, then run:

```bash
dotnet run
```

After the application starts, open Postman (or similar app) and make the following GET HTTP requests:  
* Make sure the port on which the application runs is the same (and if needed change the URL)

```
http://localhost:5148/api/demographics/
```

You should see a JSON response of around 35 objects (population, stateName).

```
http://localhost:5148/api/demographics/arizona
```

You should see a JSON response with a single object for Arizona, something like:

```JSON
{
    "population": 7427991,
    "stateName": "Arizona"
}
```
