using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalOrUrbanEngineering.Services.Migrations
{
    public partial class fixMetersData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MetersDatas_Flats_FlatId",
                table: "MetersDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_MetersDatas_ServiceTypes_ServiceTypeId",
                table: "MetersDatas");

            migrationBuilder.DropIndex(
                name: "IX_MetersDatas_FlatId",
                table: "MetersDatas");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "MetersDatas");

            migrationBuilder.RenameColumn(
                name: "ServiceTypeId",
                table: "MetersDatas",
                newName: "TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_MetersDatas_ServiceTypeId",
                table: "MetersDatas",
                newName: "IX_MetersDatas_TariffId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTariff",
                table: "Tariffs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTariff",
                table: "Tariffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_MetersDatas_Tariffs_TariffId",
                table: "MetersDatas",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MetersDatas_Tariffs_TariffId",
                table: "MetersDatas");

            migrationBuilder.DropColumn(
                name: "EndTariff",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "StartTariff",
                table: "Tariffs");

            migrationBuilder.RenameColumn(
                name: "TariffId",
                table: "MetersDatas",
                newName: "ServiceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_MetersDatas_TariffId",
                table: "MetersDatas",
                newName: "IX_MetersDatas_ServiceTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "FlatId",
                table: "MetersDatas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MetersDatas_FlatId",
                table: "MetersDatas",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_MetersDatas_Flats_FlatId",
                table: "MetersDatas",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MetersDatas_ServiceTypes_ServiceTypeId",
                table: "MetersDatas",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
