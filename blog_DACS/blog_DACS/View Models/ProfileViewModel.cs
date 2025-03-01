namespace blog_DACS.View_Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; } = null!;
        public long IdPost { get; set; }
        public string Title { get; set; }
        public string ContentPost { get; set; }
        public string? ImagePost { get; set; }
        public int Shares { get; set; }
    }
}
