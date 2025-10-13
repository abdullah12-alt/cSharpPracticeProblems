
##  1. What are `VARCHAR` and `NVARCHAR`?

Both are used to **store text** in SQL — like names, addresses, etc.

| Type       | Meaning                                           |
| ---------- | ------------------------------------------------- |
| `VARCHAR`  | Normal text (non-Unicode)                         |
| `NVARCHAR` | Unicode text (supports all languages and symbols) |

---

##  2. Think of it like languages 

Imagine your SQL database is a **school**:

* `VARCHAR` class teaches **only one language** (like English).

  * It doesn’t understand other scripts (Arabic, Urdu, Chinese, Emoji, etc.).
* `NVARCHAR` class teaches **all languages in the world**.

  * It can store any character — English ✅ Urdu ✅ Chinese ✅ Emoji ✅

---

##  3. Difference in how they store data

| Type           | Storage               | Example              | Can store                                  |
| -------------- | --------------------- | -------------------- | ------------------------------------------ |
| `VARCHAR(10)`  | 1 byte per character  | `'Hello'` = 5 bytes  | English only (unless UTF-8 collation used) |
| `NVARCHAR(10)` | 2 bytes per character | `'Hello'` = 10 bytes | All languages (Unicode)                    |

 So, `NVARCHAR` takes **more space** but supports **all characters**.

---

## 📘 4. Example in SQL

### Case 1: Using `VARCHAR`

```sql
CREATE TABLE TestVarchar (
    Name VARCHAR(50)
);

INSERT INTO TestVarchar VALUES ('Abdullah');
INSERT INTO TestVarchar VALUES ('عبداللہ'); -- Urdu text
```

➡️ Result: The Urdu text becomes **?????** or **null** (because `VARCHAR` doesn’t understand it).

---

### Case 2: Using `NVARCHAR`

```sql
CREATE TABLE TestNvarchar (
    Name NVARCHAR(50)
);

INSERT INTO TestNvarchar VALUES (N'Abdullah');
INSERT INTO TestNvarchar VALUES (N'عبداللہ'); -- Urdu text
```

➡️ Result: Both texts store **correctly** 
(*Notice the `N` before the string — it tells SQL “this is Unicode text”*)

---

##  5. Simple summary

| Feature                        | `VARCHAR`               | `NVARCHAR`                  |
| ------------------------------ | ----------------------- | --------------------------- |
| Stores Unicode (all languages) | ❌ No                    | ✅ Yes                       |
| Storage space                  | Smaller                 | Bigger (2x)                 |
| Use when                       | Only English/ASCII text | Multiple languages / emojis |
| Prefix when inserting          | `'text'`                | `N'text'`                   |

---

##  6. Real-life Example

Imagine you make an app for:

* only English users → use `VARCHAR`
* global users (Urdu, Arabic, Chinese, etc.) → use `NVARCHAR`

If you use `VARCHAR` and try to save “عبداللہ” or “李”, they’ll be corrupted.

---

##  7. Quick rule to remember

> 💡 “Use `NVARCHAR` whenever you expect **any non-English text**.
> Use `VARCHAR` only for **English-only data**.”



---


## 💡 What is `IDENTITY` in SQL?

**`IDENTITY`** is a property that makes a column **auto-increment** its value every time you insert a new record.

In simple words:

> It automatically gives a **unique number** to every new row.

---

## 🧱 Example

Let’s say you create a table:

```sql
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1),
    Name NVARCHAR(50)
);
```

Now let’s insert some data:

```sql
INSERT INTO Students (Name) VALUES ('Ali');
INSERT INTO Students (Name) VALUES ('Sara');
INSERT INTO Students (Name) VALUES ('Usman');
```

Result inside the table:

| StudentID | Name  |
| --------- | ----- |
| 1         | Ali   |
| 2         | Sara  |
| 3         | Usman |

---

## ⚙️ How `IDENTITY` works

The syntax is:

```sql
IDENTITY(seed, increment)
```

| Term          | Meaning                          | Example                |
| ------------- | -------------------------------- | ---------------------- |
| **Seed**      | The starting number              | `1` → starts from 1    |
| **Increment** | The amount to increase each time | `1` → goes 1, 2, 3, 4… |

So `IDENTITY(1,1)` starts at 1 and adds 1 each time.
If you write `IDENTITY(100,10)`, it will go: 100, 110, 120, 130…

---

## 🧮 Another example

```sql
CREATE TABLE Orders (
    OrderID INT IDENTITY(1000,5),
    ProductName VARCHAR(50)
);
```

```sql
INSERT INTO Orders (ProductName) VALUES ('Laptop');
INSERT INTO Orders (ProductName) VALUES ('Mouse');
```

| OrderID | ProductName |
| ------- | ----------- |
| 1000    | Laptop      |
| 1005    | Mouse       |

---

## 🚫 You don’t insert the ID manually

If a column is `IDENTITY`, you **don’t** include it in your `INSERT`:

✅ Correct:

```sql
INSERT INTO Students (Name) VALUES ('Ayesha');
```

❌ Wrong:

```sql
INSERT INTO Students (StudentID, Name) VALUES (10, 'Ayesha');
```

(SQL will give an error unless you turn on `IDENTITY_INSERT` manually.)

---

## 🧠 Why do we use it?

* To **auto-generate primary keys**
* To make sure each record has a **unique ID**
* To avoid manual numbering errors

---

## ⚡ Summary

| Feature            | Description                             |
| ------------------ | --------------------------------------- |
| **What**           | Auto-generates numbers for each new row |
| **Syntax**         | `IDENTITY(seed, increment)`             |
| **Common example** | `IDENTITY(1,1)` → 1, 2, 3, 4…           |
| **Used for**       | Primary key columns                     |
| **Manual insert?** | Not by default                          |

---


**disadvantages and limitations** of using `IDENTITY` in SQL 
---

## ⚠️ 1. You **can’t easily control** the number sequence

Once the numbers start increasing (1, 2, 3…),
you **can’t reset or reuse** numbers automatically.

Example:

```sql
DELETE FROM Students WHERE StudentID = 3;
```

➡️ The next ID won’t become 3 again — it becomes **4**.
So your IDs can have **gaps** in them.

---

## ⚠️ 2. **Gaps appear** after delete or failed inserts

If an insert fails or a row is deleted, that number is **lost forever**.

Example:

```
IDs: 1, 2, (failed), 4, 5
```

ID 3 is missing — and you can’t get it back automatically.

---

## ⚠️ 3. You **can’t update** or **insert** your own ID easily

By default, SQL Server won’t let you insert your own value into an identity column:

```sql
INSERT INTO Students (StudentID, Name) VALUES (10, 'Ali');
-- ❌ Error: Cannot insert explicit value for identity column
```

To do it, you must temporarily enable:

```sql
SET IDENTITY_INSERT Students ON;
```

But that’s not recommended in normal use.

---

## ⚠️ 4. **Identity values can “jump”** after a restart or rollback

In some versions of SQL Server (like 2012+),
if the server restarts or transaction rolls back,
the identity value might **skip numbers** (e.g. jump from 10 to 1001).

This happens because SQL pre-allocates identity values in memory for speed.
(You can control it using `DBCC CHECKIDENT` or sequence objects.)

---

## ⚠️ 5. **No built-in reset** for identity

If you want to start numbering from 1 again after clearing data,
you must manually run:

```sql
DBCC CHECKIDENT ('Students', RESEED, 0);
```

---

## ⚠️ 6. **Identity ≠ guaranteed order**

Many people think identity means “sorted by insert order” —
but SQL **does not guarantee** that.
If you query the table, you must use `ORDER BY` to get correct sequence.

---

## ⚠️ 7. **Difficult for distributed systems**

If you have multiple servers or databases inserting data,
identity numbers can **clash** (same IDs generated on different machines).
In those cases, people use **GUIDs** or **SEQUENCE** objects instead.

---

## ⚡ Summary

| Disadvantage            | Description                      |
| ----------------------- | -------------------------------- |
| Gaps appear             | When you delete rows or rollback |
| Hard to reset           | Need manual reseed               |
| Not user-controllable   | SQL auto-generates numbers       |
| Can skip values         | After restart or error           |
| Not globally unique     | Only unique *within one table*   |
| Doesn’t guarantee order | You must use `ORDER BY` manually |

---

### ✅ When to use

Use `IDENTITY` when:

* You just need simple, local numbering
* Small/medium system
* Not sharing data across servers

### ❌ Avoid it when

* You need no gaps
* You need globally unique IDs
* You want complete control over ID generation

---

