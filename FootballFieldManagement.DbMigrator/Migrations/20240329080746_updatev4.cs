using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballFieldManagement.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class updatev4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldPrices_Times_TimeId",
                table: "FieldPrices");

            migrationBuilder.DropIndex(
                name: "IX_FieldPrices_TimeId",
                table: "FieldPrices");

            migrationBuilder.DropColumn(
                name: "TimeId",
                table: "FieldPrices");

            migrationBuilder.AddColumn<double>(
                name: "EndTime",
                table: "FieldPrices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartTime",
                table: "FieldPrices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "FieldPrices");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "FieldPrices");

            migrationBuilder.AddColumn<int>(
                name: "TimeId",
                table: "FieldPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FieldPrices_TimeId",
                table: "FieldPrices",
                column: "TimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldPrices_Times_TimeId",
                table: "FieldPrices",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
