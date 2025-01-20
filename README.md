## Instructions

1. Install Docker

2. Install mongosh

```bash
docker pull mongodb/mongodb-community-server:latest
```

```bash
docker run --name mongodb -p 27017:27017 -d mongodb/mongodb-community-server:latest
```

```bash
docker container ls
```

```bash
mongosh --port 27017
```

```bash
db.runCommand(
   {
      hello: 1
   }
)
```

---

```bash
mongosh --port 27017
```

```bash
use usa_census
```

```bash
db.demographics.insertMany([{ population: 123, stateName: "Arizona" }, { population: 345, stateName: "California" }])
```

```bash
db.demographics.insertOne({
   population: 489,
   stateName: "New Jersey"
});
```

```bash
try {
   db.demographics.insertOne( { population: 221, stateName: "Colorado" } );
} catch (e) {
   print (e);
};
```

---

### Create Background service project using a template

```bash
dotnet new worker -o UsaCensus.BackgroundTasks
```

---

## TODO:

3. Exception Handling for DemographicsRepository

4. Add error code to Result and set error code in HttpClientWrapper catch statements

5. Return maybe BadRequests with response body (properties - errorMessage, errorCode)

6. Add logging in some important methods - Setup Logger in Program.cs (maybe Console logger) - make sure Structured Logging is used within the app

7. Configure Telemetry for UsaCensus.API Project

8. Write documentation

8.1. Instructions how to run the Projects - prerequisites, etc.

8.2. Explain made decisions, the usage of databases, classes (e.g. HttpClientWrapper, `Result<T>`) and libraries, explain about horizontal scalability (Docker instances and Kubernetes), MongoDB sharding, possible integration of the UsaCensus API and Prometheus with Grafana or cloud solutions like Azure Monitor (Azure Application Insights).

8.3. Further improvements - Use advanced Background jobs library (e.g. Hangfire)

9. Fix warnings

10. Configure .NET Analyzer (Roslyn Analyzer) for both projects and fix warnings

11. Check OpenAPI generated documentation

12. Create mapping from Demographics model to DemographicsViewModel (no id field)

13. Demographics Repository - BulkInsertAsync() use transaction

14. Add database schema - constraints: stateName - unique, stateName - string, stateName - max chars 100; population - zero or positive integer

14. Remove not used methods from DemographicsRepository

---


For now removed not used methods in DemographicsRepository (to be added later on):

```csharp
public async Task<Demographics?> GetAsync(string id) =>
        await this.demographicsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task CreateAsync(Demographics newDemographics) =>
        await this.demographicsCollection.InsertOneAsync(newDemographics);
    
    public async Task UpdateAsync(string id, Demographics updatedDemographics) =>
        await this.demographicsCollection.ReplaceOneAsync(x => x.Id == id, updatedDemographics);
    
    public async Task RemoveAsync(string id) =>
        await this.demographicsCollection.DeleteOneAsync(x => x.Id == id);
```
