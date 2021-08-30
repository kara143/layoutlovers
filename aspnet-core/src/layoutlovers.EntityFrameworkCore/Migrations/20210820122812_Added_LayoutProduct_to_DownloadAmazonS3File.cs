using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace layoutlovers.Migrations
{
    public partial class Added_LayoutProduct_to_DownloadAmazonS3File : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LayoutProductId",
                table: "AppDownloadAmazonS3File",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "AppDownloadAmazonS3File",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppDownloadAmazonS3File_LayoutProductId",
                table: "AppDownloadAmazonS3File",
                column: "LayoutProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppDownloadAmazonS3File_AppLayoutProducts_LayoutProductId",
                table: "AppDownloadAmazonS3File",
                column: "LayoutProductId",
                principalTable: "AppLayoutProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppDownloadAmazonS3File_AppLayoutProducts_LayoutProductId",
                table: "AppDownloadAmazonS3File");

            migrationBuilder.DropIndex(
                name: "IX_AppDownloadAmazonS3File_LayoutProductId",
                table: "AppDownloadAmazonS3File");

            migrationBuilder.DropColumn(
                name: "LayoutProductId",
                table: "AppDownloadAmazonS3File");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AppDownloadAmazonS3File");
        }
    }
}
