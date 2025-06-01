using BugFixer.Application.Security;
using HyperKala.Application.Extensions;
using HyperKala.Application.Interfaces;
using HyperKala.Application.Statics;
using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Interfaces;
using HyperKala.Domain.ViewModels.Account;
using HyperKala.Domain.ViewModels.Admin.UserRoleAndPermisson;
using Microsoft.AspNetCore.Http;
using Shop.Application.Extentions;

namespace HyperKala.Application.Services
{
    public class UserService : IUserService
    {
        #region DI
        private readonly IUserRepository _userRepository;
        private readonly ISmsService _smsService;
        public UserService(IUserRepository userRepository, ISmsService smsService)
        {
            _userRepository = userRepository;
            _smsService = smsService;
        }
        #endregion

        #region Account
        public async Task<RegisterUserResult> RegisterUserAsync(RegisterUserViewModel register)
        {
            if (await _userRepository.IsExistUserByPhoneNumberAsync(register.PhoneNumber))
                return RegisterUserResult.MobileExists;

            var userHashedPassword = PasswordHelper.HashPassword(register.Password);

            User user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserGender = UserGender.Unknown,
                PhoneNumber = register.PhoneNumber,
                Password = userHashedPassword,
                Avatar = "default.png",
                IsMobileActive = false,
                MobileActiveCode = new Random().Next(10000, 99999).ToString(),
                IsBlocked = false,
                IsDelete = false,

            };

            await _userRepository.AddNewUserAsync(user);
            await _userRepository.SaveChangeAsync();

            // Send Activation Code
            //await _smsService.SendVerificationCode(user.PhoneNumber, user.MobileActiveCode);

            return RegisterUserResult.Success;
        }

        public async Task<LoginUserResult> LoginUserAsync(LoginUserViewModel login)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(login.PhoneNumber);

            var userInputHashedPassword = PasswordHelper.HashPassword(login.Password);

            if (user == null || user.IsDelete || user.Password != userInputHashedPassword)
                return LoginUserResult.NotFound;
            if (user.IsBlocked)
                return LoginUserResult.IsBlocked;
            if (!user.IsMobileActive)
                return LoginUserResult.NotActive;

            return LoginUserResult.Success;

        }

        public Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
        }

        public async Task<ActiveAccountResult> ActivateAccountAsync(ActiveAccountViewModel activeAccount)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(activeAccount.PhoneNumber);

            if (user == null || user.IsDelete)
                return ActiveAccountResult.NotFound;
            if (user.IsBlocked)
                return ActiveAccountResult.IsBlock;
            if (user.MobileActiveCode != activeAccount.ActiveCode)
                return ActiveAccountResult.WrongCode;

            if (user.PhoneNumber == activeAccount.PhoneNumber && user.MobileActiveCode == activeAccount.ActiveCode)
            {
                user.MobileActiveCode = new Random().Next(10000, 99999).ToString();
                user.IsMobileActive = true;

                _userRepository.UpdateUser(user);
                await _userRepository.SaveChangeAsync();
                return ActiveAccountResult.Success;
            }

            return ActiveAccountResult.Error;

        }

        #endregion

        #region UserPanel
        public async Task<User?> GetUserByIdAsync(long Id)
        {
            return await _userRepository.GetUserByIdAsync(Id);
        }

        public async Task<EditUserProfileResult> EditUserProfile(long userId, IFormFile userAvatar, EditUserProfileViewModel edit)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return EditUserProfileResult.NotFound;

            user.FirstName = edit.FirstName;
            user.LastName = edit.LastName;
            user.UserGender = edit.UserGender;

            if (userAvatar != null && userAvatar.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(userAvatar.FileName);
                userAvatar.AddImageToServer(imageName, PathTools.UserAvatarUploadPath, 150, 150, PathTools.UserThumbAvatarUploadPath);

                user.Avatar = imageName;
            }

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChangeAsync();

            return EditUserProfileResult.Success;
        }

        public async Task<EditUserProfileViewModel> FillEditUserProfile(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return new EditUserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserGender = user.UserGender
            };
        }

        public async Task<ChangePasswordResult> ChangePasswordInUserPanelAsync(long userId, ChangePasswordViewModel changePassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                var newPassword = PasswordHelper.HashPassword(changePassword.NewPassword);
                if (user.Password != newPassword)
                {
                    user.Password = newPassword;
                    _userRepository.UpdateUser(user);
                    await _userRepository.SaveChangeAsync();

                    return ChangePasswordResult.Success;
                }
                return ChangePasswordResult.PasswordEqual;
            }
            return ChangePasswordResult.NotFound;
        }


        #endregion

        #region admin

        public async Task<FilterUserViewModel?> FilterUsers(FilterUserViewModel filter)
        {
            return await _userRepository.FilterUsers(filter);
        }

        public async Task<EditUserFromAdmin?> GetEditUserFromAdmin(long userId)
        {
            return await _userRepository.GetEditUserFromAdmin(userId);
        }

        public async Task<EditUserFromAdminResult> EditUserFromAdmin(EditUserFromAdmin editUser)
        {
            var user = await _userRepository.GetUserByIdAsync(editUser.Id);

            if (user == null)
                return EditUserFromAdminResult.NotFound;


            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.UserGender = editUser.UserGender;

            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.HashPassword(editUser.Password);
            }

            _userRepository.UpdateUser(user);

            await _userRepository.RemoveAllUserSelectedRole(editUser.Id);

            await _userRepository.AddUserToRole(editUser.RoleIds, editUser.Id);

            await _userRepository.SaveChangeAsync();

            return EditUserFromAdminResult.Success;

        }

        public async Task<CreateOrEditRoleViewModel?> GetEditRoleById(long roleId)
        {
            return await _userRepository.GetEditRoleById(roleId);
        }

        public async Task<CreateOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleViewModel createOrEditRole)
        {
            if (createOrEditRole.Id != 0)
            {
                var role = await _userRepository.GetRoleById(createOrEditRole.Id);

                if (role == null)
                    return CreateOrEditRoleResult.NotFound;

                role.RoleTitle = createOrEditRole.RoleTitle;

                _userRepository.UpdateRole(role);

                await _userRepository.RemoveAllPermissionSelectedRole(createOrEditRole.Id);


                if (createOrEditRole.SelectedPermission == null)
                    return CreateOrEditRoleResult.NotExistPermissions;

                await _userRepository.AddPermissionToRole(createOrEditRole.SelectedPermission, createOrEditRole.Id);

                await _userRepository.SaveChangeAsync();

                return CreateOrEditRoleResult.Success;
            }
            else
            {
                //create

                var newRole = new Role
                {
                    RoleTitle = createOrEditRole.RoleTitle
                };

                await _userRepository.CreateRole(newRole);

                if (createOrEditRole.SelectedPermission == null)
                    return CreateOrEditRoleResult.NotExistPermissions;

                await _userRepository.AddPermissionToRole(createOrEditRole.SelectedPermission, newRole.Id);

                await _userRepository.SaveChangeAsync();


                return CreateOrEditRoleResult.Success;
            }
        }

        public async Task<FilterRolesViewModel?> FilterRoles(FilterRolesViewModel filter)
        {
            return await _userRepository.FilterRoles(filter);
        }

        public async Task<List<Permission>> GetAllActivePermission()
        {
            return await _userRepository.GetAllActivePermission();
        }
        public async Task<List<Role>> GetAllActiveRoles()
        {
            return await _userRepository.GetAllActiveRoles();
        }

        public async Task<bool> CreateRoleForUserAtAdminPanelAsync(CreateRoleForUserSampleVM createRoleForUserSample)
        {
            if (await _userRepository.IsExistRoleForUser(createRoleForUserSample.RoleTitle))
                return false;

            var newRole = new Role
            {
                RoleTitle = createRoleForUserSample.RoleTitle
            };

            await _userRepository.CreateRole(newRole);
            await _userRepository.SaveChangeAsync();
            return true;
        }
        #endregion

    }
}
