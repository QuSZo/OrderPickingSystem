using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class RobotPIDValueMeans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DerivativeAbsoluteMean",
                table: "Orders",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IntegralAbsoluteMean",
                table: "Orders",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PowerDifferenceAbsoluteMean",
                table: "Orders",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProportionalAbsoluteMean",
                table: "Orders",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DerivativeAbsoluteMean",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IntegralAbsoluteMean",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PowerDifferenceAbsoluteMean",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProportionalAbsoluteMean",
                table: "Orders");
        }
    }
}
