using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xekoshop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedQttyAndTotalCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleCount",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Cart",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleCount",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Cart");
        }
    }
}
