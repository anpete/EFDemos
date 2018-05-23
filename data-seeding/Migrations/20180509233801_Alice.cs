using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demos.Migrations
{
    public partial class Alice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 1,
                column: "TitleColor",
                value: "AliceBlue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 1,
                column: "TitleColor",
                value: "Red");
        }
    }
}
