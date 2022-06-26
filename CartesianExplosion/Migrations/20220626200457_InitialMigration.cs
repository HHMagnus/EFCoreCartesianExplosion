using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CartesianExplosion.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Summaries",
                columns: table => new
                {
                    SummaryId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summaries", x => x.SummaryId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    Load1 = table.Column<string>(nullable: false),
                    Load2 = table.Column<string>(nullable: false),
                    Load3 = table.Column<string>(nullable: false),
                    Load4 = table.Column<string>(nullable: false),
                    Load5 = table.Column<string>(nullable: false),
                    Load6 = table.Column<string>(nullable: false),
                    Load7 = table.Column<string>(nullable: false),
                    Load8 = table.Column<string>(nullable: false),
                    Load9 = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "Payins",
                columns: table => new
                {
                    PayinId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AmountGoingIn = table.Column<decimal>(nullable: false),
                    BalanceAfter = table.Column<decimal>(nullable: false),
                    BalanceBefore = table.Column<decimal>(nullable: false),
                    PayinDate = table.Column<DateTime>(nullable: false),
                    TransactionId = table.Column<long>(nullable: true),
                    SummaryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payins", x => x.PayinId);
                    table.ForeignKey(
                        name: "FK_Payins_Summaries_SummaryId",
                        column: x => x.SummaryId,
                        principalTable: "Summaries",
                        principalColumn: "SummaryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payins_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payouts",
                columns: table => new
                {
                    PayoutId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AmountGoingOut = table.Column<decimal>(nullable: false),
                    BalanceAfter = table.Column<decimal>(nullable: false),
                    BalanceBefore = table.Column<decimal>(nullable: false),
                    PayoutDate = table.Column<DateTime>(nullable: false),
                    TransactionId = table.Column<long>(nullable: true),
                    SummaryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payouts", x => x.PayoutId);
                    table.ForeignKey(
                        name: "FK_Payouts_Summaries_SummaryId",
                        column: x => x.SummaryId,
                        principalTable: "Summaries",
                        principalColumn: "SummaryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payouts_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payins_SummaryId",
                table: "Payins",
                column: "SummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_TransactionId",
                table: "Payins",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_SummaryId",
                table: "Payouts",
                column: "SummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_TransactionId",
                table: "Payouts",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payins");

            migrationBuilder.DropTable(
                name: "Payouts");

            migrationBuilder.DropTable(
                name: "Summaries");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
