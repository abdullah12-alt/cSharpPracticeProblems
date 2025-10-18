## 🔍 1. What is a Trigger?

A **trigger** is a **special kind of stored procedure** in SQL that **automatically executes** when an event occurs on a table or a view.

You **don’t call** a trigger manually — the **database engine executes it automatically** in response to:

* `INSERT`
* `UPDATE`
* `DELETE`

---

## ⚙️ 2. How Triggers Work

When an event occurs (say, an `UPDATE`), the SQL engine automatically creates two **temporary (virtual) tables**:

| Virtual Table | Description                                   |
| ------------- | --------------------------------------------- |
| **inserted**  | Holds new data after an `INSERT` or `UPDATE`  |
| **deleted**   | Holds old data before an `UPDATE` or `DELETE` |

Example:

```sql
UPDATE Employees SET Salary = 60000 WHERE EmpID = 1;
```

* `deleted` → contains the old record of EmpID = 1
* `inserted` → contains the new record (Salary = 60000)

You can use these virtual tables *inside triggers* to perform actions such as logging, validation, cascading updates, etc.

---

## 🧩 3. Types of Triggers

Different SQL databases have slightly different types,
but in **SQL Server**, the main types are:

| Type                                | Fires When                | Description                                         |
| ----------------------------------- | ------------------------- | --------------------------------------------------- |
| **AFTER INSERT**                    | After an insert statement | Executes after data is inserted                     |
| **AFTER UPDATE**                    | After an update statement | Executes after data is updated                      |
| **AFTER DELETE**                    | After a delete statement  | Executes after data is deleted                      |
| **INSTEAD OF INSERT/UPDATE/DELETE** | Instead of the event      | Used mainly on *views* to override default behavior |

---

### 🔹 Example 1: AFTER INSERT Trigger

```sql
CREATE TRIGGER trg_AfterInsertEmployee
ON Employees
AFTER INSERT
AS
BEGIN
    INSERT INTO AuditLog (TableName, Action, RecordID, ActionTime)
    SELECT 'Employees', 'INSERT', EmpID, GETDATE()
    FROM inserted;
END;
```

✅ This trigger logs every new employee added.

---

### 🔹 Example 2: AFTER UPDATE Trigger

```sql
CREATE TRIGGER trg_AfterUpdateSalary
ON Employees
AFTER UPDATE
AS
BEGIN
    INSERT INTO SalaryLog (EmpID, OldSalary, NewSalary, ChangeDate)
    SELECT d.EmpID, d.Salary, i.Salary, GETDATE()
    FROM deleted d
    JOIN inserted i ON d.EmpID = i.EmpID;
END;
```

✅ This logs salary changes automatically.

---

### 🔹 Example 3: AFTER DELETE Trigger

```sql
CREATE TRIGGER trg_AfterDeleteEmployee
ON Employees
AFTER DELETE
AS
BEGIN
    INSERT INTO AuditLog (TableName, Action, RecordID, ActionTime)
    SELECT 'Employees', 'DELETE', EmpID, GETDATE()
    FROM deleted;
END;
```

✅ This logs when an employee record is deleted.

---

### 🔹 Example 4: INSTEAD OF Trigger

Used mostly on **views**.

Example:

```sql
CREATE VIEW vw_ActiveEmployees AS
SELECT * FROM Employees WHERE IsActive = 1;
GO

CREATE TRIGGER trg_InsteadOfDelete
ON vw_ActiveEmployees
INSTEAD OF DELETE
AS
BEGIN
    UPDATE Employees
    SET IsActive = 0
    WHERE EmpID IN (SELECT EmpID FROM deleted);
END;
```

✅ This trigger **prevents physical deletion** — instead, it sets `IsActive = 0`.

---

## ⚙️ 4. Trigger Operations

| Operation  | Description                    | Virtual Tables Used           |
| ---------- | ------------------------------ | ----------------------------- |
| **INSERT** | When new data is added         | `inserted`                    |
| **UPDATE** | When existing data is modified | both `inserted` and `deleted` |
| **DELETE** | When data is removed           | `deleted`                     |

---

## ✅ 5. Advantages of Triggers

| Advantage          | Description                                                          |
| ------------------ | -------------------------------------------------------------------- |
| **Automation**     | Executes automatically without manual intervention.                  |
| **Data Integrity** | Enforces complex business rules at the database level.               |
| **Audit Trail**    | Automatically logs changes (insert/update/delete).                   |
| **Consistency**    | Ensures consistent behavior across applications.                     |
| **Security**       | Can restrict or modify certain operations (via INSTEAD OF triggers). |

---

## ⚠️ 6. Disadvantages of Triggers

| Disadvantage             | Description                                                                    |
| ------------------------ | ------------------------------------------------------------------------------ |
| **Hidden Logic**         | Business logic hidden inside triggers can confuse developers.                  |
| **Performance Overhead** | Too many triggers slow down DML operations (insert/update/delete).             |
| **Debugging Difficulty** | Hard to debug since triggers run automatically.                                |
| **Cascade Problems**     | Multiple triggers on related tables can cause recursive or cyclic updates.     |
| **Limited Control**      | Triggers always fire on events — can’t be easily turned off for one operation. |

---

## 🧰 7. Best Practices for Using Triggers

1. ✅ Keep triggers **short and efficient**.
2. 🧾 Use them mainly for **auditing or enforcing integrity** — not for main business logic.
3. 🧮 Avoid triggers that **call other triggers recursively**.
4. 📜 Always document what each trigger does.
5. 🔒 Don’t use triggers for large-scale data changes — use stored procedures instead.

---

## 🧠 8. Summary Table

| Feature            | Description                                          |
| ------------------ | ---------------------------------------------------- |
| **Definition**     | Automatic SQL procedure fired by DML events          |
| **Main Types**     | AFTER INSERT, AFTER UPDATE, AFTER DELETE, INSTEAD OF |
| **Virtual Tables** | `inserted`, `deleted`                                |
| **Use Cases**      | Auditing, enforcing rules, data validation           |
| **Advantages**     | Automation, integrity, security                      |
| **Disadvantages**  | Performance, hidden logic, debugging difficulty      |


## 🧱 Step 1: Create a Main Table

We’ll track employees and their salaries.

```sql
CREATE TABLE Employees (
    EmpID INT IDENTITY PRIMARY KEY,
    EmpName NVARCHAR(50),
    Department NVARCHAR(50),
    Salary DECIMAL(10,2)
);
```

✅ This is our **main business table**.

---

## 📜 Step 2: Create a Log Table

We’ll log every change (insert/update/delete) here.

```sql
CREATE TABLE EmployeeLog (
    LogID INT IDENTITY PRIMARY KEY,
    EmpID INT,
    ActionType NVARCHAR(20),
    OldSalary DECIMAL(10,2) NULL,
    NewSalary DECIMAL(10,2) NULL,
    ActionTime DATETIME DEFAULT GETDATE()
);
```

✅ This will store what happened, when, and the old/new salary values.

---

## 🚀 Step 3: Create an AFTER INSERT Trigger

Whenever a new employee is added, log it.

```sql
CREATE TRIGGER trg_AfterInsert_Employee
ON Employees
AFTER INSERT
AS
BEGIN
    INSERT INTO EmployeeLog (EmpID, ActionType, NewSalary)
    SELECT EmpID, 'INSERT', Salary
    FROM inserted;
END;
```

✅ Trigger automatically runs **after an insert**.

---

## 🔁 Step 4: Create an AFTER UPDATE Trigger

When salary or department changes, log the old and new salary.

```sql
CREATE TRIGGER trg_AfterUpdate_Employee
ON Employees
AFTER UPDATE
AS
BEGIN
    INSERT INTO EmployeeLog (EmpID, ActionType, OldSalary, NewSalary)
    SELECT d.EmpID, 'UPDATE', d.Salary, i.Salary
    FROM deleted d
    JOIN inserted i ON d.EmpID = i.EmpID;
END;
```

✅ Trigger automatically runs **after update**, comparing `deleted` (old) and `inserted` (new) data.

---

## ❌ Step 5: Create an AFTER DELETE Trigger

When an employee is deleted, log the deletion.

```sql
CREATE TRIGGER trg_AfterDelete_Employee
ON Employees
AFTER DELETE
AS
BEGIN
    INSERT INTO EmployeeLog (EmpID, ActionType, OldSalary)
    SELECT EmpID, 'DELETE', Salary
    FROM deleted;
END;
```

✅ This runs **after delete** — logs who was removed.

---

## 🧪 Step 6: Test Everything

### 👉 Insert Data

```sql
INSERT INTO Employees (EmpName, Department, Salary)
VALUES ('Ali Khan', 'IT', 50000),
       ('Sara Malik', 'HR', 45000);
```

Now check:

```sql
SELECT * FROM Employees;
SELECT * FROM EmployeeLog;
```

You’ll see 2 entries in EmployeeLog with **ActionType = 'INSERT'**.

---

### 👉 Update Data

```sql
UPDATE Employees SET Salary = 60000 WHERE EmpName = 'Ali Khan';
```

Check again:

```sql
SELECT * FROM EmployeeLog;
```

You’ll see a new entry:

* ActionType = `UPDATE`
* OldSalary = 50000
* NewSalary = 60000

---

### 👉 Delete Data

```sql
DELETE FROM Employees WHERE EmpName = 'Sara Malik';
```

Check logs again:

```sql
SELECT * FROM EmployeeLog;
```

You’ll see:

* ActionType = `DELETE`
* OldSalary = 45000

---

## 🧩 Step 7: Review the Flow

| Operation | Trigger Fired              | Virtual Table Used | Logged Info      |
| --------- | -------------------------- | ------------------ | ---------------- |
| INSERT    | `trg_AfterInsert_Employee` | inserted           | New salary       |
| UPDATE    | `trg_AfterUpdate_Employee` | inserted + deleted | Old + New salary |
| DELETE    | `trg_AfterDelete_Employee` | deleted            | Old salary       |

---

## ✅ Advantages (in this example)

* You **automatically log all changes** — no app logic needed
* Keeps **audit history** of every employee’s salary change
* Maintains **data integrity** directly at the database level

---

## ⚠️ Disadvantages

* If your application does **bulk inserts/updates**, triggers can slow performance.
* Logic is **hidden** — another developer might not know a trigger is firing.
* Difficult to **debug** if multiple triggers run on the same table.

---

## 💡 Tip

To view all triggers in SQL Server:

```sql
SELECT name, parent_class_desc, create_date
FROM sys.triggers;
```

To disable or enable a trigger:

```sql
DISABLE TRIGGER trg_AfterInsert_Employee ON Employees;
ENABLE TRIGGER trg_AfterInsert_Employee ON Employees;
```

