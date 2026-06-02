using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class RobotPIDSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<double>>(
                name: "DerivativeHistory",
                table: "Orders",
                type: "double precision[]",
                nullable: true);

            migrationBuilder.AddColumn<List<double>>(
                name: "IntegralHistory",
                table: "Orders",
                type: "double precision[]",
                nullable: true);

            migrationBuilder.AddColumn<List<double>>(
                name: "PowerDifferenceHistory",
                table: "Orders",
                type: "double precision[]",
                nullable: true);

            migrationBuilder.AddColumn<List<double>>(
                name: "ProportionalHistory",
                table: "Orders",
                type: "double precision[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DerivativeHistory",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IntegralHistory",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PowerDifferenceHistory",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProportionalHistory",
                table: "Orders");
        }
    }
}
