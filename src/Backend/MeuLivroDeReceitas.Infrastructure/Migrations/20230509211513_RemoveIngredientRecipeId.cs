﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuLivroDeReceitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIngredientRecipeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Ingredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
