using blog_DACS.Models;
using System.Reflection.Metadata;

namespace blog_DACS.ViewModels
{
    public class PostViewModel
    {
        public string FullName { get; set; } = null!;
        public long IdPost { get; set; }

        public string Title { get; set; } = null!;

        public string ContentPost { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public string? ImagePost { get; set; }

        public DateTime? LastAccessedAt { get; set; }

        public string? ExistenceStatus { get; set; }

        public bool? IsPublic { get; set; }

        public long IdUser { get; set; }
    }
}

