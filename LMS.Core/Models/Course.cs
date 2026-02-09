namespace LMS.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ScormPackageData { get; set; } = string.Empty; // Base64 encoded
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int CurrentVersion { get; set; } = 1;

        public virtual ICollection<Page> Pages { get; set; } = new List<Page>();
        public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<PageVersion> PageVersions { get; set; } = new List<PageVersion>();
        public virtual ICollection<CourseFeedback> Feedbacks { get; set; } = new List<CourseFeedback>();
    }
}
