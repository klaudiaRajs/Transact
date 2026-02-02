using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transact.Core.Transactions.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTransactionToKeepSerializedContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "IX_Transactions_OwnerId",
                table: "Transactions");
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductsList",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserSnapshot",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsList",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UserSnapshot",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Product_TransactionId",
                table: "Product",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_User_OwnerId",
                table: "Transactions",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
