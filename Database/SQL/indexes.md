## 📘 What is an Index in SQL?

👉 An **index** is like an **index in a book** — it helps SQL find data **faster**.

Without an index, SQL must read **every row** in a table to find something (called a *table scan*).
With an index, SQL can jump **directly to the matching rows**, just like how you use a book’s index to find a topic quickly.

---

## 🧠 Example (Easy analogy)

Imagine you have a book of **1000 pages**:

* Without index → you read every page to find “Cloud Computing” 😩
* With index → you go to the back, find “Cloud Computing → page 342” ✅

That’s exactly how an SQL **index** works.

---

## ⚙️ SQL Example

### Without index:

```sql
SELECT * FROM Employees WHERE Name = 'Ali';
```

SQL will **scan the whole table**.

### With index:

```sql
CREATE INDEX idx_Employees_Name ON Employees(Name);
```

Now SQL will **use the index** to find “Ali” very fast 🔥

---

## 🧩 Syntax

```sql
CREATE INDEX index_name
ON table_name (column1 [, column2, ...]);
```

---

## ⚡ Main Types of Indexes in SQL Server (and most RDBMS)

| Type                       | Description                                                                                                                             | Example                                                                   |
| -------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| **1. Clustered Index**     | Sorts and stores the data rows **physically** in the table in the order of the index. Each table can have **only one** clustered index. | Usually created automatically on **Primary Key**.                         |
| **2. Non-Clustered Index** | Creates a **separate structure** that points to the actual table rows. You can have **many** non-clustered indexes per table.           | Common for frequently searched columns.                                   |
| **3. Unique Index**        | Ensures that **no duplicate values** exist in the indexed column(s).                                                                    | Automatically created on columns with `UNIQUE` or `PRIMARY KEY`.          |
| **4. Composite Index**     | An index on **multiple columns**.                                                                                                       | `CREATE INDEX idx_emp_name_dept ON Employees(Name, Department);`          |
| **5. Full-Text Index**     | Used for **searching words/phrases** inside long text fields.                                                                           | Good for searching in large text or documents.                            |
| **6. Filtered Index**      | Stores index for **subset of rows** (based on a filter condition).                                                                      | `CREATE INDEX idx_active_emp ON Employees(Status) WHERE Status='Active';` |
| **7. Columnstore Index**   | Stores data **column-wise** (instead of row-wise) for fast **analytics and reporting**.                                                 | Used in data warehouses or large datasets.                                |

---

## 🧱 Clustered vs Non-Clustered (Most Important)

| Feature               | Clustered Index                | Non-Clustered Index         |
| --------------------- | ------------------------------ | --------------------------- |
| Storage               | Data is stored in sorted order | Index stored separately     |
| Number per table      | Only **1**                     | Many allowed                |
| Speed                 | Faster for range queries       | Faster for specific lookups |
| Automatically created | On `PRIMARY KEY`               | Must be created manually    |

---

### Example:

```sql
-- Clustered index on primary key
CREATE TABLE Employees (
    EmpID INT PRIMARY KEY,   -- Creates clustered index automatically
    Name NVARCHAR(50),
    Department NVARCHAR(50),
    Salary INT
);

-- Non-clustered index on Name column
CREATE NONCLUSTERED INDEX idx_Emp_Name
ON Employees(Name);
```

---

## 📊 Performance Impact

| Advantage                      | Disadvantage                                                   |
| ------------------------------ | -------------------------------------------------------------- |
| Faster SELECT queries          | Slower INSERT, UPDATE, DELETE (because index must also update) |
| Useful for sorting & searching | Takes extra storage space                                      |
| Reduces table scans            | Must be maintained properly                                    |

---

## 🧠 When to Use Indexes

✅ Use index on:

* Columns used in **WHERE**, **JOIN**, or **ORDER BY**
* Columns frequently searched
* Large tables

❌ Avoid index on:

* Small tables
* Columns frequently updated (causes overhead)
* Columns with many duplicate values (like `Gender`, `Status`)

---

## 💬 Summary

| Concept           | Explanation                               |
| ----------------- | ----------------------------------------- |
| **Index**         | Structure that makes searching faster     |
| **Clustered**     | Data stored in sorted order (1 per table) |
| **Non-clustered** | Separate structure pointing to data       |
| **Unique**        | Prevents duplicate values                 |
| **Composite**     | Index on multiple columns                 |
| **Full-text**     | For searching text                        |
| **Filtered**      | For subset of rows                        |
| **Columnstore**   | For analytical queries                    |

