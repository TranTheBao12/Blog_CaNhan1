using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace blog_DACS.Models;

public partial class Post
{
    [Key]
    public long IdPost { get; set; }
    [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
    [Display(Name = "Tiêu đề")]
    [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
    public string Title { get; set; } = null!;
    [Display(Name = "Nội dung")]
    public string ContentPost { get; set; } = null!;
    [Display(Name = "Ngày tạo")]
    public DateTime? CreatedAt { get; set; }

    public string? ImagePost { get; set; }

    public DateTime? LastAccessedAt { get; set; }

    public string? ExistenceStatus { get; set; }

    public bool? IsPublic { get; set; }

    public long IdUser { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FavoritePost> FavoritePosts { get; set; } = new List<FavoritePost>();

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Liked> Likeds { get; set; } = new List<Liked>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();
}
