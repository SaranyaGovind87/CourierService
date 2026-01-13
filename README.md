# 📦 Courier Service Delivery System 

A high-performance logistics engine designed to calculate delivery costs and optimize fleet scheduling. This system uses advanced combinatorial logic to maximize vehicle utility while providing precise delivery time estimations.

---

## 🏗 Architectural Overview

The project is built using a **Layered Architecture**, ensuring that business rules, data models, and infrastructure (logging/UI) are strictly separated.

### 1. Domain Layer (`Models/`)

* **Package & Vehicle:** Core entities using `decimal` types for high-precision weight and time calculations.
* **Encapsulation:** Properties are designed to prevent invalid states (e.g., negative distances).

### 2. Application Layer (`Services/` & `Strategies/`)

* **Offer Strategy Pattern:** Decouples discount logic from cost calculation. Adding a new offer (e.g., `OFR005`) requires zero changes to the core engine.
* **FleetService (Optimization Hub):** Implements a **Greedy Combinatorial Algorithm** to solve the "Knapsack-style" problem of grouping packages based on weight limits.

### 3. Infrastructure Layer (`Logging/`)

* **Singleton FileLogger:** A thread-safe logger that provides real-time feedback in the console (color-coded) and maintains a persistent audit trail in `delivery_log.txt`.

---

## 🧠 Core Logic & Error Handling

### Scheduling Logic (Problem 02)

The system uses a recursive subset generator to evaluate every possible shipment combination. It selects the optimal group based on:

1. **Max Package Count** (Primary)
2. **Max Total Weight** (Secondary)
3. **Min Max Distance** (Tertiary - ensures vehicles return to the hub faster).

### Robust Error Handling

* **Sequence Protection:** Uses defensive checks (`if count == 0`) to prevent `InvalidOperationException` when no packages fit.
* **Deadlock Prevention:** The engine detects "Unshippable" packages (overweight) and logs an error instead of entering an infinite loop.
* **Precision Math:** Implements `Math.Floor(time * 100) / 100` to match strict 2-decimal truncation requirements from the challenge documentation.

---

## 🚀 Step-by-Step Execution Guide

### 1. Environment Setup

* Ensure **.NET 10 SDK** is installed (`dotnet --version`).
* Open the solution in Visual Studio 2022 or VS Code.

### 2. Running the Console Application

1. Right-click the `CourierService` project and select **Set as Startup Project**.
2. Press `F5` or run:
```bash
dotnet run --project CourierService

```


3. **Input Example:**
Console is developed with user interative comments for input the data



### 3. Running Automated Tests (xUnit)

To verify the logic against the Everest Engineering requirements:

1. Open the **Test Explorer** in Visual Studio.
2. Click **Run All Tests** or use the CLI:
```bash
dotnet test

```


3. **Key Tests:**
* `CalculateTimes_ShouldMatchRequirementExample`: Validates the exact delivery times (e.g., PKG4 at 0.85).
* `CalculateDeliveryTimes_WithOverweightPackage_ShouldLogAndExit`: Validates the new Logger and error-handling logic.



---

## 📂 Logging & Diagnostics

All system activity is recorded in `delivery_log.txt` located in your execution directory.

* **[INFO]:** Tracks vehicle assignments and trip starts.
* **[ERROR]:** Highlights packages that cannot be delivered due to weight constraints.

---
