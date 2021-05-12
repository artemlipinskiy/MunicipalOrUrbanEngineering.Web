﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MunicipalOrUrbanEngineering.Services;

namespace MunicipalOrUrbanEngineering.Services.Migrations
{
    [DbContext(typeof(MUEContext))]
    [Migration("20210325150127_addDefaultData")]
    partial class addDefaultData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HashPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Building", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StreetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StreetId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.BulletinBoardItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ShowEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ShowStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ToAllUsers")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ToBuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ToFlatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ToOwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ToStreetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ToBuildingId");

                    b.HasIndex("ToFlatId");

                    b.HasIndex("ToOwnerId");

                    b.HasIndex("ToStreetId");

                    b.ToTable("BulletinBoardItems");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Flat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApartmentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Flats");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.History", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Entity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.MetersData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PaymentPeriodId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TariffId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PaymentPeriodId");

                    b.HasIndex("TariffId");

                    b.ToTable("MetersDatas");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Owner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.PaymentPeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("PaymentPeriods");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.PaymentSheet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FlatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PaymentPeriodId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.HasIndex("PaymentPeriodId");

                    b.HasIndex("StatusId");

                    b.ToTable("PaymentSheets");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.RequestType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RequestTypes");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.ServiceBill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PaymentSheetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TariffId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PaymentSheetId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TariffId");

                    b.ToTable("ServiceBills");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.ServiceRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RequestorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ResponserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RequestTypeId");

                    b.HasIndex("RequestorId");

                    b.HasIndex("ResponserId");

                    b.HasIndex("StatusId");

                    b.ToTable("ServiceRequests");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.ServiceResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ResponserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceRequestId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ResponserId");

                    b.HasIndex("ServiceRequestId");

                    b.ToTable("ServiceResponses");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.ServiceType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCounterReadings")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ServiceTypes");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Status", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Street", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Streets");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Tariff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DefaultData")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("EndTariff")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FlatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ServiceTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTariff")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.HasIndex("ServiceTypeId");

                    b.ToTable("Tariffs");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.AppUser", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Role", "Role")
                        .WithMany("AppUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Building", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Street", "Street")
                        .WithMany("Buildings")
                        .HasForeignKey("StreetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Street");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.BulletinBoardItem", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Employee", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Building", "ToBuilding")
                        .WithMany()
                        .HasForeignKey("ToBuildingId");

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Flat", "ToFlat")
                        .WithMany()
                        .HasForeignKey("ToFlatId");

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Owner", "ToOwner")
                        .WithMany()
                        .HasForeignKey("ToOwnerId");

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Street", "ToStreet")
                        .WithMany()
                        .HasForeignKey("ToStreetId");

                    b.Navigation("Creator");

                    b.Navigation("ToBuilding");

                    b.Navigation("ToFlat");

                    b.Navigation("ToOwner");

                    b.Navigation("ToStreet");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Employee", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Flat", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Building", "Building")
                        .WithMany("Flats")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Owner", "Owner")
                        .WithMany("Flats")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Building");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.History", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Employee");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.MetersData", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.PaymentPeriod", "PaymentPeriod")
                        .WithMany()
                        .HasForeignKey("PaymentPeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Tariff", "Tariff")
                        .WithMany()
                        .HasForeignKey("TariffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentPeriod");

                    b.Navigation("Tariff");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Owner", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.PaymentPeriod", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.PaymentSheet", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Flat", "Flat")
                        .WithMany()
                        .HasForeignKey("FlatId");

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.PaymentPeriod", "PaymentPeriod")
                        .WithMany()
                        .HasForeignKey("PaymentPeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.Navigation("Flat");

                    b.Navigation("PaymentPeriod");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.ServiceBill", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.PaymentSheet", "PaymentSheet")
                        .WithMany("ServiceBills")
                        .HasForeignKey("PaymentSheetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Tariff", "Tariff")
                        .WithMany()
                        .HasForeignKey("TariffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentSheet");

                    b.Navigation("Status");

                    b.Navigation("Tariff");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.ServiceRequest", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.RequestType", "RequestType")
                        .WithMany()
                        .HasForeignKey("RequestTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Owner", "Requestor")
                        .WithMany()
                        .HasForeignKey("RequestorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Employee", "Responser")
                        .WithMany()
                        .HasForeignKey("ResponserId");

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Status", "Status")
                        .WithMany("ServiceRequests")
                        .HasForeignKey("StatusId");

                    b.Navigation("Requestor");

                    b.Navigation("RequestType");

                    b.Navigation("Responser");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.ServiceResponse", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Employee", "Responser")
                        .WithMany()
                        .HasForeignKey("ResponserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.ServiceRequest", "ServiceRequest")
                        .WithMany()
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Responser");

                    b.Navigation("ServiceRequest");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Tariff", b =>
                {
                    b.HasOne("MunicipalOrUrbanEngineering.Entities.Flat", "Flat")
                        .WithMany("Tariffs")
                        .HasForeignKey("FlatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MunicipalOrUrbanEngineering.Entities.ServiceType", "ServiceType")
                        .WithMany()
                        .HasForeignKey("ServiceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flat");

                    b.Navigation("ServiceType");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Building", b =>
                {
                    b.Navigation("Flats");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Flat", b =>
                {
                    b.Navigation("Tariffs");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Owner", b =>
                {
                    b.Navigation("Flats");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.PaymentSheet", b =>
                {
                    b.Navigation("ServiceBills");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Role", b =>
                {
                    b.Navigation("AppUsers");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Status", b =>
                {
                    b.Navigation("ServiceRequests");
                });

            modelBuilder.Entity("MunicipalOrUrbanEngineering.Entities.Street", b =>
                {
                    b.Navigation("Buildings");
                });
#pragma warning restore 612, 618
        }
    }
}
