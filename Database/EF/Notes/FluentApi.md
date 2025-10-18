#  What is Fluent API?

The **Fluent API** in EF Core is a **programmatic way to configure your models** using **method chaining** inside `OnModelCreating(ModelBuilder modelBuilder)` in your `DbContext`.

It’s called *“fluent”* because you can chain methods together, like this:

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.Name)
    .IsRequired()
    .HasMaxLength(100);
```

 Fluent API is **more powerful** than Data Annotations:

* It supports advanced configurations that attributes can’t express.
* Keeps your entity classes **clean** (no extra attributes).
* Great for large enterprise applications with dozens of entities.

---

#  1. Basic Fluent API Setup

Every `DbContext` has a method:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Fluent configurations go here
}
```

Inside it, you access each entity via:

```csharp
modelBuilder.Entity<TEntity>()
```

---

#  2. Entity Configuration Basics

### ➤ 2.1 Table & Schema Mapping

By default, EF Core creates a table name equal to the class name.
You can rename it or move it into a schema.

```csharp
modelBuilder.Entity<Student>()
    .ToTable("StudentsTable", schema: "School");
```

Result:

```sql
CREATE TABLE [School].[StudentsTable] (...)
```

---

### ➤ 2.2 Primary Key Configuration

Usually automatic, but you can define manually:

```csharp
modelBuilder.Entity<Student>()
    .HasKey(s => s.Id);
```

For **composite keys**:

```csharp
modelBuilder.Entity<Enrollment>()
    .HasKey(e => new { e.StudentId, e.CourseId });
```

---

### ➤ 2.3 Column Mapping

Rename a property → column name or datatype:

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.Name)
    .HasColumnName("StudentName")
    .HasColumnType("varchar(100)");
```

---

### ➤ 2.4 Nullability and Required Fields

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.Name)
    .IsRequired();  // equivalent to [Required]
```

---

### ➤ 2.5 Default Values

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.EnrollmentDate)
    .HasDefaultValueSql("GETDATE()");
```

---

### ➤ 2.6 Max Length / Precision

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.Name)
    .HasMaxLength(100);

modelBuilder.Entity<Student>()
    .Property(s => s.GPA)
    .HasPrecision(3, 2); // decimal(3,2)
```

---

#  3. Relationship Configurations (Fluent API Style)

Let’s go deeper into **navigations, keys, and cascade behaviors**.

---

### ➤ 3.1 One-to-Many

Example: `Teacher` → many `Students`

```csharp
modelBuilder.Entity<Student>()
    .HasOne(s => s.Teacher)
    .WithMany(t => t.Students)
    .HasForeignKey(s => s.TeacherId)
    .OnDelete(DeleteBehavior.Restrict);
```

 Explanation:

* `HasOne()` → “each student has one teacher”
* `WithMany()` → “each teacher has many students”
* `HasForeignKey()` → defines FK column
* `OnDelete(DeleteBehavior.Restrict)` → disables cascade delete

---

### ➤ 3.2 One-to-One

```csharp
modelBuilder.Entity<Student>()
    .HasOne(s => s.Address)
    .WithOne(a => a.Student)
    .HasForeignKey<StudentAddress>(a => a.StudentId);
```

---

### ➤ 3.3 Many-to-Many (Implicit Join Table)

```csharp
modelBuilder.Entity<Student>()
    .HasMany(s => s.Courses)
    .WithMany(c => c.Students)
    .UsingEntity(j => j.ToTable("StudentCourses"));
```

 EF Core automatically creates a join table named `StudentCourses`.

---

### ➤ 3.4 Many-to-Many (Explicit Join Entity)

If you need extra fields (like EnrollmentDate):

```csharp
modelBuilder.Entity<Enrollment>()
    .HasKey(e => new { e.StudentId, e.CourseId });

modelBuilder.Entity<Enrollment>()
    .HasOne(e => e.Student)
    .WithMany(s => s.Enrollments)
    .HasForeignKey(e => e.StudentId);

modelBuilder.Entity<Enrollment>()
    .HasOne(e => e.Course)
    .WithMany(c => c.Enrollments)
    .HasForeignKey(e => e.CourseId);
```

---

#  4. Advanced Property Configurations

These are often used in real-world apps to fine-tune your data model.

---

### ➤ 4.1 Computed Columns

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.FullName)
    .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
```

---

### ➤ 4.2 Value Conversions

Convert between .NET and database representations.

Example: Store an enum as a string:

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.Gender)
    .HasConversion<string>();
```

---

### ➤ 4.3 Ignore Properties

Exclude a property from the database:

```csharp
modelBuilder.Entity<Student>()
    .Ignore(s => s.TempValue);
```

---

### ➤ 4.4 Shadow Properties

Properties that **exist in the database but not in your class**.

```csharp
modelBuilder.Entity<Student>()
    .Property<DateTime>("CreatedDate")
    .HasDefaultValueSql("GETDATE()");
```

Accessed like:

```csharp
var created = context.Entry(student).Property("CreatedDate").CurrentValue;
```

---

### ➤ 4.5 Unique Indexes

```csharp
modelBuilder.Entity<Student>()
    .HasIndex(s => s.Email)
    .IsUnique();
```

---

### ➤ 4.6 Cascade Delete Behaviors

Control what happens when a parent entity is deleted:

```csharp
modelBuilder.Entity<Teacher>()
    .HasMany(t => t.Students)
    .WithOne(s => s.Teacher)
    .OnDelete(DeleteBehavior.Cascade);    // default
```

Options:

| Behavior   | Description                        |
| ---------- | ---------------------------------- |
| `Cascade`  | Child rows deleted automatically   |
| `Restrict` | Prevent deletion if children exist |
| `SetNull`  | FK becomes null                    |
| `NoAction` | No effect in DB (manual control)   |

---

#  5. Model-Level Configurations

You can apply conventions globally.

### ➤ 5.1 Change Table Naming Convention

Example: pluralize all table names manually:

```csharp
foreach (var entity in modelBuilder.Model.GetEntityTypes())
{
    entity.SetTableName(entity.DisplayName() + "s");
}
```

---

### ➤ 5.2 Default Schema for All Tables

```csharp
modelBuilder.HasDefaultSchema("School");
```

---

### ➤ 5.3 Global Filter (Soft Delete / Multitenancy)

Example: automatically filter soft-deleted rows:

```csharp
modelBuilder.Entity<Student>()
    .HasQueryFilter(s => !s.IsDeleted);
```

Now `context.Students.ToList()` ignores deleted ones.

---

#  6. Fluent API vs Data Annotations — Quick Recap

| Feature                | Data Annotation     | Fluent API           |
| ---------------------- | ------------------- | -------------------- |
| Table name             | ✅ `[Table("Name")]` | ✅ `.ToTable("Name")` |
| Schema                 | ❌                   | ✅                    |
| Composite keys         | ❌                   | ✅                    |
| Index                  | ✅ (EF Core 5+)      | ✅                    |
| Default value SQL      | ❌                   | ✅                    |
| Cascade delete control | ❌                   | ✅                    |
| Shadow properties      | ❌                   | ✅                    |
| Global query filter    | ❌                   | ✅                    |

---

#  7. Organizing Fluent Configurations (Best Practice)

For large projects, don’t put all config in one big `OnModelCreating`.
Use **EntityTypeConfiguration classes**.

Example:

```csharp
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);
    }
}
```

Then register in your context:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfiguration(new StudentConfiguration());
}
```

Or apply all automatically:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
```

---

#  Summary Table

| Concept                | Example                              | Purpose             |
| ---------------------- | ------------------------------------ | ------------------- |
| `ToTable()`            | `.ToTable("Students")`               | Rename table        |
| `HasKey()`             | `.HasKey(e => e.Id)`                 | Define PK           |
| `HasColumnName()`      | `.HasColumnName("StudentName")`      | Rename column       |
| `IsRequired()`         | `.IsRequired()`                      | Non-null column     |
| `HasDefaultValueSql()` | `.HasDefaultValueSql("GETDATE()")`   | Default value       |
| `HasPrecision()`       | `.HasPrecision(3, 2)`                | Decimal precision   |
| `HasOne().WithMany()`  | Relationship mapping                 |                     |
| `OnDelete()`           | `.OnDelete(DeleteBehavior.Restrict)` | Cascade rule        |
| `HasQueryFilter()`     | `.HasQueryFilter(e => !e.IsDeleted)` | Soft delete filter  |
| `HasIndex()`           | `.HasIndex(e => e.Email).IsUnique()` | Index configuration |

