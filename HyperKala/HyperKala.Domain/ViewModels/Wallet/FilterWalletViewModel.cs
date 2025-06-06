﻿using HyperKala.Domain.Entities.Wallet;
using HyperKala.Domain.ViewModels.Paging;

namespace HyperKala.Domain.ViewModels.Wallet
{
    public class FilterWalletViewModel : BasePaging
    {
        #region properties
        public long? UserId { get; set; }
        public List<UserWallet> UserWallets { get; set; }
        #endregion

        #region methods
        public FilterWalletViewModel SetWallets(List<UserWallet> userWallets)
        {
            this.UserWallets = userWallets;
            return this;
        }

        public FilterWalletViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.CountForShowAfterAndBefore = paging.CountForShowAfterAndBefore;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }
}
