using System;
using System.Collections.Generic;

namespace blog_DACS.Models;

public partial class Follow
{
    public long IdFollower { get; set; }

    public long IdFollowing { get; set; }

    public string? ExistenceStatus { get; set; }

    public virtual User IdFollowerNavigation { get; set; } = null!;

    public virtual User IdFollowingNavigation { get; set; } = null!;
}
