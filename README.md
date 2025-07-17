# Configurable Workflow Engine, .NET 8 Minimal API

This project is a backend service that implements a configurable workflow engine using .NET 8 Minimal APIs. The system is modeled as a state machine. This setup allows for dynamic workflow definitions, controlled state transitions, and strict rule enforcement at runtime.

---

## What This Project Does

It allows clients to:
1. Define a workflow – States (initial/final) and transitions (actions)
2. Start a workflow instance – Begins at the initial state of the definition
3. Perform transitions (actions) – Moves the instance from one state to another if valid
4. View definitions and instances – For inspection and tracking

---

## How It Works, End to End

### 1. Workflow Definition
A workflow definition is a blueprint that describes:
- A set of states:
  - Each with an Id
  - One and only one must be marked IsInitial = true
  - Any number can be marked IsFinal = true
- A set of transitions (actions):
  - Each has a Name (unique)
  - One ToStateId (destination state)
  - One or more FromStateIds (source states)

Example:
```json
{
  "id": "ticket-approval",
  "states": [
    { "id": "Open", "isInitial": true },
    { "id": "Approved" },
    { "id": "Closed", "isFinal": true }
  ],
