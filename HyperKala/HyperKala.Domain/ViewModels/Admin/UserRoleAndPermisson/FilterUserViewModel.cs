using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.ViewModels.Paging;

namespace HyperKala.Domain.ViewModels.Admin.UserRoleAndPermisson
{
    public class FilterUserViewModel : BasePaging
    {
        public string PhoneNumber { get; set; }
        public List<User> Users { get; set; }

        #region methods
        public FilterUserViewModel SetUsers(List<User> users)
        {
            Users = users;
            return this;
        }

        public FilterUserViewModel SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;
            AllEntityCount = paging.AllEntityCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            TakeEntity = paging.TakeEntity;
            CountForShowAfterAndBefore = paging.CountForShowAfterAndBefore;
            SkipEntity = paging.SkipEntity;
            PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }
}
