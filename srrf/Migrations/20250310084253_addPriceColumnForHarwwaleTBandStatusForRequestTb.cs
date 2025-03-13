using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace srrf.Migrations
{
    /// <inheritdoc />
    public partial class addPriceColumnForHarwwaleTBandStatusForRequestTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "HardwareRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalPrice",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "HardwareRequests");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Hardware");
        }
    }
}
