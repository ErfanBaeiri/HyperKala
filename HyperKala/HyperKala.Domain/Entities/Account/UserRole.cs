using HyperKala.Domain.Entities.BaseEntities;

namespace HyperKala.Domain.Entities.Account
{
    public class UserRole : BaseEntity
    {
        #region properties
        public long UserId { get; set; }
        public long RoleId { get; set; }
        #endregion

        #region relations
        public User User { get; set; }
        public Role Role { get; set; }
        #endregion
    }
}
