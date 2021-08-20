using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace layoutlovers.Migrations
{
    public partial class Added_Purchase_Table_PaymentProvider_Property_Change_PurchaseItem_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPurchase_AbpUsers_UserId",
                table: "AppPurchase");

            migrationBuilder.DropForeignKey(
                name: "FK_AppPurchase_AppLayoutProducts_LayoutProductId",
                table: "AppPurchase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppPurchase",
                table: "AppPurchase");

            migrationBuilder.DropIndex(
                name: "IX_AppPurchase_LayoutProductId",
                table: "AppPurchase");

            migrationBuilder.DropIndex(
                name: "IX_AppPurchase_UserId",
                table: "AppPurchase");

            migrationBuilder.DropColumn(
                name: "LayoutProductId",
                table: "AppPurchase");

            migrationBuilder.RenameTable(
                name: "AppPurchase",
                newName: "AppPurchases");

            migrationBuilder.AddColumn<int>(
                name: "PaymentProvider",
                table: "AppPurchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "AppPurchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppPurchases",
                table: "AppPurchases",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppPurchaseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LayoutProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPurchaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPurchaseItems_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppPurchaseItems_AppLayoutProducts_LayoutProductId",
                        column: x => x.LayoutProductId,
                        principalTable: "AppLayoutProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppPurchaseItems_AppPurchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "AppPurchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseItems_LayoutProductId",
                table: "AppPurchaseItems",
                column: "LayoutProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseItems_PurchaseId",
                table: "AppPurchaseItems",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseItems_UserId",
                table: "AppPurchaseItems",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPurchaseItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppPurchases",
                table: "AppPurchases");

            migrationBuilder.DropColumn(
                name: "PaymentProvider",
                table: "AppPurchases");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "AppPurchases");

            migrationBuilder.RenameTable(
                name: "AppPurchases",
                newName: "AppPurchase");

            migrationBuilder.AddColumn<Guid>(
                name: "LayoutProductId",
                table: "AppPurchase",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppPurchase",
                table: "AppPurchase",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchase_LayoutProductId",
                table: "AppPurchase",
                column: "LayoutProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchase_UserId",
                table: "AppPurchase",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPurchase_AbpUsers_UserId",
                table: "AppPurchase",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppPurchase_AppLayoutProducts_LayoutProductId",
                table: "AppPurchase",
                column: "LayoutProductId",
                principalTable: "AppLayoutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
