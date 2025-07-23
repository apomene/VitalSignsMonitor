# 🏗 Architecture Overview: VitalSigns Monitor Web App

## 📚 Overview
The **VitalSigns Monitor** is a web-based healthcare monitoring application built with **ASP.NET Core MVC**. It allows real-time tracking and visualization of patient vital signs such as heart rate, blood pressure, and oxygen saturation.

The application supports both **real-time updates** via SignalR and **historical data review** via RESTful APIs.

---

## 🧱 Key Architectural Components

### 1. Presentation Layer (MVC)
- **Views (Razor Views)**: Displays patient vitals in real-time and historical charts using Chart.js.
- **Controllers**: Handle user requests and communicate with services or the data layer.
- **SignalR Integration**: Enables live streaming of vital sign data to connected clients.

### 2. API Layer
- Exposes RESTful endpoints for:
  - Listing patients
  - Retrieving patient vitals (latest or historical)
  - Submitting new vital sign data
- Uses `[ApiController]` with routing like `/api/patient/{id}/vitals`.

### 3. Real-time Communication
- **SignalR Hub (`VitalSignsHub`)**
  - Pushes new vital sign data to all connected clients via `ReceiveVital`.
  - Integrated with the `PostVitalSigns` API method.

### 4. Data Access Layer
- **Entity Framework Core (EF Core)**:
  - Uses `ApplicationDbContext` to interact with a SQL or in-memory database.
  - Models include `Patient`, `VitalSign`, and DTOs like `Vitals`.

### 5. Models
- `Patient`: Stores patient metadata (e.g., name, room number).
- `VitalSign`: Stores heart rate, blood pressure, oxygen saturation, and timestamps.
- `Vitals` (DTO): Input model for submitting new vital signs.

---

## 🛠 Technologies Used

| Layer         | Technology                        |
|---------------|-----------------------------------|
| Backend       | ASP.NET Core MVC (.NET 9)         |
| Frontend      | Razor Views, Bootstrap, Chart.js  |
| Real-time     | SignalR                           |
| Data Access   | Entity Framework Core             |
| Testing       | NUnit, Moq                        |


---

## 🔄 Data Flow Summary

1. **Patient submits vitals** via a POST request or simulated device.
2. **API stores** the vitals in the database and **notifies all clients** using SignalR.
3. **UI updates** the table and chart in real-time.
4. Historical vitals are fetched on request for visualization.

---

## 🧪 Testing Strategy

- Unit tests written with **NUnit** and **Moq**.
- Controller tests validate:
  - Proper HTTP status codes
  - Input validation
  - SignalR broadcast behavior
- Tests use **InMemoryDatabase** for fast execution.

---

## 🗂 Folder Structure (Simplified)

VitalSignsMonitor/
├── Controllers/
│ └── PatientApiController.cs
├── Hubs/
│ └── VitalSignsHub.cs
├── Models/
│ ├── Patient.cs
│ ├── VitalSign.cs
│ └── Vitals.cs (DTO)
├── Views/
│ └── Patient/
│ └── LiveVitals.cshtml
├── wwwroot/
│ └── js/
├── VitalSignsTests/
│ └── PatientApiControllerTests.cs

