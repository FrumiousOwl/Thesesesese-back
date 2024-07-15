using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace srrf.Migrations
{
    /// <inheritdoc />
    public partial class addingAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "ServiceRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Defective = table.Column<int>(type: "int", nullable: true),
                    Available = table.Column<int>(type: "int", nullable: true),
                    Deployed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_AssetId",
                table: "ServiceRequests",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Assets_AssetId",
                table: "ServiceRequests",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "AssetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Assets_AssetId",
                table: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_AssetId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "ServiceRequests");
        }
    }
}
