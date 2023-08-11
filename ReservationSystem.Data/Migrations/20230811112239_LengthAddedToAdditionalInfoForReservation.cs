using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class LengthAddedToAdditionalInfoForReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("c6153451-3af6-48be-8577-d2b1bc66117d"));

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalInformation",
                table: "Reservations",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("6e195895-ca57-4bcc-88d4-90ddb3b112ca"), 50, true, "internal" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("6e195895-ca57-4bcc-88d4-90ddb3b112ca"));

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalInformation",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("c6153451-3af6-48be-8577-d2b1bc66117d"), 50, true, "internal" });
        }
    }
}
