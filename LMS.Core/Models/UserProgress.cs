namespace LMS.Core.Models
{
    public class UserProgress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int PageId { get; set; }
        public bool Completed { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;

        public virtual User? User { get; set; }
        public virtual Course? Course { get; set; }
        public virtual Page? Page { get; set; }
    }
}
