-- init.sql

DROP TABLE IF EXISTS VitalSigns;
DROP TABLE IF EXISTS Patients;

CREATE TABLE Patients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Age INTEGER NOT NULL,
    RoomNumber TEXT NOT NULL
);

CREATE TABLE VitalSigns (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId INTEGER NOT NULL,
    Timestamp DATETIME NOT NULL,
    HeartRate INTEGER NOT NULL,
    SystolicBP INTEGER NOT NULL,
    DiastolicBP INTEGER NOT NULL,
    OxygenSaturation INTEGER NOT NULL,
    FOREIGN KEY (PatientId) REFERENCES Patients (Id) ON DELETE CASCADE
);

-- Seed Patients
INSERT INTO Patients (Id, Name, Age, RoomNumber) VALUES
(1, 'John Doe', 45, '101'),
(2, 'Jane Smith', 32, '102'),
(3, 'Bob Johnson', 67, '103');

-- Optionally seed VitalSigns with historical data
INSERT INTO VitalSigns (PatientId, Timestamp, HeartRate, SystolicBP, DiastolicBP, OxygenSaturation) VALUES
(1, DATETIME('now', '-1 hours'), 78, 115, 75, 97),
(2, DATETIME('now', '-30 minutes'), 92, 130, 85, 93),
(3, DATETIME('now', '-10 minutes'), 105, 145, 95, 88);
