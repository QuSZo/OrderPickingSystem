using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedTspAlgorithmResultForOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TspAlgorithmResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalWeight = table.Column<double>(type: "double precision", nullable: false),
                    Distances = table.Column<List<double>>(type: "double precision[]", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TspAlgorithmResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TspAlgorithmResult_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TspPosition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false),
                    TspAlgorithmResultId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TspPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TspPosition_TspAlgorithmResult_TspAlgorithmResultId",
                        column: x => x.TspAlgorithmResultId,
                        principalTable: "TspAlgorithmResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TspAlgorithmResult_OrderId",
                table: "TspAlgorithmResult",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TspPosition_TspAlgorithmResultId",
                table: "TspPosition",
                column: "TspAlgorithmResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TspPosition");

            migrationBuilder.DropTable(
                name: "TspAlgorithmResult");
        }
    }
}
