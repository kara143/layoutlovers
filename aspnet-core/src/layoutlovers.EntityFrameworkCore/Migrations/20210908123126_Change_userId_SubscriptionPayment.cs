using Microsoft.EntityFrameworkCore.Migrations;

namespace layoutlovers.Migrations
{
    public partial class Change_userId_SubscriptionPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "AppSubscriptionPayments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AppSubscriptionPayments");
        }
    }
}
