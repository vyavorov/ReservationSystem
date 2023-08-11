using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class SeedDatabse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("6e195895-ca57-4bcc-88d4-90ddb3b112ca"));

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("79b37152-4bac-48fb-9d12-cdc6f4c694f6"), 50, true, "internal" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("79b37152-4bac-48fb-9d12-cdc6f4c694f6"));

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("6e195895-ca57-4bcc-88d4-90ddb3b112ca"), 50, true, "internal" });
        }
    }
}
