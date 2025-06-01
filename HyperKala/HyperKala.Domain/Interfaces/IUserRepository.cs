using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.ViewModels.Admin;

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
        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);
        Task<EditUserFromAdmin?> GetEditUserFromAdmin(long userId);

        #endregion
    }
}
