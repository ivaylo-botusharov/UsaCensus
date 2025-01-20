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

1. **Start UsaCensus.BackgroundTasks**

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

2. **Start UsaCensus.API**

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

## Technical Choice Analysis and Rationale

### API technology

I have selected to use `ASP.NET Core Web API` as a backend technology since it is a very mature framework created and supported by Microsoft. In addition, it performs very well in terms of requests per second (load testing) according TechEmpower benchmarks. One more point is that I have advanced knowledge of ASP.NET Core and .NET

### Database

I have selected `MongoDB` as a database, since the structure of data looks like documents in a single collection. There are no multiple entities with relations between them. Another point is that `MongoDB` could be horizontally scaled through sharding if there a quite high loads. MongoDB's document-based structure allows for a flexible schema, which means you can easily add or remove fields as the data evolves without having to alter the database schema. MongoDB integrates seamlessly with various programming languages and frameworks, facilitating the development and maintenance of the REST API.

### .NET Background Tasks (BackgroundService)

For the task purpose and since there are no detailed requirements, I have selected a minimalistic .NET solution - creating a background worker class (inherits from BackgroundService), which runs in the background a while loop. If the background task should be scheduled to run at specific time periodically or there are some additional requirements for production-ready systems libraries like: Hangfire, Quartz.NET could be used.

* References:

Background tasks with hosted services in ASP.NET Core  
<https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-9.0&tabs=visual-studio>

Implement background tasks in microservices with IHostedService and the BackgroundService class  
<https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice>

---

### HTTP

#### HttpClient

Used HttpClient for making HTTP requests within the backend since it is one of the mostly used and supported by Microsoft solutions. In addition, Microsoft put some extra effort to optimize the serialization / deserialization performance (System.Text.Json) which the HttpClient class highly utilizes.

* **HTTP requests resilience**

For the UsaCensus.BackgroundTasks the method `builder.AddServiceDefaults();` configures HttpClient defaults, the so called Standard resilience pipeline (Rate limiter, Total timeout, Retry, Circuit breaker, Attempt timeout).

* References:

Build resilient HTTP apps: Key development patterns - Add resilience to an HTTP client  
<https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience?tabs=dotnet-cli#add-resilience-to-an-http-client>

ResilienceHttpClientBuilderExtensions.AddStandardResilienceHandler Method  
<https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.resiliencehttpclientbuilderextensions.addstandardresiliencehandler?view=net-9.0-pp>

Microsoft.Extensions.Http.Resilience  
<https://www.nuget.org/packages/Microsoft.Extensions.Http.Resilience>

Building resilient cloud services with .NET 8  
<https://devblogs.microsoft.com/dotnet/building-resilient-cloud-services-with-dotnet-8/>

`eShop/src/eShop.ServiceDefaults/Extensions.cs` - `builder.Services.ConfigureHttpClientDefaults()` 
<https://github.com/dotnet/eShop/blob/7a23f90d13418eca59cb0a292a093589bad415f9/src/eShop.ServiceDefaults/Extensions.cs#L22>

HttpClientFactoryServiceCollectionExtensions.ConfigureHttpClientDefaults Method  
<https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.httpclientfactoryservicecollectionextensions.configurehttpclientdefaults?view=net-9.0-pp>

.NET Aspire service defaults - Add service defaults functionality  
<https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/service-defaults#add-service-defaults-functionality>

* **IHttpClientFactory**

In order to avoid port exhaustion problems, decided to reuse HttpClient instances for as many HTTP requests as possible through the IHttpClientFactory interface (injected in HttpClientWrapper)

* References:

IHttpClientFactory with .NET  
<https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory>

Use IHttpClientFactory to implement resilient HTTP requests  
<https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests>

* **HttpClientWrapper class**

The need for this class emerged since for GET HTTP requests the .NET HttpClient does not have functionality for URL encoding query parameters and their respective values. Another points is that I have added exception handling and logging functionality.

---

### Error handling

#### Result class

Decided to implement more functional style of error handling - catch the exceptions of I/O operations and pass Result objects with all the details to invoking methods. For example, if there is an exception during an IO operation, we could have some additional business logic and execute it depending on the Result values. Another point is that re-throwing exceptions is more expensive and catching them in another section of the code makes it more difficult to follow through and debug.

* References:

Functional C#: Handling failures, input errors  
<https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/>

Error handling: Exception or Result?  
<https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/>

`CSharpFunctionalExtensions/CSharpFunctionalExtensions/Result/ResultT.cs`  
<https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Result/ResultT.cs>

---

### Logging

Implemented logging within the application (DemographicsRepository, HttpClientWrapper) using the benefits of compile-time logging source generation. For now the Logging provider is the console, but it could be easily changed to use another provider.

* References:

Logging in C# and .NET  
<https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line>

---

## TODO:

Since there is no time left for this task I consider several points for further improvements:

1. **Configure Telemetry for the Project**

Here the OpenTelemetry protocol and Microsoft classes implementing it could be used for the ASP.NET Core API project (UsaCensus.API), so that it would be easier for the application to be connected to Observability / Monitoring solution in Production.

* References:

eShop/src/eShop.ServiceDefaults/Extensions.cs - ConfigureOpenTelemetry()
<https://github.com/dotnet/eShop/blob/7a23f90d13418eca59cb0a292a093589bad415f9/src/eShop.ServiceDefaults/Extensions.cs#L50>

---

## Additional Points:

* High loads and scalability

If there are very high loads for the application Kubernetes could be used to start and stop additional Docker container instances running the UsaCensus.API.

---
