using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.ViewModels.Admin.UserRoleAndPermisson
{
    public class CreateRoleForUserSampleVM
    {
        [DisplayName("نقش")]
        [Required(ErrorMessage = "فیلد نقش الزامی است")]
        public required string RoleTitle { get; set; }
    }
}
