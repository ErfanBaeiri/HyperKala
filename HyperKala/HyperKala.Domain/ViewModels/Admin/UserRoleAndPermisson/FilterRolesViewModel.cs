using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.ViewModels.Paging;

namespace HyperKala.Domain.ViewModels.Admin.UserRoleAndPermisson
{
    public class FilterRolesViewModel : BasePaging
    {
        public string RoleName { get; set; }
        public List<Role> Roles { get; set; }


        #region methods
        public FilterRolesViewModel SetRoles(List<Role> roles)
        {
            Roles = roles;
            return this;
        }

        public FilterRolesViewModel SetPaging(BasePaging paging)
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
