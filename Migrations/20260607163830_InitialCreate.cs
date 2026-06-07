using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenFactory.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnergyRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MeterName = table.Column<string>(type: "TEXT", nullable: false),
                    kWh = table.Column<double>(type: "REAL", nullable: false),
                    WaterM3 = table.Column<double>(type: "REAL", nullable: false),
                    CarbonKgCO2e = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KpiTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MeterName = table.Column<string>(type: "TEXT", nullable: false),
                    EnergyTarget = table.Column<double>(type: "REAL", nullable: false),
                    EnergyThreshold = table.Column<double>(type: "REAL", nullable: false),
                    WaterTarget = table.Column<double>(type: "REAL", nullable: false),
                    WaterThreshold = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpiTargets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyRecords");

            migrationBuilder.DropTable(
                name: "KpiTargets");
        }
    }
}
