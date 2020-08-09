using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketData.Adapter.Deribit.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    BlockTradeId = table.Column<string>(nullable: true),
                    Direction = table.Column<string>(nullable: true),
                    IndexPrice = table.Column<decimal>(nullable: false),
                    InstrumentName = table.Column<string>(nullable: true),
                    Volatility = table.Column<decimal>(nullable: true),
                    Liquidation = table.Column<string>(nullable: true),
                    MarkPrice = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    TickDirection = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    TradeId = table.Column<string>(nullable: true),
                    TradeSequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instruments");
        }
    }
}
