using Microsoft.EntityFrameworkCore.Migrations;

namespace Stonks.API.Migrations
{
    public partial class UpdateTimeSeriesProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSeries_Stocks_StockSymbol",
                table: "TimeSeries");

            migrationBuilder.DropIndex(
                name: "IX_TimeSeries_StockSymbol",
                table: "TimeSeries");

            migrationBuilder.DropColumn(
                name: "Interval",
                table: "TimeSeries");

            migrationBuilder.DropColumn(
                name: "StockSymbol",
                table: "TimeSeries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Interval",
                table: "TimeSeries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StockSymbol",
                table: "TimeSeries",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSeries_StockSymbol",
                table: "TimeSeries",
                column: "StockSymbol");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSeries_Stocks_StockSymbol",
                table: "TimeSeries",
                column: "StockSymbol",
                principalTable: "Stocks",
                principalColumn: "Symbol",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
