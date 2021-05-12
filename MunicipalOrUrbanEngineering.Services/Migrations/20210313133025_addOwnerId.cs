using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalOrUrbanEngineering.Services.Migrations
{
    public partial class addOwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "BulletinBoardItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToOwnerId",
                table: "BulletinBoardItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BulletinBoardItems_OwnerId",
                table: "BulletinBoardItems",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BulletinBoardItems_Owners_OwnerId",
                table: "BulletinBoardItems",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BulletinBoardItems_Owners_OwnerId",
                table: "BulletinBoardItems");

            migrationBuilder.DropIndex(
                name: "IX_BulletinBoardItems_OwnerId",
                table: "BulletinBoardItems");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BulletinBoardItems");

            migrationBuilder.DropColumn(
                name: "ToOwnerId",
                table: "BulletinBoardItems");
        }
    }
}
