using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recipes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CookingTimeMinutes", "CreatedAt", "Description", "ImageUrl", "Instructions", "Title" },
                values: new object[,]
                {
                    { 1, null, 45, new DateTime(2025, 12, 22, 15, 25, 4, 989, DateTimeKind.Utc).AddTicks(3666), "Clásico mexicano", "https://theeburgerdude.com/wp-content/uploads/2024/11/Al-Pastor-01-1024x1024.jpg", "Marinar la carne y asar.", "Tacos al Pastor" },
                    { 2, null, 20, new DateTime(2025, 12, 22, 15, 25, 4, 989, DateTimeKind.Utc).AddTicks(3697), "Receta original italiana", "https://www.informacibo.it/wp-content/uploads/2018/04/carbonara.jpg", "Mezclar huevo y queso.", "Pasta Carbonara" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Amount", "Name", "RecipeId", "Unit" },
                values: new object[,]
                {
                    { 1, 500.0, "Cerdo", 1, "gramos" },
                    { 2, 1.0, "Piña", 1, "pieza" },
                    { 3, 3.0, "Huevo", 2, "unidades" },
                    { 4, 50.0, "Pecorino", 2, "gramos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
