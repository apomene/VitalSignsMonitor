using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VitalSignsMonitor.Migrations
{
    /// <inheritdoc />
    public partial class SeedVitalSigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VitalSigns",
                columns: new[] { "Id", "DiastolicBP", "HeartRate", "OxygenSaturation", "PatientId", "SystolicBP", "Timestamp" },
                values: new object[,]
                {
                    { 1, 80, 80, 97, 1, 120, new DateTime(2025, 7, 22, 7, 30, 54, 42, DateTimeKind.Utc).AddTicks(5810) },
                    { 2, 85, 95, 92, 2, 135, new DateTime(2025, 7, 22, 7, 20, 54, 42, DateTimeKind.Utc).AddTicks(6122) },
                    { 3, 95, 110, 88, 3, 145, new DateTime(2025, 7, 22, 7, 10, 54, 42, DateTimeKind.Utc).AddTicks(6127) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
