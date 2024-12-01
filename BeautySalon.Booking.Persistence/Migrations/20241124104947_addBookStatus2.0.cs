using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautySalon.Booking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addBookStatus20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BookStatus",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "BookStatusId1",
                table: "Booking",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "BookStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Processing" },
                    { 2, "Confirmed" },
                    { 3, "Canceled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BookStatusId1",
                table: "Booking",
                column: "BookStatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking",
                column: "BookStatusId",
                principalTable: "BookStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId1",
                table: "Booking",
                column: "BookStatusId1",
                principalTable: "BookStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId1",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_BookStatusId1",
                table: "Booking");

            migrationBuilder.DeleteData(
                table: "BookStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "BookStatusId1",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BookStatus",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking",
                column: "BookStatusId",
                principalTable: "BookStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
