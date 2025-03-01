namespace blog_DACS.View_Models
{
    public class CommentViewModels
    {
        public string ContentComment { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public int? ParentComment { get; set; }

        public long IdPost { get; set; }

        public long IdUser { get; set; }
        public string FullName { get; set; } = null!;
    }
}
