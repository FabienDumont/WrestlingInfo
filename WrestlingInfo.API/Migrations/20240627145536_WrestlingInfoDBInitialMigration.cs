using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WrestlingInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class WrestlingInfoDBInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wrestlers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wrestlers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WrestlingEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PromotionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WrestlingEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WrestlingEvents_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WrestlingEventReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    WrestlingEventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WrestlingEventReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WrestlingEventReviews_WrestlingEvents_WrestlingEventId",
                        column: x => x.WrestlingEventId,
                        principalTable: "WrestlingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "World Wrestling Entertainment (WWE) was founded in 1953 as Capitol Wrestling Corporation (CWC).", "WWE" },
                    { 2, "All Elite Wrestling (AEW) was founded in 2019.", "AEW" }
                });

            migrationBuilder.InsertData(
                table: "Wrestlers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "AJ Styles" },
                    { 2, "Akira Tozawa" },
                    { 3, "Akam" },
                    { 4, "Alba Fyre" },
                    { 5, "Andrade" }
                });

            migrationBuilder.InsertData(
                table: "WrestlingEvents",
                columns: new[] { "Id", "Date", "Name", "PromotionId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 1, 1), "Raw", 1 },
                    { 2, new DateOnly(2024, 1, 3), "Rampage", 2 },
                    { 3, new DateOnly(2024, 1, 5), "Smackdown", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WrestlingEventReviews_WrestlingEventId",
                table: "WrestlingEventReviews",
                column: "WrestlingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_WrestlingEvents_PromotionId",
                table: "WrestlingEvents",
                column: "PromotionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wrestlers");

            migrationBuilder.DropTable(
                name: "WrestlingEventReviews");

            migrationBuilder.DropTable(
                name: "WrestlingEvents");

            migrationBuilder.DropTable(
                name: "Promotions");
        }
    }
}
