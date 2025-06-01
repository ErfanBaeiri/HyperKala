using HyperKala.Domain.Entities.Wallet;
using HyperKala.Domain.ViewModels.Wallet;

namespace HyperKala.Application.Interfaces
{
    public interface IWalletService
    {
        Task<long> ChargeWallet(long userId, ChargeWalletViewModel chargeWallet, string description);
        Task<UserWallet?> GetUserWalletById(long walletId);
        Task<bool> UpdateWalletForCharge(UserWallet wallet);
        Task<FilterWalletViewModel> FilterWallets(FilterWalletViewModel filter);
    }
}
