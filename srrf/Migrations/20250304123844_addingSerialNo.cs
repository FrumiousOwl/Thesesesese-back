using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace srrf.Migrations
{
    /// <inheritdoc />
    public partial class addingSerialNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNo",
                table: "HardwareRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNo",
                table: "HardwareRequests");
        }
    }
}
