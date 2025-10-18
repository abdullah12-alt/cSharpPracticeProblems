#  1. Entity Classes — The Heart of EF Core

An **Entity** is a .NET class that represents a **table** in your database.
Each **property** in the class becomes a **column**, and each **instance** represents a **row**.

Example:

```csharp
public class Student
{
    public int Id { get; set; }             // Primary Key (PK)
    public string Name { get; set; }        // Regular column
    public DateTime EnrollmentDate { get; set; }
}
```

When EF Core runs migrations, it creates this table automatically:

```sql
CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(MAX),
    EnrollmentDate DATETIME
);
```

---

##  1.1 Primary Keys (PK)

By convention, EF Core uses a property named `Id` or `<ClassName>Id` as the primary key.

```csharp
public class Teacher
{
    public int TeacherId { get; set; } // PK by convention
    public string FullName { get; set; }
}
```

If you want to define it manually:

```csharp
[Key]
public int MyCustomKey { get; set; }
```

---

##  1.2 Entity Conventions

EF Core uses **conventions** (rules) to automatically infer database structure:

* Property `Id` or `<TypeName>Id` → primary key
* `string` → nvarchar(max)
* `DateTime` → datetime2
* Navigation property → relationship (if foreign key exists)

You can override these defaults using:

* **Data Annotations** (attributes)
* **Fluent API** (inside `OnModelCreating`)

We’ll see both soon.

---

#  2. DbContext Setup

The `DbContext` class is your **bridge between C# and the database**.
It defines:

* Which entities (tables) exist (`DbSet`)
* How they’re connected (relationships)
* Database provider and connection string

---

### Example: Creating a `SchoolContext`

```csharp
public class SchoolContext : DbContext
{
    // DbSet represents a table in the database
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    // Configure the connection and provider
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=.;Database=SchoolDB;Trusted_Connection=True;");
    }

    // Fluent API configurations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Example: configure composite key
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });
    }
}
```

### How it works:

* EF Core reads `DbSet<T>` properties to know what tables exist.
* When you run migrations, it creates matching tables.
* `OnModelCreating()` lets you customize entity mapping.

---

#  3. Entity Relationships (Associations)

EF Core supports **three main types of relationships:**

| Relationship Type  | Description                             | Example            |
| ------------------ | --------------------------------------- | ------------------ |
| 1️⃣ One-to-One     | One entity is related to only one other | Student → Address  |
| 1️⃣➡️∞ One-to-Many | One entity has many related ones        | Teacher → Students |
| ∞↔️∞ Many-to-Many  | Both sides can have many                | Student ↔ Courses  |

Let’s see each one in depth 

---

## 3.1  One-to-One Relationship

Example: A `Student` has **one** `Address`, and that address belongs to **one** student.

### Entity Classes:

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation property
    public StudentAddress Address { get; set; }
}

public class StudentAddress
{
    [Key, ForeignKey("Student")] // use same key as Student
    public int StudentId { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public Student Student { get; set; } // back-reference
}
```

### Fluent API Alternative:

```csharp
modelBuilder.Entity<Student>()
    .HasOne(s => s.Address)
    .WithOne(a => a.Student)
    .HasForeignKey<StudentAddress>(a => a.StudentId);
```

 EF Core creates a `StudentAddresses` table with a foreign key to `Students`.

---

## 3.2  One-to-Many Relationship

Example: One `Teacher` can teach many `Students`.

### Entities:

```csharp
public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation: One-to-Many
    public ICollection<Student> Students { get; set; }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Foreign Key
    public int TeacherId { get; set; }

    // Navigation back
    public Teacher Teacher { get; set; }
}
```

### Fluent API:

```csharp
modelBuilder.Entity<Teacher>()
    .HasMany(t => t.Students)
    .WithOne(s => s.Teacher)
    .HasForeignKey(s => s.TeacherId);
```

➡️ Creates a foreign key column `TeacherId` in `Students`.

---

## 3.3  Many-to-Many Relationship

Example: A `Student` can enroll in many `Courses`, and each `Course` can have many `Students`.

### Entities (EF Core 5+ supports implicit join tables):

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
}
```

### What EF Core does:

* Automatically creates a **join table** called `CourseStudent` with:

  ```
  StudentId INT
  CourseId INT
  ```

### If you want a custom join entity (explicit mapping):

```csharp
public class Enrollment
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public DateTime EnrolledOn { get; set; }
}
```

Fluent API for explicit join:

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

#  4. Fluent API vs Data Annotations

###  Data Annotations — Easy and Declarative

Applied directly on classes/properties using attributes.

Example:

```csharp
public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Column("EnrollDate", TypeName = "date")]
    public DateTime EnrollmentDate { get; set; }
}
```

---

###  Fluent API — More Powerful and Centralized

Configured inside `OnModelCreating()` using the `ModelBuilder` object.

Example:

```csharp
modelBuilder.Entity<Student>()
    .Property(s => s.Name)
    .IsRequired()
    .HasMaxLength(100);

modelBuilder.Entity<Student>()
    .ToTable("StudentsTable")
    .HasIndex(s => s.Name)
    .IsUnique();
```

---

#  5. Complete Example — Realistic Setup

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

public class Enrollment
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public DateTime EnrolledOn { get; set; }
}
```

```csharp
public class SchoolContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=SchoolDB;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}
```

Now EF Core will automatically generate the correct schema with:

* Students
* Courses
* Enrollments (join table with FKs)

---

# 🧾 Summary

| Concept                 | Description                           |
| ----------------------- | ------------------------------------- |
| **Entity**              | C# class mapped to a table            |
| **DbSet<TEntity>**      | Table representation inside DbContext |
| **DbContext**           | Database session and configuration    |
| **Primary Key**         | Unique identifier (by convention: Id) |
| **Foreign Key**         | Connects two entities                 |
| **Navigation Property** | Object reference to related entity    |
| **Fluent API**          | Programmatic configuration            |
| **Data Annotations**    | Attribute-based configuration         |
