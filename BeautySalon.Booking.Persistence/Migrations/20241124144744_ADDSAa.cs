using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautySalon.Booking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ADDSAa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Processing" },
                    { 2, "Confirmed" },
                    { 3, "Canceled" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
