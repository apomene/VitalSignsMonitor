using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitalSignsMonitor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVitalSignsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2025, 7, 21, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 7, 21, 12, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 3,
                column: "Timestamp",
                value: new DateTime(2025, 7, 21, 13, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2025, 7, 22, 7, 30, 54, 42, DateTimeKind.Utc).AddTicks(5810));

            migrationBuilder.UpdateData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 7, 22, 7, 20, 54, 42, DateTimeKind.Utc).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "VitalSigns",
                keyColumn: "Id",
                keyValue: 3,
                column: "Timestamp",
                value: new DateTime(2025, 7, 22, 7, 10, 54, 42, DateTimeKind.Utc).AddTicks(6127));
        }
    }
}
