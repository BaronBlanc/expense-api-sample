using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpenseApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name", "Symbol" },
                values: new object[,]
                {
                    { new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"), "USD", "U.S. Dollar", "$" },
                    { new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"), "RUB", "Russian Ruble", "₽" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CurrencyId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("b5c5e15b-e1c3-4d35-beef-7bf0994b38a4"), new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"), "Natasha", "Romanova" },
                    { new Guid("c0141cb5-03fe-43c2-9cfa-96b40d77f21b"), new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"), "Anthony", "Stark" }
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Comment", "CurrencyId", "Date", "Type", "UserId" },
                values: new object[,]
                {
                    { new Guid("15dd0dfd-728c-476c-89a6-2bc9bccbe75c"), 113.81893995949065, "Hotel stay", new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"), new DateTime(2024, 12, 7, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(4391), 0, new Guid("b5c5e15b-e1c3-4d35-beef-7bf0994b38a4") },
                    { new Guid("2d2f39ab-2523-4ae9-a911-89170f672939"), 103.68933144408696, "Business meeting", new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"), new DateTime(2024, 12, 11, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(3440), 2, new Guid("c0141cb5-03fe-43c2-9cfa-96b40d77f21b") },
                    { new Guid("bcda063a-f719-44f9-8066-dd2827b3ec51"), 140.62828957261399, "Client dinner", new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"), new DateTime(2024, 12, 11, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(4387), 0, new Guid("c0141cb5-03fe-43c2-9cfa-96b40d77f21b") },
                    { new Guid("f6cda131-c002-422c-906d-aac7f3c0dc26"), 118.39945875162243, "Travel expense", new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"), new DateTime(2024, 11, 17, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(4376), 1, new Guid("b5c5e15b-e1c3-4d35-beef-7bf0994b38a4") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CurrencyId",
                table: "Expenses",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrencyId",
                table: "Users",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
