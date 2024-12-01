using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautySalon.Booking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class asd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId1",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_BookStatusId",
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
                name: "BookStatusId",
                table: "Booking");

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

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BookStatus",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "BookStatus",
                table: "Booking",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookStatus",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BookStatus",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BookStatus",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "BookStatusId",
                table: "Booking",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_Booking_BookStatusId",
                table: "Booking",
                column: "BookStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BookStatusId1",
                table: "Booking",
                column: "BookStatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId",
                table: "Booking",
                column: "BookStatusId",
                principalTable: "BookStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookStatus_BookStatusId1",
                table: "Booking",
                column: "BookStatusId1",
                principalTable: "BookStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
