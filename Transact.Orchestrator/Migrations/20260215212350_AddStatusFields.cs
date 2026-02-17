using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transact.Orchestrator.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OrchestratorTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrchestratorTransactions");
        }
    }
}
