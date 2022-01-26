using Microsoft.EntityFrameworkCore.Migrations;

namespace layoutlovers.Migrations
{
    public partial class Added_Properties_to_LayoutProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlternativeGridType",
                table: "AppLayoutProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "FeaturedOrder",
                table: "AppLayoutProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "AppLayoutProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlternativeGridType",
                table: "AppLayoutProducts");

            migrationBuilder.DropColumn(
                name: "FeaturedOrder",
                table: "AppLayoutProducts");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "AppLayoutProducts");
        }
    }
}
