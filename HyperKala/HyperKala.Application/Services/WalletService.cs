using HyperKala.Application.Interfaces;
using HyperKala.Domain.Entities.Wallet;
using HyperKala.Domain.Interfaces;
using HyperKala.Domain.ViewModels.Wallet;

namespace HyperKala.Application.Services
{
    public class WalletService : IWalletService
    {
        #region DI
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        public WalletService(IUserRepository userRepository, IWalletRepository walletRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }
        #endregion

        public async Task<long> ChargeWallet(long userId, ChargeWalletViewModel chargeWallet, string description)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return 0;

            var wallet = new UserWallet
            {
                UserId = userId,
                Amount = chargeWallet.Amount,
                Description = description,
                IsPay = false,
                WalletType = WalletType.Variz
            };
            await _walletRepository.CreateWallet(wallet);
            await _walletRepository.SaveChangeAsync();

            return wallet.Id;
        }

        public async Task<UserWallet?> GetUserWalletById(long walletId)
        {
            return await _walletRepository.GetUserWalletById(walletId);
        }

        public async Task<bool> UpdateWalletForCharge(UserWallet wallet)
        {
            if (wallet != null)
            {
                wallet.IsPay = true;
                _walletRepository.UpdateWallet(wallet);
                await _walletRepository.SaveChangeAsync();
                return true;
            }
            return false;
        }

        public async Task<FilterWalletViewModel> FilterWallets(FilterWalletViewModel filter)
        {
            return await _walletRepository.FilterWallets(filter);
        }



    }
}
