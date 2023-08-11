using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class Reviewsrelationfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("5ffa17db-904a-4e5c-aabc-a3402eff6200"));

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("5fced3d9-5c58-4951-929a-a95241d6bdb1"), 50, true, "internal" });

            migrationBuilder.CreateIndex(
                name: "IX_Review_LocationId",
                table: "Review",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Locations_LocationId",
                table: "Review",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Locations_LocationId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_LocationId",
                table: "Review");

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("5fced3d9-5c58-4951-929a-a95241d6bdb1"));

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Review");

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("5ffa17db-904a-4e5c-aabc-a3402eff6200"), 50, true, "internal" });
        }
    }
}
