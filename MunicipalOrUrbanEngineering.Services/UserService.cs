using MunicipalOrUrbanEngineering.DataTransferObjects.Account;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MunicipalOrUrbanEngineering.DataTransferObjects;
using MunicipalOrUrbanEngineering.Entities;
using Constants = MunicipalOrUrbanEngineering.DataTransferObjects.Constants;
using System.Linq;

namespace MunicipalOrUrbanEngineering.Services
{
    public class UserService
    {
        private readonly MUEContext db;
        private readonly EmployeeService employeeService;
        private readonly OwnerService ownerService;
        private readonly HistoryService historyService;

        public UserService(MUEContext db)
        {
            this.db = db;
            employeeService = new EmployeeService(db);
            ownerService = new OwnerService(db);
            historyService = new HistoryService(db);
        }

        public async Task<bool> LoginExist(string login)
        {
            try
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(x => x.Login == login);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> PasswordMatch(string password, string repeatPassword)
        {
            try
            {
                return password == repeatPassword;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
      
        public async Task<bool> Registration(AccountRegisterDto dto, Guid? EmployeeId, Guid? FlatId)
        {
            try
            {
                Guid AppUserId = Guid.NewGuid();
                var user = new AppUser
                {
                    Id = AppUserId,
                    HashPassword = HashPassword(dto.Password),
                    Login = dto.Login,
                    RegistrationDate = DateTime.Now,
                    RoleId = dto.RoleId
                };
                await db.AppUsers.AddAsync(user);
                await db.SaveChangesAsync();
                await historyService.Create(new CreateHistoryDto
                {
                    EntityId = AppUserId,
                    Action = Constants.HistoryAction.Create,
                    CreationDate = DateTime.Now,
                    Details = null,
                    EmployeeId = EmployeeId,
                    Entity = nameof(AppUser)

                });
                if (dto.RoleId == Constants.Role.EmployeeRoleId)
                {
                    await employeeService.Create(new CreateEmployeeDto
                    {
                        AppUserId = AppUserId,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        MiddleName = dto.MiddleName
                    });
                }

                if (dto.RoleId == Constants.Role.OwnerRoleId)
                {
                    await ownerService.Create(new CreateOwnerDto
                    {
                        AppUserId = AppUserId,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        MiddleName = dto.MiddleName,
                        FlatId = FlatId
                    }, EmployeeId);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserViewDto> Get(string login)
        {
            try
            {
                var userdb = await db.AppUsers.Include(x => x.Role).FirstOrDefaultAsync(x => x.Login == login);
                return new UserViewDto
                {
                    Id = userdb.Id,
                    Login = userdb.Login,
                    RoleId = userdb.RoleId,
                    Role =  new RoleDto
                    {
                        Id = userdb.Role.Id,
                        Name = userdb.Role.Name
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> IsCorrectPassword(UserViewDto dto, string password)
        {
            try
            {
                var userdb = await GetEntity(dto.Id);
                if (VerifyHashedPassword(userdb.HashPassword, password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<AppUser> GetEntity(Guid Id)
        {
            try
            {
                return await db.AppUsers.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private string HashPassword(string password)
        {
            //TODO
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        private static bool ByteArraysEqual(byte[] buffer3, byte[] buffer4)
        {
            if (buffer3.Length != buffer4.Length)
            {
                return false;
            }
            for (int i = 0; i < buffer3.Length; i++)
            {
                if (buffer3[i] != buffer4[i])
                {
                    return false;
                }
            }

            return true;
        }


        public async Task<IList<UserViewDto>> All()
        {
            try
            {
                var entitiess = await db.AppUsers
                    .Include(x => x.Role)
                    .ToListAsync();
                if (entitiess == null)
                {
                    return null;
                }
                var owners = await ownerService.List();
                var employees = await employeeService.List();
                return entitiess.Select(entity => new UserViewDto
                {
                    Id = entity.Id,
                    Login = entity.Login,
                    Role = new RoleDto { Id = entity.Role.Id, Name = entity.Role.Name},
                    RoleId = entity.RoleId,
                    FullName = GetFullname(entity.RoleId, entity.Id, owners, employees)
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<Page<UserViewDto>> List(int pageSize = 10, int page = 1)
        {
            try
            {
                var entitiess = await db.AppUsers
                    .Include(x => x.Role)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var total = await db.AppUsers.CountAsync();
                if (entitiess == null)
                {
                    return null;
                }

                var owners = await ownerService.List();
                var employees = await employeeService.List();
                var dto = entitiess.Select(entity => new UserViewDto
                {
                    Id = entity.Id,
                    Login = entity.Login,
                    Role = new RoleDto { Id = entity.Role.Id, Name = entity.Role.Name },
                    RoleId = entity.RoleId,
                    FullName = GetFullname(entity.RoleId, entity.Id, owners, employees)
                }).ToList();

                return new Page<UserViewDto>(dto, total, pageSize, page);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GetFullname(Guid RoleId, Guid UserId, IList<ViewOwnerDto> owners, IList<EmployeeViewDto> employees)
        {
            if (RoleId == Constants.Role.EmployeeRoleId)
            {
                var employee = employees.FirstOrDefault(x => x.AppUserId == UserId);
                return employee == null ? string.Empty : employee.Fullname;
            }
            if (RoleId == Constants.Role.OwnerRoleId)
            {
                var owner = owners.FirstOrDefault(x => x.AppUserId == UserId);
                return owner == null ? string.Empty : owner.Fullname;
            }

            return string.Empty;
        }
    }
}
