using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class Equipmentmanytomanyfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("b639cd04-128a-433b-a998-141cf6a246de"));

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("eb0afcc1-7ba1-48cf-bab6-c26a67e16f29"), 50, "internal" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("eb0afcc1-7ba1-48cf-bab6-c26a67e16f29"));

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("b639cd04-128a-433b-a998-141cf6a246de"), 50, "internal" });
        }
    }
}
