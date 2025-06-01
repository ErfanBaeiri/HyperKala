using HyperKala.DataLayer.Context;
using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Entities.Wallet;
using HyperKala.Domain.Interfaces;
using HyperKala.Domain.ViewModels.Paging;
using HyperKala.Domain.ViewModels.Wallet;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace HyperKala.DataLayer.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        #region DI
        private readonly HyperKalaDbContext _context;
        public WalletRepository(HyperKalaDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task CreateWallet(UserWallet userWallet)
        {
            await _context.UserWallets.AddAsync(userWallet);
        }

        public async Task<UserWallet?> GetUserWalletById(long walletId)
        {
            return await _context.UserWallets.AsQueryable().SingleOrDefaultAsync(s => s.Id == walletId);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateWallet(UserWallet wallet)
        {
            _context.UserWallets.Update(wallet);
        }

        public async Task<FilterWalletViewModel> FilterWallets(FilterWalletViewModel filter)
        {
            var query = _context.UserWallets.AsQueryable();

            #region filter
            if (filter.UserId != 0 && filter.UserId != null)
            {
                query = query.Where(c => c.UserId == filter.UserId);
            }
            #endregion

            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetPaging(pager).SetWallets(allData);
        }
    }
}
