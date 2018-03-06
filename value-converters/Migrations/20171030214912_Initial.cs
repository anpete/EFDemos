// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demos.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    ThemeId = table.Column<uint>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    TitleColor = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Themes", x => x.ThemeId); });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlogUrl = table.Column<string>(nullable: true),
                    ThemeId = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_Blogs", x => x.BlogId);
                        table.ForeignKey(
                            name: "FK_Blogs_Themes_ThemeId",
                            column: x => x.ThemeId,
                            principalTable: "Themes",
                            principalColumn: "ThemeId",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "ThemeId", "Name", "TitleColor" },
                values: new object[] { 1u, "MSDN", "Red" });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "ThemeId", "Name", "TitleColor" },
                values: new object[] { 2u, "TechNet", "Red" });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "ThemeId", "Name", "TitleColor" },
                values: new object[] { 3u, "Personal", "LightBlue" });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ThemeId",
                table: "Blogs",
                column: "ThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Themes");
        }
    }
}
