using System;
using System.Collections.Generic;

namespace blog_DACS.Models;

public partial class FavoriteUser
{
    public long IdUser { get; set; }

    public long IdFavoriteUser { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User IdFavoriteUserNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
