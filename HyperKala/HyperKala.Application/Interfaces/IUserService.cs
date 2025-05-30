using HyperKala.Application.Services;
using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Http;

namespace HyperKala.Application.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserResult> RegisterUserAsync(RegisterUserViewModel register);
        Task<LoginUserResult> LoginUserAsync(LoginUserViewModel login);
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<ActiveAccountResult> ActivateAccountAsync(ActiveAccountViewModel activeAccount);
        Task<User?> GetUserByIdAsync(long Id);
        Task<EditUserProfileViewModel> FillEditUserProfile(long userId);
        Task<EditUserProfileResult> EditUserProfile(long useriId,IFormFile userAvatar,EditUserProfileViewModel edit);
        Task<ChangePasswordResult> ChangePasswordInUserPanelAsync(long userId,ChangePasswordViewModel changePassword);
    }
}
