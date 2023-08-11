using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem.Data.Migrations
{
    public partial class Equipmentrelationsfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentsReservations",
                table: "EquipmentsReservations");

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("eb0afcc1-7ba1-48cf-bab6-c26a67e16f29"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "EquipmentsReservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentsReservations",
                table: "EquipmentsReservations",
                column: "Id");

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("ff4b4f44-4cd7-4864-8753-e0b859e2dab4"), 50, "internal" });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentsReservations_EquipmentId",
                table: "EquipmentsReservations",
                column: "EquipmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentsReservations",
                table: "EquipmentsReservations");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentsReservations_EquipmentId",
                table: "EquipmentsReservations");

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: new Guid("ff4b4f44-4cd7-4864-8753-e0b859e2dab4"));

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EquipmentsReservations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentsReservations",
                table: "EquipmentsReservations",
                columns: new[] { "EquipmentId", "ReservationId" });

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("eb0afcc1-7ba1-48cf-bab6-c26a67e16f29"), 50, "internal" });
        }
    }
}
