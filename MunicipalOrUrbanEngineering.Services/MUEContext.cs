using System;
using Microsoft.EntityFrameworkCore;
using MunicipalOrUrbanEngineering.Entities;

namespace MunicipalOrUrbanEngineering.Services
{
    public class MUEContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceResponse> ServiceResponses { get; set; }
        public DbSet<BulletinBoardItem> BulletinBoardItems { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<MetersData> MetersDatas { get; set; }
        public DbSet<PaymentPeriod> PaymentPeriods { get; set; }
        public DbSet<PaymentSheet> PaymentSheets { get; set; }
        public DbSet<ServiceBill> ServiceBills { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public MUEContext(DbContextOptions<MUEContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
