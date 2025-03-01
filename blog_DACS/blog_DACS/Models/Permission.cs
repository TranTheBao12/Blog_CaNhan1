using System;
using System.Collections.Generic;

namespace blog_DACS.Models;

public partial class Permission
{
    public long IdPermission { get; set; }

    public string NamePermission { get; set; } = null!;

    public string DescriptionPermission { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<RoleUser> IdRoles { get; set; } = new List<RoleUser>();
}
