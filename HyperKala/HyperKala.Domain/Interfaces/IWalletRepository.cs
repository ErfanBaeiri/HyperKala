using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Entities.Wallet;
using HyperKala.Domain.ViewModels.Wallet;

namespace HyperKala.Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task CreateWallet(UserWallet userWallet);
        Task<UserWallet?> GetUserWalletById(long walletId);
        Task SaveChangeAsync();
        void UpdateWallet(UserWallet wallet);
        Task<FilterWalletViewModel> FilterWallets(FilterWalletViewModel filter);
    }
}
