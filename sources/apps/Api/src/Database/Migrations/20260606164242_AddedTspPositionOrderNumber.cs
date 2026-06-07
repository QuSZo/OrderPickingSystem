using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedTspPositionOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "TspPosition",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "TspPosition");
        }
    }
}
