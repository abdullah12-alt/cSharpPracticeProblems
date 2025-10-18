## 🧩 What is a **View** in SQL?

Think of a **View** as a **“virtual table”** —
it looks like a table, but it **doesn’t actually store data**.

It just **shows** data from one or more real tables using a **saved SELECT query**.

---

### 🧠 Simple definition:

> A **View** is a saved SQL query that you can treat like a table.

---

## 📘 Example

Let’s say you have a table called `Employees`:

| EmpID | Name   | Department | Salary |
| ----- | ------ | ---------- | ------ |
| 1     | Ali    | HR         | 50000  |
| 2     | Sara   | IT         | 60000  |
| 3     | Usman  | IT         | 70000  |
| 4     | Ayesha | Finance    | 55000  |

Now you want to see **only IT employees** again and again.
Instead of writing the same query every time, you can **create a view**.

---

### ✅ Create a View

```sql
CREATE VIEW IT_Employees AS
SELECT Name, Department, Salary
FROM Employees
WHERE Department = 'IT';
```

Now you can use this **like a table**:

```sql
SELECT * FROM IT_Employees;
```

Output:

| Name  | Department | Salary |
| ----- | ---------- | ------ |
| Sara  | IT         | 60000  |
| Usman | IT         | 70000  |

---

## ⚙️ How it works

* The **view does not store data**; it stores only the **query**.
* Whenever you run:

  ```sql
  SELECT * FROM IT_Employees;
  ```

  SQL actually runs the query inside the view.

---

## 🌟 Advantages of Views

| Advantage                         | Explanation                                                               |
| --------------------------------- | ------------------------------------------------------------------------- |
| **1. Simplifies complex queries** | You can save long queries as a view and reuse them easily.                |
| **2. Security / Privacy**         | You can show only specific columns (e.g., hide salary from normal users). |
| **3. Reusability**                | You can use the same view in multiple reports, queries, or apps.          |
| **4. Consistency**                | Everyone uses the same logic — no mistakes or query changes.              |
| **5. Easier maintenance**         | If table structure changes, you only update the view once.                |
| **6. Logical separation**         | Keeps your main data safe — users can query the view, not the table.      |

---

## ⚠️ Disadvantages (just to know)

* Views don’t store data, so **they can be slower** for very complex queries.
* Some views **cannot be updated** (especially when made from joins or group by).
* If base tables change (columns renamed or removed), view might break.

---

## 💡 Summary

| Feature         | Description                                                                  |
| --------------- | ---------------------------------------------------------------------------- |
| What is a View? | A virtual table based on a SELECT query                                      |
| Stores data?    | ❌ No                                                                         |
| Purpose         | Simplify queries, improve security, and reuse logic                          |
| Example         | `CREATE VIEW IT_Employees AS SELECT * FROM Employees WHERE Department='IT';` |

