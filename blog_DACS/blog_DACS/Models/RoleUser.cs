using System;
using System.Collections.Generic;

namespace blog_DACS.Models;

public partial class RoleUser
{
    public long IdRole { get; set; }

    public string RoleName { get; set; } = null!;

    public string DescriptionRole { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Permission> IdPermissions { get; set; } = new List<Permission>();
}
