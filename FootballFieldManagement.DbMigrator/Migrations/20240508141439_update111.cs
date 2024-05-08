using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballFieldManagement.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class update111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "ProductBills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "PriceField",
                table: "Bills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceProduct",
                table: "Bills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceField",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PriceProduct",
                table: "Bills");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "ProductBills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
