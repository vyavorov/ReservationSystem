using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class Reviewtableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("9f000d1b-a931-4b91-acfe-c13aa969d28b"));

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("5ffa17db-904a-4e5c-aabc-a3402eff6200"), 50, true, "internal" });

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("5ffa17db-904a-4e5c-aabc-a3402eff6200"));

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "IsActive", "Name" },
                values: new object[] { new Guid("9f000d1b-a931-4b91-acfe-c13aa969d28b"), 50, true, "internal" });
        }
    }
}
