using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuLivroDeReceitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeFileExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Recipies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Recipies");
        }
    }
}
