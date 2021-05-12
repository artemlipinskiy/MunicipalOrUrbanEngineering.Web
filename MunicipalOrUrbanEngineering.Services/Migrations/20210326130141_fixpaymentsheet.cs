using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalOrUrbanEngineering.Services.Migrations
{
    public partial class fixpaymentsheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSheets_Statuses_StatusId",
                table: "PaymentSheets");

            migrationBuilder.AlterColumn<Guid>(
                name: "StatusId",
                table: "PaymentSheets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSheets_Statuses_StatusId",
                table: "PaymentSheets",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSheets_Statuses_StatusId",
                table: "PaymentSheets");

            migrationBuilder.AlterColumn<Guid>(
                name: "StatusId",
                table: "PaymentSheets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSheets_Statuses_StatusId",
                table: "PaymentSheets",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
