using System;
using System.Collections.Generic;

namespace Swift.DataLayer.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Passwor { get; set; }

    public string? ActivateCode { get; set; }

    public bool? IsActive { get; set; }

    public string? UserAvatar { get; set; }

    public DateOnly? RegisterDate { get; set; }

    public string? Mobile { get; set; }

    public string? Tell { get; set; }

    public string? Adress { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
