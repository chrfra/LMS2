namespace LMS.Core.Models
{
    public class PageVersion
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int CourseId { get; set; }
        public int VersionNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Page? Page { get; set; }
        public virtual Course? Course { get; set; }
    }
}
