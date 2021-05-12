using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalOrUrbanEngineering.Services.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "RequestTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RequestTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Roles",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Roles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ServiceTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UnitName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsCounterReadings = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ServiceTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Statuses",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Statuses", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Streets",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Streets", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AppUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AppUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AppUsers_Roles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "Roles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PaymentPeriods",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PaymentPeriods", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PaymentPeriods_Statuses_StatusId",
            //            column: x => x.StatusId,
            //            principalTable: "Statuses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Buildings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StreetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Buildings", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Buildings_Streets_StreetId",
            //            column: x => x.StreetId,
            //            principalTable: "Streets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Employees",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Employees", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Employees_AppUsers_AppUserId",
            //            column: x => x.AppUserId,
            //            principalTable: "AppUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Owners",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Owners", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Owners_AppUsers_AppUserId",
            //            column: x => x.AppUserId,
            //            principalTable: "AppUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Flats",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ApartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Flats", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Flats_Buildings_BuildingId",
            //            column: x => x.BuildingId,
            //            principalTable: "Buildings",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Flats_Owners_OwnerId",
            //            column: x => x.OwnerId,
            //            principalTable: "Owners",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Histories",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Entity = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Histories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Histories_Employees_EmployeeId",
            //            column: x => x.EmployeeId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Histories_Owners_OwnerId",
            //            column: x => x.OwnerId,
            //            principalTable: "Owners",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ServiceRequests",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        RequestorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ResponserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ServiceRequests", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ServiceRequests_Employees_ResponserId",
            //            column: x => x.ResponserId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ServiceRequests_Owners_RequestorId",
            //            column: x => x.RequestorId,
            //            principalTable: "Owners",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ServiceRequests_RequestTypes_RequestTypeId",
            //            column: x => x.RequestTypeId,
            //            principalTable: "RequestTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ServiceRequests_Statuses_StatusId",
            //            column: x => x.StatusId,
            //            principalTable: "Statuses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BulletinBoardItems",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ToStreetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ToBuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ToFlatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ToAllUsers = table.Column<bool>(type: "bit", nullable: false),
            //        ShowStart = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ShowEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BulletinBoardItems", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_BulletinBoardItems_Buildings_ToBuildingId",
            //            column: x => x.ToBuildingId,
            //            principalTable: "Buildings",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_BulletinBoardItems_Employees_CreatorId",
            //            column: x => x.CreatorId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_BulletinBoardItems_Flats_ToFlatId",
            //            column: x => x.ToFlatId,
            //            principalTable: "Flats",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_BulletinBoardItems_Streets_ToStreetId",
            //            column: x => x.ToStreetId,
            //            principalTable: "Streets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MetersDatas",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        PaymentPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FlatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MetersDatas", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MetersDatas_Flats_FlatId",
            //            column: x => x.FlatId,
            //            principalTable: "Flats",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MetersDatas_PaymentPeriods_PaymentPeriodId",
            //            column: x => x.PaymentPeriodId,
            //            principalTable: "PaymentPeriods",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MetersDatas_ServiceTypes_ServiceTypeId",
            //            column: x => x.ServiceTypeId,
            //            principalTable: "ServiceTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PaymentSheets",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PaymentPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FlatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PaymentSheets", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PaymentSheets_Flats_FlatId",
            //            column: x => x.FlatId,
            //            principalTable: "Flats",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_PaymentSheets_PaymentPeriods_PaymentPeriodId",
            //            column: x => x.PaymentPeriodId,
            //            principalTable: "PaymentPeriods",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_PaymentSheets_Statuses_StatusId",
            //            column: x => x.StatusId,
            //            principalTable: "Statuses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tariffs",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        ServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FlatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tariffs", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Tariffs_Flats_FlatId",
            //            column: x => x.FlatId,
            //            principalTable: "Flats",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Tariffs_ServiceTypes_ServiceTypeId",
            //            column: x => x.ServiceTypeId,
            //            principalTable: "ServiceTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ServiceResponses",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ResponserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ServiceResponses", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ServiceResponses_Employees_ResponserId",
            //            column: x => x.ResponserId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ServiceResponses_ServiceRequests_ServiceRequestId",
            //            column: x => x.ServiceRequestId,
            //            principalTable: "ServiceRequests",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ServiceBills",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        TariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PaymentSheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ServiceBills", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ServiceBills_PaymentSheets_PaymentSheetId",
            //            column: x => x.PaymentSheetId,
            //            principalTable: "PaymentSheets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ServiceBills_Statuses_StatusId",
            //            column: x => x.StatusId,
            //            principalTable: "Statuses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ServiceBills_Tariffs_TariffId",
            //            column: x => x.TariffId,
            //            principalTable: "Tariffs",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppUsers_RoleId",
            //    table: "AppUsers",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Buildings_StreetId",
            //    table: "Buildings",
            //    column: "StreetId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BulletinBoardItems_CreatorId",
            //    table: "BulletinBoardItems",
            //    column: "CreatorId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BulletinBoardItems_ToBuildingId",
            //    table: "BulletinBoardItems",
            //    column: "ToBuildingId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BulletinBoardItems_ToFlatId",
            //    table: "BulletinBoardItems",
            //    column: "ToFlatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BulletinBoardItems_ToStreetId",
            //    table: "BulletinBoardItems",
            //    column: "ToStreetId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employees_AppUserId",
            //    table: "Employees",
            //    column: "AppUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Flats_BuildingId",
            //    table: "Flats",
            //    column: "BuildingId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Flats_OwnerId",
            //    table: "Flats",
            //    column: "OwnerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Histories_EmployeeId",
            //    table: "Histories",
            //    column: "EmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Histories_OwnerId",
            //    table: "Histories",
            //    column: "OwnerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MetersDatas_FlatId",
            //    table: "MetersDatas",
            //    column: "FlatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MetersDatas_PaymentPeriodId",
            //    table: "MetersDatas",
            //    column: "PaymentPeriodId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MetersDatas_ServiceTypeId",
            //    table: "MetersDatas",
            //    column: "ServiceTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Owners_AppUserId",
            //    table: "Owners",
            //    column: "AppUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentPeriods_StatusId",
            //    table: "PaymentPeriods",
            //    column: "StatusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentSheets_FlatId",
            //    table: "PaymentSheets",
            //    column: "FlatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentSheets_PaymentPeriodId",
            //    table: "PaymentSheets",
            //    column: "PaymentPeriodId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentSheets_StatusId",
            //    table: "PaymentSheets",
            //    column: "StatusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceBills_PaymentSheetId",
            //    table: "ServiceBills",
            //    column: "PaymentSheetId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceBills_StatusId",
            //    table: "ServiceBills",
            //    column: "StatusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceBills_TariffId",
            //    table: "ServiceBills",
            //    column: "TariffId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceRequests_RequestorId",
            //    table: "ServiceRequests",
            //    column: "RequestorId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceRequests_RequestTypeId",
            //    table: "ServiceRequests",
            //    column: "RequestTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceRequests_ResponserId",
            //    table: "ServiceRequests",
            //    column: "ResponserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceRequests_StatusId",
            //    table: "ServiceRequests",
            //    column: "StatusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceResponses_ResponserId",
            //    table: "ServiceResponses",
            //    column: "ResponserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServiceResponses_ServiceRequestId",
            //    table: "ServiceResponses",
            //    column: "ServiceRequestId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tariffs_FlatId",
            //    table: "Tariffs",
            //    column: "FlatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tariffs_ServiceTypeId",
            //    table: "Tariffs",
            //    column: "ServiceTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulletinBoardItems");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "MetersDatas");

            migrationBuilder.DropTable(
                name: "ServiceBills");

            migrationBuilder.DropTable(
                name: "ServiceResponses");

            migrationBuilder.DropTable(
                name: "PaymentSheets");

            migrationBuilder.DropTable(
                name: "Tariffs");

            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "PaymentPeriods");

            migrationBuilder.DropTable(
                name: "Flats");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Streets");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
