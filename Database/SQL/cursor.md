
## 💡 What is a Cursor in SQL?

> A **cursor** is a database object that allows you to **process rows one by one** from a result set.

Normally, SQL works with **sets of data** (all rows at once).
But sometimes you need to perform an action **row-by-row** — that’s when you use a **cursor**.

---

## 🧠 Simple example idea

Imagine you have a table of employees and you want to **give each employee a bonus** based on their department.
You can’t do it easily in a single query if the logic is complex —
so you use a **cursor** to:

1. Read one row (employee),
2. Perform some logic,
3. Move to the next row.

---

## 📘 Example Table

| EmpID | EmpName | Salary |
| ----- | ------- | ------ |
| 1     | Ali     | 50000  |
| 2     | Sara    | 60000  |
| 3     | Usman   | 70000  |

---

## ⚙️ Example of a Cursor

Let’s increase everyone’s salary by 10%.

```sql
DECLARE @EmpID INT, @Salary INT;

-- Step 1: Declare the cursor
DECLARE emp_cursor CURSOR FOR
SELECT EmpID, Salary FROM Employees;

-- Step 2: Open the cursor
OPEN emp_cursor;

-- Step 3: Fetch the first row
FETCH NEXT FROM emp_cursor INTO @EmpID, @Salary;

-- Step 4: Loop through each row
WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE Employees
    SET Salary = @Salary * 1.10
    WHERE EmpID = @EmpID;

    -- Fetch next row
    FETCH NEXT FROM emp_cursor INTO @EmpID, @Salary;
END;

-- Step 5: Close and deallocate
CLOSE emp_cursor;
DEALLOCATE emp_cursor;
```

✅ What happens:

* The cursor goes through each employee one by one.
* For each one, it updates their salary.

---

## 🧱 Steps of Using a Cursor

| Step | Command                                      | Purpose                           |
| ---- | -------------------------------------------- | --------------------------------- |
| 1    | `DECLARE cursor_name CURSOR FOR ...`         | Define which data to loop through |
| 2    | `OPEN cursor_name`                           | Open it for use                   |
| 3    | `FETCH NEXT FROM cursor_name INTO variables` | Get one row of data               |
| 4    | `WHILE @@FETCH_STATUS = 0 ...`               | Process each row                  |
| 5    | `CLOSE cursor_name`                          | Close the cursor                  |
| 6    | `DEALLOCATE cursor_name`                     | Free memory                       |

---

## 🌟 When to Use a Cursor

✅ Use when:

* You must handle **row-by-row logic** (like sending an email per user, applying a different formula per record, etc.)
* You can’t solve it with standard **set-based SQL** (like `UPDATE`, `SUM`, or `JOIN`).

❌ Avoid when:

* You can solve the same problem using **set-based SQL queries** — those are much **faster**.

---

## ⚡ Disadvantages of Cursors

| Disadvantage           | Explanation                                               |
| ---------------------- | --------------------------------------------------------- |
| **Slow**               | Processes one row at a time (not optimized like SQL sets) |
| **Uses more memory**   | Keeps data in memory while looping                        |
| **Harder to maintain** | Code becomes long and complex                             |
| **Locking**            | Can lock rows or tables during processing                 |

---

## ✅ Best Practice

Try to **avoid cursors** when possible and use:

* `UPDATE` with `JOIN`
* `CASE` expressions
* `MERGE`
* Temporary tables
* Window functions

Cursors are good for **special cases** only — like procedural tasks or data migrations.

---

## 🧠 Quick Summary

| Concept           | Description                                       |
| ----------------- | ------------------------------------------------- |
| **Cursor**        | Used to go through rows one by one                |
| **When to use**   | When set-based SQL can’t handle the logic         |
| **Main commands** | `DECLARE`, `OPEN`, `FETCH`, `CLOSE`, `DEALLOCATE` |
| **Drawback**      | Slower and memory-heavy                           |
| **Better use**    | For complex, row-based processing                 |

