using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSdigital_1.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderBookEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<string>(type: "TEXT", nullable: false),
                    OrderBookId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrderBookId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBookEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderBookEntry_OrderBooks_OrderBookId",
                        column: x => x.OrderBookId,
                        principalTable: "OrderBooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderBookEntry_OrderBooks_OrderBookId1",
                        column: x => x.OrderBookId1,
                        principalTable: "OrderBooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderBookEntry_OrderBookId",
                table: "OrderBookEntry",
                column: "OrderBookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBookEntry_OrderBookId1",
                table: "OrderBookEntry",
                column: "OrderBookId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderBookEntry");

            migrationBuilder.DropTable(
                name: "OrderBooks");
        }
    }
}
