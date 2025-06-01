using HyperKala.DataLayer.Context;
using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Interfaces;
using HyperKala.Domain.ViewModels.Admin.UserRoleAndPermisson;
using HyperKala.Domain.ViewModels.Paging;
using Microsoft.EntityFrameworkCore;

namespace HyperKala.DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region DI
        private readonly HyperKalaDbContext _context;
        public UserRepository(HyperKalaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region General Method
        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Account
        public async Task<bool> IsExistUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(s => s.PhoneNumber == phoneNumber);
        }

        public async Task AddNewUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.SingleOrDefaultAsync(s => s.PhoneNumber == phoneNumber);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        #endregion

        #region UserPanel
        public async Task<User?> GetUserByIdAsync(long id)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.Id == id);
        }
        #endregion

        #region admin
        public async Task<FilterUserViewModel?> FilterUsers(FilterUserViewModel filter)
        {
            var query = _context.Users.AsQueryable();


            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumber == filter.PhoneNumber);
            }


            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetPaging(pager).SetUsers(allData);
        }

        public async Task<EditUserFromAdmin?> GetEditUserFromAdmin(long userId)
        {
            return await _context.Users.AsQueryable()
                .Include(c => c.UserRoles)
                .Where(c => c.Id == userId)
                .Select(x => new EditUserFromAdmin
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserGender = x.UserGender,
                    RoleIds = x.UserRoles.Where(c => c.UserId == userId).Select(c => c.RoleId).ToList()
                }).SingleOrDefaultAsync();
        }

        public async Task<CreateOrEditRoleViewModel?> GetEditRoleById(long roleId)
        {
            return await _context.Roles.AsQueryable().Include(x => x.RolePermissions)
                 .Where(c => c.Id == roleId)
                 .Select(x => new CreateOrEditRoleViewModel
                 {
                     Id = x.Id,
                     RoleTitle = x.RoleTitle,
                     SelectedPermission = x.RolePermissions.Select(s => s.PermissionId).ToList()

                 }).SingleOrDefaultAsync();
        }

        public async Task CreateRole(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
        }

        public async Task<Role?> GetRoleById(long id)
        {
            return await _context.Roles.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<FilterRolesViewModel?> FilterRoles(FilterRolesViewModel filter)
        {
            var query = _context.Roles.AsQueryable();

            #region filter
            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                query = query.Where(c => EF.Functions.Like(c.RoleTitle, $"%{filter.RoleName}%"));
            }
            #endregion

            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetPaging(pager).SetRoles(allData);
        }

        public async Task<List<Permission>> GetAllActivePermission()
        {
            return await _context.Permissions.Where(c => !c.IsDelete).ToListAsync();
        }
        public async Task RemoveAllPermissionSelectedRole(long roleId)
        {
            var allRolePermissions = await _context.RolePermissions.Where(c => c.RoleId == roleId).ToListAsync();

            if (allRolePermissions.Any())
            {
                _context.RolePermissions.RemoveRange(allRolePermissions);
            }
        }
        public async Task AddPermissionToRole(List<long> selctedPermission, long roleId)
        {
            if (selctedPermission != null && selctedPermission.Any())
            {
                var rolePermissions = new List<RolePermission>();

                foreach (var permissionId in selctedPermission)
                {
                    rolePermissions.Add(new RolePermission
                    {
                        PermissionId = permissionId,
                        RoleId = roleId,

                    });
                }

                await _context.RolePermissions.AddRangeAsync(rolePermissions);
            }
        }

        public async Task<List<Role>> GetAllActiveRoles()
        {
            return await _context.Roles.AsQueryable().Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task RemoveAllUserSelectedRole(long userId)
        {
            var allUserRoles = await _context.UserRoles.AsQueryable().Where(c => c.UserId == userId).ToListAsync();

            if (allUserRoles.Any())
            {
                _context.UserRoles.RemoveRange(allUserRoles);

                await _context.SaveChangesAsync();
            }
        }

        public async Task AddUserToRole(List<long> selectedRole, long userId)
        {
            if (selectedRole != null && selectedRole.Any())
            {
                var userRoles = new List<UserRole>();

                foreach (var roleId in selectedRole)
                {
                    userRoles.Add(new UserRole
                    {
                        RoleId = roleId,
                        UserId = userId
                    });
                }

                await _context.UserRoles.AddRangeAsync(userRoles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsExistRoleForUser(string roleTitle)
        {
           return await _context.Roles.AnyAsync(s => s.RoleTitle == roleTitle);
        }
        #endregion

    }
}
