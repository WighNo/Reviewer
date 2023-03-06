using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reviewer.Migrations
{
    /// <inheritdoc />
    public partial class EditComapnies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyCategories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCategories_CompanyId",
                table: "CompanyCategories",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCategories_Companies_CompanyId",
                table: "CompanyCategories",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCategories_Companies_CompanyId",
                table: "CompanyCategories");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCategories_CompanyId",
                table: "CompanyCategories");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyCategories");
        }
    }
}
