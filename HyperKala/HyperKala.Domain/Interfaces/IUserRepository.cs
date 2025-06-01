using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.ViewModels.Admin.UserRoleAndPermisson;

namespace HyperKala.Domain.Interfaces
{
    public interface IUserRepository
    {


        #region Genetal Method
        Task SaveChangeAsync();
        #endregion

        #region Account
        Task<bool> IsExistUserByPhoneNumberAsync(string phoneNumber);
        Task AddNewUserAsync(User user);
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        void UpdateUser(User user);
        #endregion

        #region UserPanel
        Task<User?> GetUserByIdAsync(long id);
        #endregion

        #region admin
        Task<FilterUserViewModel?> FilterUsers(FilterUserViewModel filter);
        Task<EditUserFromAdmin?> GetEditUserFromAdmin(long userId);
        Task<CreateOrEditRoleViewModel?> GetEditRoleById(long roleId);
        Task<FilterRolesViewModel?> FilterRoles(FilterRolesViewModel filter);
        Task CreateRole(Role role);
        void UpdateRole(Role role);
        Task<Role?> GetRoleById(long id);
        Task<List<Permission>> GetAllActivePermission();
        Task RemoveAllPermissionSelectedRole(long roleId);
        Task AddPermissionToRole(List<long> selctedPermission, long roleId);
        Task<List<Role>> GetAllActiveRoles();
        Task RemoveAllUserSelectedRole(long userId);
        Task AddUserToRole(List<long> selctedRole, long userId);
        Task<bool> IsExistRoleForUser(string roleTitle);
        #endregion
    }
}
