namespace LMS.Core.Models
{
    public class UserCourse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public virtual User? User { get; set; }
        public virtual Course? Course { get; set; }
    }
}
