#  EF Core Methods — Complete Deep Dive

EF Core exposes most of its functionality through these two core classes:

* **`DbContext`** → Manages database connections, tracking, and transactions
* **`DbSet<TEntity>`** → Represents a database table and provides methods for CRUD and querying

We’ll cover methods for:

1. CRUD operations (`Add`, `Find`, `Update`, `Remove`, etc.)
2. Querying methods (`Where`, `Select`, `OrderBy`, etc.)
3. Change tracking and saving (`SaveChanges`, `Attach`, `Entry`)
4. Transactions and raw SQL
5. Advanced methods (AsNoTracking, Include, FromSqlRaw, etc.)

---

##  1. `DbSet` CRUD Methods

###  `Add()` / `AddAsync()`

Adds a new entity to the context so it will be **inserted** into the database when `SaveChanges()` is called.

**Example:**

```csharp
var student = new Student { Name = "Ali" };
context.Students.Add(student);
context.SaveChanges();
```

> Adds the student entity and executes an SQL `INSERT` statement.

**Async version:**

```csharp
await context.Students.AddAsync(new Student { Name = "Sara" });
await context.SaveChangesAsync();
```

---

###  `Find()` / `FindAsync()`

Finds an entity by its **primary key**.

**Example:**

```csharp
var student = context.Students.Find(1);
```

> If found in memory (tracked), it returns that instance. Otherwise, EF queries the database.

**Async:**

```csharp
var student = await context.Students.FindAsync(1);
```

---

###  `Update()`

Marks an entity as modified, meaning EF Core will generate an **UPDATE** SQL on `SaveChanges()`.

**Example:**

```csharp
var student = context.Students.Find(1);
student.Name = "Updated Name";
context.Students.Update(student);
context.SaveChanges();
```

> Normally, you don’t need to call `Update()` — EF tracks changes automatically.
> Use it when you’re updating a **detached entity** (not tracked by the context).

---

###  `Remove()` / `RemoveRange()`

Marks an entity (or list of entities) for **deletion**.

**Example:**

```csharp
var student = context.Students.Find(2);
context.Students.Remove(student);
context.SaveChanges();
```

Delete multiple:

```csharp
var inactiveStudents = context.Students.Where(s => s.IsActive == false);
context.Students.RemoveRange(inactiveStudents);
context.SaveChanges();
```

---

###  `Attach()`

Attaches an existing entity to the context **without marking it as modified**.
Used when you already have data but don’t want to insert or update it automatically.

**Example:**

```csharp
var student = new Student { Id = 3, Name = "Ali" };
context.Students.Attach(student); // tracked but not changed
```

---

##  2. Querying Methods (LINQ + EF Core)

These are LINQ methods you use with `DbSet` — EF Core translates them to SQL.

---

###  `Where()`

Filters the data.

```csharp
var students = context.Students.Where(s => s.Name.Contains("Ali")).ToList();
```

Equivalent SQL:

```sql
SELECT * FROM Students WHERE Name LIKE '%Ali%'
```

---

###  `Select()`

Projects specific columns or transforms the result.

```csharp
var names = context.Students.Select(s => s.Name).ToList();
```

> Returns only the `Name` property from each student.

---

###  `OrderBy()` / `OrderByDescending()`

Sorts the result set.

```csharp
var students = context.Students.OrderBy(s => s.Name).ToList();
```

---

###  `First()` / `FirstOrDefault()`

Gets the first record (or `null` if none found).

```csharp
var student = context.Students.FirstOrDefault(s => s.Name == "Ali");
```

---

###  `Single()` / `SingleOrDefault()`

Returns **exactly one** record. Throws an error if more than one is found.

```csharp
var student = context.Students.SingleOrDefault(s => s.Id == 1);
```

---

###  `Any()`

Checks if **any record** matches the condition.

```csharp
bool exists = context.Students.Any(s => s.Name == "Ali");
```

---

###  `Count()` / `LongCount()`

Returns the number of matching entities.

```csharp
int total = context.Students.Count();
```

---

###  `Skip()` / `Take()`

Used for pagination.

```csharp
var page2 = context.Students.Skip(10).Take(10).ToList();
```

> Skips the first 10 records, then takes the next 10.

---

###  `Include()`

Loads related data (navigational properties).

```csharp
var students = context.Students
                      .Include(s => s.Courses)
                      .ToList();
```

> Performs an SQL `JOIN` under the hood.

---

##  3. Change Tracking and Saving

###  `SaveChanges()` / `SaveChangesAsync()`

Commits all pending insert, update, and delete operations.

```csharp
context.SaveChanges();
```

Async:

```csharp
await context.SaveChangesAsync();
```

> EF Core scans all tracked entities and generates appropriate SQL commands.

---

###  `Entry()`

Gives access to metadata and state information for a specific entity.

```csharp
var entry = context.Entry(student);
Console.WriteLine(entry.State); // Added, Modified, Deleted, etc.
```

You can also manually set state:

```csharp
context.Entry(student).State = EntityState.Modified;
```

---

###  `ChangeTracker`

A property of `DbContext` that lets you inspect all tracked entities.

```csharp
var trackedEntities = context.ChangeTracker.Entries();
```

---

##  4. Transactions and Raw SQL

###  Transactions

EF Core manages transactions automatically in `SaveChanges()`,
but you can manually control them if needed.

```csharp
using var transaction = context.Database.BeginTransaction();

try
{
    context.Students.Add(new Student { Name = "Ali" });
    context.SaveChanges();

    transaction.Commit();
}
catch
{
    transaction.Rollback();
}
```

---

###  `FromSqlRaw()`

Executes a **raw SQL query** that returns entities.

```csharp
var students = context.Students
    .FromSqlRaw("SELECT * FROM Students WHERE Name LIKE '%Ali%'")
    .ToList();
```

###  `ExecuteSqlRaw()`

Executes raw SQL for non-query commands like `UPDATE` or `DELETE`.

```csharp
context.Database.ExecuteSqlRaw("DELETE FROM Students WHERE IsActive = 0");
```

---

##  5. Advanced and Utility Methods

###  `AsNoTracking()`

Disables change tracking for performance when you only need to read data.

```csharp
var students = context.Students.AsNoTracking().ToList();
```

> Great for reports or read-only operations — reduces memory usage.

---

###  `Include()` with `ThenInclude()`

Loads nested related data.

```csharp
var students = context.Students
    .Include(s => s.Courses)
        .ThenInclude(c => c.Teacher)
    .ToList();
```

---

###  `ToQueryString()`

Shows the SQL query EF Core will execute — useful for debugging.

```csharp
var query = context.Students.Where(s => s.Name.Contains("Ali"));
Console.WriteLine(query.ToQueryString());
```

---

###  `EnsureCreated()` / `EnsureDeleted()`

Quickly create or delete a database without migrations — useful in testing.

```csharp
context.Database.EnsureCreated();
context.Database.EnsureDeleted();
```

---

###  `FindTracked()`

Not an official EF method, but important conceptually: EF Core checks its **local cache** first before querying the database.

```csharp
var student = context.Students.Local.FirstOrDefault(s => s.Id == 1);
```

---

##  Summary Table

| Category     | Method                                            | Purpose                    |
| ------------ | ------------------------------------------------- | -------------------------- |
| **Create**   | `Add`, `AddAsync`, `AddRange`                     | Insert new data            |
| **Read**     | `Find`, `Where`, `Select`, `Include`, `ToList`    | Query data                 |
| **Update**   | `Update`, `Entry`, `Attach`                       | Modify existing data       |
| **Delete**   | `Remove`, `RemoveRange`                           | Delete data                |
| **Save**     | `SaveChanges`, `SaveChangesAsync`                 | Commit changes             |
| **Tracking** | `Entry`, `ChangeTracker`, `AsNoTracking`          | Monitor entity states      |
| **Raw SQL**  | `FromSqlRaw`, `ExecuteSqlRaw`                     | Run direct SQL             |
| **Utility**  | `EnsureCreated`, `EnsureDeleted`, `ToQueryString` | Database setup & debugging |


Your Code → DbContext → Change Tracker → LINQ Query → Query Translation → Database Provider → SQL Execution → Results / Save
