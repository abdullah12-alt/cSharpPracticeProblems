
## 💡 What are ACID Properties?

> **ACID** stands for **Atomicity, Consistency, Isolation, and Durability**.

These four properties ensure that **database transactions** are **reliable**, **correct**, and **safe** — even if there’s an error, crash, or power failure.

---

## 🧱 What is a Transaction?

A **transaction** is a **group of SQL statements** that must all succeed or all fail together.

Example:

```sql
BEGIN TRANSACTION;

UPDATE Accounts SET Balance = Balance - 1000 WHERE AccNo = 101;  -- debit
UPDATE Accounts SET Balance = Balance + 1000 WHERE AccNo = 202;  -- credit

COMMIT;  -- Save both changes
```

If something fails midway (e.g., system crash), the whole transaction is **rolled back** — this is what ACID ensures.

---

## 🔹 A → Atomicity

### Meaning:

> **All or nothing** rule.

A transaction must be **fully completed** or **fully cancelled**.

If one part fails, **everything rolls back**.

🧠 Example:
If you transfer money from Account A to B:

* Debit A succeeds,
* Credit B fails → SQL **undoes the debit** too.

✅ Either both happen
❌ or none happen.

---

## 🔹 C → Consistency

### Meaning:

> The database must stay **in a valid state** before and after the transaction.

All rules, constraints, and relationships (like foreign keys) must remain valid.

🧠 Example:
If you move ₹1000 from A to B,
Total money in the bank should remain the same — it cannot disappear or double.

✅ The transaction keeps data logically correct.

---

## 🔹 I → Isolation

### Meaning:

> Transactions running at the same time **don’t interfere** with each other.

Each transaction works **as if it’s the only one** running.

🧠 Example:
If two people transfer money at the same time,
their transactions shouldn’t mix up or read each other’s unfinished data.

SQL uses **isolation levels** to control this:

* **READ UNCOMMITTED** → can see uncommitted data (dirty reads)
* **READ COMMITTED** → default, sees only committed data
* **REPEATABLE READ** → prevents changing data during transaction
* **SERIALIZABLE** → highest isolation, transactions run one by one logically

---

## 🔹 D → Durability

### Meaning:

> Once a transaction is **committed**, it is **permanent**, even if the system crashes.

🧠 Example:
After you successfully transfer money,
even if the server restarts — the data is safe and stored on disk.

✅ Committed changes are never lost.

---

## 🧠 Summary Table

| Property            | Meaning                      | Example                                   |
| ------------------- | ---------------------------- | ----------------------------------------- |
| **A - Atomicity**   | All or nothing               | Either both debit & credit happen or none |
| **C - Consistency** | Keeps data valid             | Total balance remains same                |
| **I - Isolation**   | Transactions don’t interfere | Two transfers don’t overlap incorrectly   |
| **D - Durability**  | Changes are permanent        | Data survives crash after COMMIT          |

---

## ⚙️ Why ACID is Important

| Benefit                      | Description                                  |
| ---------------------------- | -------------------------------------------- |
| **Data accuracy**            | No partial or broken updates                 |
| **Data safety**              | Survives failures and errors                 |
| **Concurrency control**      | Prevents conflicts between users             |
| **Trustworthy transactions** | Ensures banking, booking, etc. work reliably |

---

## 💬 Real-world Example (Bank Transfer)

| Step | Action                          | ACID Property |
| ---- | ------------------------------- | ------------- |
| 1    | Subtract 1000 from Account A    | Atomicity     |
| 2    | Add 1000 to Account B           | Atomicity     |
| 3    | Total balance consistent        | Consistency   |
| 4    | Another transaction runs safely | Isolation     |
| 5    | Data saved permanently          | Durability    |

---

✅ **In short:**

> **ACID = Reliability of transactions**
> Always remember → **All or nothing, Correct, Isolated, Durable**

