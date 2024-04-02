using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballFieldManagement.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class updatev3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldTypeId",
                table: "FieldPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeId",
                table: "FieldPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FieldPrices_FieldTypeId",
                table: "FieldPrices",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldPrices_TimeId",
                table: "FieldPrices",
                column: "TimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldPrices_FieldTypes_FieldTypeId",
                table: "FieldPrices",
                column: "FieldTypeId",
                principalTable: "FieldTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldPrices_Times_TimeId",
                table: "FieldPrices",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldPrices_FieldTypes_FieldTypeId",
                table: "FieldPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldPrices_Times_TimeId",
                table: "FieldPrices");

            migrationBuilder.DropIndex(
                name: "IX_FieldPrices_FieldTypeId",
                table: "FieldPrices");

            migrationBuilder.DropIndex(
                name: "IX_FieldPrices_TimeId",
                table: "FieldPrices");

            migrationBuilder.DropColumn(
                name: "FieldTypeId",
                table: "FieldPrices");

            migrationBuilder.DropColumn(
                name: "TimeId",
                table: "FieldPrices");
        }
    }
}
