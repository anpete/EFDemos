using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demos.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "ThemeId", "Name", "TitleColor" },
                values: new object[,]
                {
                    { 1, "MSDN", "Red" },
                    { 2, "TechNet", "DarkCyan" },
                    { 3, "EF", "Purple" },
                    { 4, "Personal", "LightBlue" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 4);
        }
    }
}
