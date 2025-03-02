using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace srrf.Migrations
{
    /// <inheritdoc />
    public partial class addingISAnalyzedColumnAtAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnalyzed",
                table: "AuditLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnalyzed",
                table: "AuditLogs");
        }
    }
}
