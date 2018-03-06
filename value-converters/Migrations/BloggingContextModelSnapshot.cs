// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Demos.Migrations
{
    [DbContext(typeof(BloggingContext))]
    internal class BloggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-preview1-27461")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity(
                "Demos.BloggingContext+Blog", b =>
                    {
                        b.Property<int>("BlogId")
                            .ValueGeneratedOnAdd();

                        b.Property<string>("BlogUrl");

                        b.Property<uint?>("ThemeId");

                        b.HasKey("BlogId");

                        b.HasIndex("ThemeId");

                        b.ToTable("Blogs");
                    });

            modelBuilder.Entity(
                "Demos.BloggingContext+Theme", b =>
                    {
                        b.Property<uint>("ThemeId")
                            .ValueGeneratedOnAdd();

                        b.Property<string>("Name");

                        b.Property<string>("TitleColor");

                        b.HasKey("ThemeId");

                        b.ToTable("Themes");

                        b.SeedData(new { ThemeId = 1u, Name = "MSDN", TitleColor = "Red" }, new { ThemeId = 2u, Name = "TechNet", TitleColor = "Red" }, new { ThemeId = 3u, Name = "Personal", TitleColor = "LightBlue" });
                    });

            modelBuilder.Entity(
                "Demos.BloggingContext+Blog", b =>
                    {
                        b.HasOne("Demos.BloggingContext+Theme", "Theme")
                            .WithMany()
                            .HasForeignKey("ThemeId");
                    });
#pragma warning restore 612, 618
        }
    }
}
