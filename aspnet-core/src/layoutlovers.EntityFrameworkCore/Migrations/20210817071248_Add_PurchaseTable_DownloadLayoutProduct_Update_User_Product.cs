using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace layoutlovers.Migrations
{
    public partial class Add_PurchaseTable_DownloadLayoutProduct_Update_User_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAmazonS3File_AppProducts_ProductId",
                table: "AppAmazonS3File");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFavorites_AppProducts_ProductId",
                table: "AppFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductFilterTags_AppProducts_ProductId",
                table: "AppProductFilterTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AppShoppingCarts_AppProducts_ProductId",
                table: "AppShoppingCarts");

            migrationBuilder.DropTable(
                name: "AppProducts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AppShoppingCarts",
                newName: "LayoutProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppShoppingCarts_ProductId",
                table: "AppShoppingCarts",
                newName: "IX_AppShoppingCarts_LayoutProductId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AppProductFilterTags",
                newName: "LayoutProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppProductFilterTags_ProductId",
                table: "AppProductFilterTags",
                newName: "IX_AppProductFilterTags_LayoutProductId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AppFavorites",
                newName: "LayoutProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppFavorites_ProductId",
                table: "AppFavorites",
                newName: "IX_AppFavorites_LayoutProductId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AppAmazonS3File",
                newName: "LayoutProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAmazonS3File_ProductId",
                table: "AppAmazonS3File",
                newName: "IX_AppAmazonS3File_LayoutProductId");

            migrationBuilder.AddColumn<int>(
                name: "DownloadToday",
                table: "AbpUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppDownloadAmazonS3File",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AmazonS3FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDownloadAmazonS3File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDownloadAmazonS3File_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppDownloadAmazonS3File_AppAmazonS3File_AmazonS3FileId",
                        column: x => x.AmazonS3FileId,
                        principalTable: "AppAmazonS3File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppDownloadRestrictions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DownloadPerDay = table.Column<int>(type: "int", nullable: false),
                    LayoutProductType = table.Column<int>(type: "int", nullable: false),
                    SubscribableEditionId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDownloadRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDownloadRestrictions_AbpEditions_SubscribableEditionId",
                        column: x => x.SubscribableEditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppLayoutProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LayoutProductType = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLayoutProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLayoutProducts_AppCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPurchase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestPayerStatus = table.Column<int>(type: "int", nullable: false),
                    ChargeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailureMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_AppPurchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPurchase_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppPurchase_AppLayoutProducts_LayoutProductId",
                        column: x => x.LayoutProductId,
                        principalTable: "AppLayoutProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDownloadAmazonS3File_AmazonS3FileId",
                table: "AppDownloadAmazonS3File",
                column: "AmazonS3FileId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDownloadAmazonS3File_UserId",
                table: "AppDownloadAmazonS3File",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDownloadRestrictions_SubscribableEditionId",
                table: "AppDownloadRestrictions",
                column: "SubscribableEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLayoutProducts_CategoryId",
                table: "AppLayoutProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchase_LayoutProductId",
                table: "AppPurchase",
                column: "LayoutProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchase_UserId",
                table: "AppPurchase",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAmazonS3File_AppLayoutProducts_LayoutProductId",
                table: "AppAmazonS3File",
                column: "LayoutProductId",
                principalTable: "AppLayoutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFavorites_AppLayoutProducts_LayoutProductId",
                table: "AppFavorites",
                column: "LayoutProductId",
                principalTable: "AppLayoutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductFilterTags_AppLayoutProducts_LayoutProductId",
                table: "AppProductFilterTags",
                column: "LayoutProductId",
                principalTable: "AppLayoutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppShoppingCarts_AppLayoutProducts_LayoutProductId",
                table: "AppShoppingCarts",
                column: "LayoutProductId",
                principalTable: "AppLayoutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAmazonS3File_AppLayoutProducts_LayoutProductId",
                table: "AppAmazonS3File");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFavorites_AppLayoutProducts_LayoutProductId",
                table: "AppFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductFilterTags_AppLayoutProducts_LayoutProductId",
                table: "AppProductFilterTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AppShoppingCarts_AppLayoutProducts_LayoutProductId",
                table: "AppShoppingCarts");

            migrationBuilder.DropTable(
                name: "AppDownloadAmazonS3File");

            migrationBuilder.DropTable(
                name: "AppDownloadRestrictions");

            migrationBuilder.DropTable(
                name: "AppPurchase");

            migrationBuilder.DropTable(
                name: "AppLayoutProducts");

            migrationBuilder.DropColumn(
                name: "DownloadToday",
                table: "AbpUsers");

            migrationBuilder.RenameColumn(
                name: "LayoutProductId",
                table: "AppShoppingCarts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppShoppingCarts_LayoutProductId",
                table: "AppShoppingCarts",
                newName: "IX_AppShoppingCarts_ProductId");

            migrationBuilder.RenameColumn(
                name: "LayoutProductId",
                table: "AppProductFilterTags",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppProductFilterTags_LayoutProductId",
                table: "AppProductFilterTags",
                newName: "IX_AppProductFilterTags_ProductId");

            migrationBuilder.RenameColumn(
                name: "LayoutProductId",
                table: "AppFavorites",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppFavorites_LayoutProductId",
                table: "AppFavorites",
                newName: "IX_AppFavorites_ProductId");

            migrationBuilder.RenameColumn(
                name: "LayoutProductId",
                table: "AppAmazonS3File",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAmazonS3File_LayoutProductId",
                table: "AppAmazonS3File",
                newName: "IX_AppAmazonS3File_ProductId");

            migrationBuilder.CreateTable(
                name: "AppProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProducts_AppCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProducts_CategoryId",
                table: "AppProducts",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAmazonS3File_AppProducts_ProductId",
                table: "AppAmazonS3File",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFavorites_AppProducts_ProductId",
                table: "AppFavorites",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductFilterTags_AppProducts_ProductId",
                table: "AppProductFilterTags",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppShoppingCarts_AppProducts_ProductId",
                table: "AppShoppingCarts",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
