using System;
using System.Collections.Generic;
using MunicipalOrUrbanEngineering.DataTransferObjects.Account;

namespace MunicipalOrUrbanEngineering.DataTransferObjects
{
    public static class Constants
    {
        //TODO
     
        public static class Role
        {
            public static readonly Guid EmployeeRoleId = new Guid("a8013028-e382-4672-ad44-43df699dfd1e");

            public static readonly Guid OwnerRoleId = new Guid("9b7dab16-e2b8-47ad-b639-d6d76ac97afc");

            public static readonly Guid SysAdminRoleId = new Guid("4e3c33e5-7ffe-4515-9867-90e11a20df04");

            public static IList<ViewRole> List()
            {
                List<ViewRole> list = new List<ViewRole>();
                list.Add(new ViewRole() { Id = EmployeeRoleId, Name = "Работник ЖКХ" });
                list.Add(new ViewRole() { Id = OwnerRoleId, Name = "Владелец квартиры" });
                list.Add(new ViewRole() { Id = SysAdminRoleId, Name = "Системный администратор" });
                return list;
            }
        }
        public static class HistoryAction
        {
            public static readonly string Create = "Create item";
            public static readonly string Delete = "Delete item";
            public static readonly string Update = "Update item";

            public static readonly string AssignFlatToOwner = "Assign flat to owner";
            public static readonly string RemoveAssignFlatToOwner = "Remove assign flat to owner";

            public static readonly string SetStatus = "Change status";

        }
        public static class BaseWord
        {
            public static readonly string Building = "Дом";
            public static readonly string Street = "Улица";
            public static readonly string Apartament = "Квартира";

            public static readonly string AllUsers = "Все пользователи";

            public static readonly string ServiceType = "Вид услуги";
            public static readonly string UnitName = "Ед. измерения";
            public static readonly string Amount = "Стоимость";
        }
        public static class RequestStatus
        {
            public static readonly Guid New = new Guid("14680FC5-5632-4713-8BB1-DC9D7C6660E1");
            public static readonly Guid Cancel = new Guid("10BC5552-53CE-4285-840D-B7AC1041B0B7");
            public static readonly Guid Completed = new Guid("6D75AF1A-2A14-440E-9985-4459FE50A126");
        }
        public static class PaymentPeriodStatus
        {
            public static readonly Guid New = new Guid("15E2D3B0-03C0-49EF-8D8C-882BAD7F19DB");
            public static readonly Guid CollectingReadings = new Guid("DB8EC517-8841-47AC-A9E6-421DF83F5C71");
            public static readonly Guid FormationOfReceipts = new Guid("2176460C-A6A1-4536-9809-2E417EBE5A54");
            public static readonly Guid PaymentOfReceipts = new Guid("D93E31A6-DE36-4AEB-9AE4-19305D635657");
            public static readonly Guid Close = new Guid("1A0C8FDB-2E2A-46AF-8F64-01B67258291A");
        }
        public static class PaymentSheetStatus
        {
            public static readonly Guid InitializationProcess = new Guid("E56C8BBC-CE41-4558-AC7D-ACBBB8CD093B");
            public static readonly Guid ReadyForPayment = new Guid("08E4B1A5-E7D1-4F57-9CF3-8C571E80E179");
            public static readonly Guid IsPaid = new Guid("33271260-77C1-47FE-A7A7-0934D993EE2B");
            public static readonly Guid IsPartiallyPaid = new Guid("0DB18436-2BAB-456C-A7D1-50A1A0F8FBB4");
        }
        public static class ServiceBillStatus
        {
            public static readonly Guid ReadyForPayment = new Guid("FB4AF0BF-D7BB-4463-A490-0503DCB51560");
            public static readonly Guid IsPaid = new Guid("18DD5709-5326-4373-8EA5-4D57E2C7C091");
        }
    }

   



}
