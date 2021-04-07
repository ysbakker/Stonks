using Microsoft.EntityFrameworkCore.Migrations;

namespace Stonks.API.Migrations
{
    public partial class UpdateQuoteKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes",
                columns: new[] { "Symbol", "LatestTradingDay" });
        }
    }
}
