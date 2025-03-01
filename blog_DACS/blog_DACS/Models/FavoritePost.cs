using System;
using System.Collections.Generic;

namespace blog_DACS.Models;

public partial class FavoritePost
{
    public long IdUser { get; set; }

    public long IdPost { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Post IdPostNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
