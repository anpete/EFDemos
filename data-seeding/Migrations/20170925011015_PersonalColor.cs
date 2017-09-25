using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace dataseeding.Migrations
{
    public partial class PersonalColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "ThemeId", "Name", "TitleColor" },
                values: new object[] { 3, "Personal", -5383962 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 3);
        }
    }
}
