using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class DescriptionAddedForLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("b64ea8b2-1dd4-4d05-922e-c04b8bc3f777"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Locations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "Capacity", "Description", "ImageUrl", "Name", "PricePerDay" },
                values: new object[] { -1, "Perla Beach, Primorsko", 40, "Bas#Hub Perla is a first of a kind coworking space in Bulgaria. Right between the fresh forest and beautiful beach, we've placed over 30 fixed desks, which are waiting for you", "https://bashhub.bg/wp-content/uploads/2021/10/DSCF9982.png", "Perla", 35m });

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("b0329a6f-a9fa-4b9c-bdba-b8f7d2fd8ced"), 50, "internal" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("b0329a6f-a9fa-4b9c-bdba-b8f7d2fd8ced"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Locations");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "Capacity", "ImageUrl", "Name", "PricePerDay" },
                values: new object[] { 1, "Perla Beach, Primorsko", 40, "https://bashhub.bg/wp-content/uploads/2021/10/DSCF9982.png", "Perla", 35m });

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("b64ea8b2-1dd4-4d05-922e-c04b8bc3f777"), 50, "internal" });
        }
    }
}
