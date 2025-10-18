## 💡 What is a Stored Procedure?

> A **Stored Procedure** is a **predefined set of SQL statements** that you save in the database and can run again and again.

It’s like a **function** in programming — but for SQL.

---

### 🧠 Simple definition:

> A **Stored Procedure** is a named SQL script stored in the database that can take **parameters**, perform **operations**, and return **results**.

---

## 🧱 Example (Basic One)

Instead of writing this query every time:

```sql
SELECT * FROM Employees WHERE Department = 'IT';
```

You can save it as a **stored procedure** 👇

```sql
CREATE PROCEDURE GetITEmployees
AS
BEGIN
    SELECT * FROM Employees WHERE Department = 'IT';
END;
```

Now you can run it anytime like this:

```sql
EXEC GetITEmployees;
```

✅ Output → All employees from the IT department.

---

## ⚙️ Example with a **Parameter**

Let’s make it dynamic — so we can pass any department name:

```sql
CREATE PROCEDURE GetEmployeesByDept
    @DeptName NVARCHAR(50)
AS
BEGIN
    SELECT * 
    FROM Employees 
    WHERE Department = @DeptName;
END;
```

Now you can call it like:

```sql
EXEC GetEmployeesByDept @DeptName = 'Finance';
```

✅ Output → All employees from the Finance department.

---

## 🎯 Example with Multiple Parameters

```sql
CREATE PROCEDURE GetEmployeesBySalary
    @MinSalary INT,
    @MaxSalary INT
AS
BEGIN
    SELECT EmpID, Name, Salary
    FROM Employees
    WHERE Salary BETWEEN @MinSalary AND @MaxSalary;
END;
```

Run it:

```sql
EXEC GetEmployeesBySalary @MinSalary = 40000, @MaxSalary = 70000;
```

---

## 📦 Why use Stored Procedures?

| Advantage                      | Explanation                                                          |
| ------------------------------ | -------------------------------------------------------------------- |
| **1. Reusability**             | Write once, use many times                                           |
| **2. Performance**             | SQL Server compiles and optimizes it once, runs faster later         |
| **3. Security**                | Users can execute the procedure without direct access to the tables  |
| **4. Maintainability**         | Easy to update logic in one place                                    |
| **5. Reduced network traffic** | Instead of sending big SQL queries, you just call the procedure name |
| **6. Can contain logic**       | Can use IF, ELSE, loops, variables — like a small program inside SQL |

---

## ⚠️ Disadvantages

| Disadvantage               | Description                                           |
| -------------------------- | ----------------------------------------------------- |
| **1. Complex debugging**   | Harder to debug than application code                 |
| **2. Version control**     | Stored in DB, not in code repository by default       |
| **3. Portability**         | Different SQL dialects have slightly different syntax |
| **4. Overuse can slow DB** | Too much logic in DB layer may cause load             |

---

## 💬 Syntax Summary

```sql
-- Create
CREATE PROCEDURE ProcedureName
    @Parameter1 datatype,
    @Parameter2 datatype
AS
BEGIN
    -- SQL statements
END;

-- Execute
EXEC ProcedureName @Parameter1 = value1, @Parameter2 = value2;

-- Delete
DROP PROCEDURE ProcedureName;
```

---

## 🧠 Real-life Example

```sql
CREATE PROCEDURE InsertNewEmployee
    @Name NVARCHAR(50),
    @Department NVARCHAR(50),
    @Salary INT
AS
BEGIN
    INSERT INTO Employees (Name, Department, Salary)
    VALUES (@Name, @Department, @Salary);

    PRINT 'Employee added successfully';
END;
```

Run it:

```sql
EXEC InsertNewEmployee @Name = 'Ali', @Department = 'IT', @Salary = 60000;
```

---

## ✅ Summary

| Concept              | Description                              |
| -------------------- | ---------------------------------------- |
| **Stored Procedure** | A saved block of SQL code                |
| **Use**              | Run queries or operations easily         |
| **Can have**         | Parameters, variables, conditions, logic |
| **Benefits**         | Reusable, fast, secure, maintainable     |
| **Command to run**   | `EXEC procedure_name`                    |

---

