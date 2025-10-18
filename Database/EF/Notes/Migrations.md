# 📘 Table of Contents

1. What are Migrations and Why We Need Them
2. How EF Core Tracks Database Schema
3. Step-by-Step: Creating and Applying Migrations
4. Inside a Migration File — Deep Breakdown
5. Update, Revert, and Rebuild Migrations
6. Seeding Data with Migrations
7. Managing Migrations in Teams / CI/CD
8. Common Migration Problems (and Solutions)
9. Best Practices for Real-World Projects

---

##  What Are EF Core Migrations?

Migrations are **version control for your database** 🗄️

When you change your entity classes or Fluent configurations, EF Core can:

* **Detect** what changed in your model
* **Generate** a migration file describing what SQL to apply
* **Update** your database schema automatically

Think of it like Git, but for your database structure.

---

###  The Migration Lifecycle

```mermaid
graph LR
A[Change Entity Class or Model] --> B[Add Migration]
B --> C[EF Generates Migration Script]
C --> D[Apply Migration -> Database Updated]
```

So, EF Core **keeps your C# models and database schema in sync** — without manual SQL.

---

##  How EF Core Tracks the Schema

EF Core stores model metadata in a **Migrations History Table**:

```
__EFMigrationsHistory
```

This table lives in your database and records:

* Migration name
* Product version
* Timestamp

It ensures EF knows which migrations have already been applied.

---

## Step-by-Step: Creating and Applying Migrations

Let’s do it hands-on with a real example.

### Prerequisites

Ensure you have:

* Installed the EF Core CLI tools

  ```
  dotnet tool install --global dotnet-ef
  ```
* Your `DbContext` is configured correctly.

---

###  Step 1: Add a Migration

When you make changes to entities or Fluent configuration:

```bash
dotnet ef migrations add InitialCreate
```

📁 This creates a new folder:

```
Migrations/
 ├── 20251018122000_InitialCreate.cs
 ├── 20251018122000_InitialCreate.Designer.cs
 └── MyDbContextModelSnapshot.cs
```

---

###  Step 2: Apply Migration to Database

Run:

```bash
dotnet ef database update
```

 EF Core will:

* Read your migration files
* Generate the SQL
* Apply it to your database
* Record it in `__EFMigrationsHistory`

---

###  Step 3: Make a Change

Now, say you add a new property:

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; } // new property
}
```

Add another migration:

```bash
dotnet ef migrations add AddEmailToStudent
dotnet ef database update
```

EF Core will generate the needed SQL automatically:

```sql
ALTER TABLE Students ADD Email nvarchar(max) NULL;
```

---

##  Inside a Migration File — In-Depth Breakdown

Here’s what a typical migration file looks like:

```csharp
public partial class AddEmailToStudent : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "Students",
            type: "nvarchar(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Email",
            table: "Students");
    }
}
```

 What’s happening:

* `Up()` = applies schema changes (upgrade)
* `Down()` = reverts those changes (rollback)
* EF Core tracks all changes here

---

###  Designer File

The `.Designer.cs` file holds EF Core’s **internal model snapshot**.
It helps EF detect differences next time you run `add migration`.

---

##  Managing Migrations: Update, Revert, Rebuild

### ➤ Apply Latest Migration

```bash
dotnet ef database update
```

### ➤ Apply a Specific Migration

```bash
dotnet ef database update AddEmailToStudent
```

### ➤ Roll Back to Previous State

```bash
dotnet ef database update InitialCreate
```

 EF calls the `Down()` method of later migrations.

### ➤ Remove the Last Unapplied Migration

```bash
dotnet ef migrations remove
```

---

##  Seeding Data with Migrations (In-Code Seeding)

EF Core allows **automatic data seeding** using Fluent API.

Example:

```csharp
modelBuilder.Entity<Teacher>().HasData(
    new Teacher { Id = 1, Name = "John Smith" },
    new Teacher { Id = 2, Name = "Sara Khan" }
);
```

Then run:

```bash
dotnet ef migrations add SeedTeachers
dotnet ef database update
```

 EF Core generates an `INSERT` statement in the migration.

 Important:

* Seeded data is **inserted only once** (during migration).
* If you change seed data, EF will generate **UPDATE/DELETE** scripts accordingly.

---

##  Migrations in Teams / CI/CD Environments

###  Scenario: Multiple Developers

* Each dev adds migrations locally.
* When merging branches, EF Core must reconcile schema differences.

#### Tips:

* Always run `dotnet ef migrations add` *after* pulling new code.
* Never manually edit the snapshot unless you’re resolving a merge conflict carefully.
* Keep migration names descriptive (e.g., `AddOrderDateToInvoice`).
* Check migration files into source control (they are code, not runtime data).

---

### CI/CD Integration (DevOps Pipelines)

In your deployment pipeline:

```bash
dotnet ef database update --context AppDbContext
```

You can automate schema updates when new versions of your app are deployed.

Or, to generate SQL without applying:

```bash
dotnet ef migrations script
```

This creates a SQL file you can review or run manually on a production database.

---

##  Common Migration Problems (and Fixes)

| Problem                                         | Cause                                           | Solution                                       |
| ----------------------------------------------- | ----------------------------------------------- | ---------------------------------------------- |
| ❌ “Unable to create object of type ‘DbContext’” | EF can’t find context                           | Check your `OnConfiguring` or DI setup         |
| ❌ “Pending model changes detected”              | You changed entities but didn’t migrate         | Run `dotnet ef migrations add <Name>`          |
| ❌ Duplicate column error                        | Model renamed property without dropping old one | Use `.HasColumnName()` to map or drop manually |
| ❌ Data loss warning                             | Property type changed                           | Use migration `Up()` to manually preserve data |
| ❌ History table missing                         | Database manually altered                       | Recreate with `dotnet ef database update`      |

---

## 9️⃣ Best Practices for Migrations

✅ **Name migrations clearly**

> `AddDateOfBirthToStudent`, `RenameTeacherToInstructor`

✅ **Keep migrations small**

> Easier to debug, revert, and merge

✅ **Never edit old migrations**

> Once applied in production, treat them as immutable

✅ **Use `HasData()` for static data**, not runtime data

> Avoid seeding user data with migrations

✅ **Review generated SQL**

> Use `dotnet ef migrations script` before running in prod

✅ **Reset during development only**
If the schema gets messy during dev:

```bash
dotnet ef database drop
dotnet ef database update
```

⚠️ Never do this on production.

---

##  Real-World Example — Migration Evolution

Imagine this timeline:

| Step | Change                | Command                                          |
| ---- | --------------------- | ------------------------------------------------ |
| 1    | Create Student entity | `dotnet ef migrations add InitialCreate`         |
| 2    | Add Email property    | `dotnet ef migrations add AddEmailToStudent`     |
| 3    | Add Course entity     | `dotnet ef migrations add AddCourseEntity`       |
| 4    | Add relationship      | `dotnet ef migrations add StudentCourseRelation` |
| 5    | Apply all             | `dotnet ef database update`                      |

Each migration file represents a “schema checkpoint.”

---

##  Summary Cheat Sheet

| Command                            | Purpose                      |
| ---------------------------------- | ---------------------------- |
| `dotnet ef migrations add <Name>`  | Create new migration         |
| `dotnet ef database update`        | Apply migrations             |
| `dotnet ef database update <Name>` | Apply to specific migration  |
| `dotnet ef migrations remove`      | Undo last migration          |
| `dotnet ef migrations script`      | Generate SQL script          |
| `dotnet ef database drop`          | Drop database                |
| `dotnet ef dbcontext info`         | Show info about context      |
| `dotnet ef dbcontext scaffold`     | Reverse engineer existing DB |

