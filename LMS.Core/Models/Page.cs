namespace LMS.Core.Models
{
    public class Page
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Order { get; set; } // Position in the course
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // HTML content
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int CurrentVersion { get; set; } = 1;

        public virtual Course? Course { get; set; }
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<PageVersion> Versions { get; set; } = new List<PageVersion>();
    }
}
