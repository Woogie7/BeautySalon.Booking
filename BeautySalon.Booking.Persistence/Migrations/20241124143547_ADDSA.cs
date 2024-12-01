using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautySalon.Booking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ADDSA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
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

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BookStatus",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking",
                column: "BookStatusId",
                principalTable: "BookStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BookStatus",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "BookStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Processing" },
                    { 2, "Confirmed" },
                    { 3, "Canceled" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking",
                column: "BookStatusId",
                principalTable: "BookStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
