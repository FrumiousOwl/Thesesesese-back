using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace srrf.Migrations
{
    /// <inheritdoc />
    public partial class whyItsNotWerking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "143d6429-fd8c-469b-aa56-bf0c24be0e28", "3b40fb08-571d-430a-bdcf-29a4783eeaf9", "Admin", "ADMIN" },
                    { "fedb896c-fa79-45df-b555-9cd412494890", "59236aa3-a261-488a-adb6-f70dc0dd5fe6", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "143d6429-fd8c-469b-aa56-bf0c24be0e28");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fedb896c-fa79-45df-b555-9cd412494890");
        }
    }
}
