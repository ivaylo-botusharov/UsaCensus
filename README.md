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
use usa-census
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
