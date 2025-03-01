using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace blog_DACS.Models;

public partial class User
{
    public long IdUser { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? PlaceOfBirth { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public string? ExistenceStatus { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Pass { get; set; } = null!;

    public long IdRole { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FavoritePost> FavoritePosts { get; set; } = new List<FavoritePost>();

    public virtual ICollection<FavoriteUser> FavoriteUserIdFavoriteUserNavigations { get; set; } = new List<FavoriteUser>();

    public virtual ICollection<FavoriteUser> FavoriteUserIdUserNavigations { get; set; } = new List<FavoriteUser>();

    public virtual ICollection<Follow> FollowIdFollowerNavigations { get; set; } = new List<Follow>();

    public virtual ICollection<Follow> FollowIdFollowingNavigations { get; set; } = new List<Follow>();

    public virtual RoleUser IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Liked> Likeds { get; set; } = new List<Liked>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();
}
