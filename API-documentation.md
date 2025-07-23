## Base Route: /api/patient
## Controller: PatientApiController
## Purpose: Manage patients and their vital signs

1. GET /api/patient
Description

Retrieves all registered patients.
Response

    200 OK – Returns a list of patients.

<pre> ```json
  {
    "id": 1,
    "name": "John Doe",
    "roomNumber": "101"
  },
  ...
``` </pre>


2. GET /api/patient/{id}/vitals
Description

Retrieves all recorded vital signs for a specific patient.
Parameters

    id (int) – Patient ID

Response

    200 OK – Returns a list of vital signs for the patient

    404 Not Found – If no vital signs exist for the given patient

<pre> ```json
  {
    "id": 25,
    "patientId": 1,
    "heartRate": 72,
    "systolicBP": 120,
    "diastolicBP": 80,
    "oxygenSaturation": 98,
    "timestamp": "2025-07-22T14:55:23Z"
  },
  ...

{
  "message": "No vital signs found for patient ID 1."
}
``` </pre>


3. POST /api/patient/{id}/vitals
Description

Posts new vital sign data for a patient and broadcasts it via SignalR.
Parameters

    id (int) – Patient ID

Request Body
<pre> ```json
{
  "heartRate": 75,
  "systolicBP": 120,
  "diastolicBP": 80,
  "oxygenSaturation": 97
}
``` </pre>
Validations

    heartRate: 0–200

    systolicBP: 50–250

    diastolicBP: 30–150

    oxygenSaturation: 50–100

Response

    200 OK – Returns the saved vital sign

    400 Bad Request – If input values are outside allowed ranges
<pre> ```json
{
  "id": 50,
  "patientId": 1,
  "heartRate": 75,
  "systolicBP": 120,
  "diastolicBP": 80,
  "oxygenSaturation": 97,
  "timestamp": "2025-07-23T08:01:12Z"
}
``` </pre>
4. GET /api/patient/{id}/vitals/history
Description

Returns the patient's vital signs recorded in the last 24 hours.
Parameters

    id (int) – Patient ID

Response

    200 OK – List of vital signs in chronological order
    (empty list if no data in the last 24 hours)

<pre> ```json
  {
    "id": 31,
    "patientId": 1,
    "heartRate": 70,
    "systolicBP": 118,
    "diastolicBP": 78,
    "oxygenSaturation": 96,
    "timestamp": "2025-07-23T07:10:00Z"
  },
  ...
``` </pre>


