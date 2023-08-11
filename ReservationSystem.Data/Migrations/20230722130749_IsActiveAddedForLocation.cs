using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class IsActiveAddedForLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("b0329a6f-a9fa-4b9c-bdba-b8f7d2fd8ced"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: -1,
                column: "IsActive",
                value: true);

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("3a8a84c3-c8c7-4cd0-bd0a-454fc8b00013"), 50, "internal" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("3a8a84c3-c8c7-4cd0-bd0a-454fc8b00013"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Locations");

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("b0329a6f-a9fa-4b9c-bdba-b8f7d2fd8ced"), 50, "internal" });
        }
    }
}
