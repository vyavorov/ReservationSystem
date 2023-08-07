using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class AddUserFirstNameAndLastName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("a39f5c7e-d5a2-43c9-86c8-cf584a4f8278"));

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("c6153451-3af6-48be-8577-d2b1bc66117d"), 50, true, "internal" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("c6153451-3af6-48be-8577-d2b1bc66117d"));

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("a39f5c7e-d5a2-43c9-86c8-cf584a4f8278"), 50, true, "internal" });
        }
    }
}
