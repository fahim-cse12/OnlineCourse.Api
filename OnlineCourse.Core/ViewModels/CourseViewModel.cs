namespace OnlineCourse.Core.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CourseType { get; set; } = null!;
        public int? SeatsAvailable { get; set; }
        public decimal Duration { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Thumbnail { get; set; }
        public CourseCategoryViewModel Category { get; set; } = null!;
        public UserRatingViewModel UserRating { get; set; }
        //public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        //public virtual Instructor Instructor { get; set; } = null!;
        //public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        //public virtual ICollection<SessionDetail> SessionDetails { get; set; } = new List<SessionDetail>();
    }

    public class CourseDetailViewModel : CourseViewModel
    {
        public List<UserReviewViewModel> Reviews { get; set; } = new List<UserReviewViewModel>();
        public List<SessionDetailViewModel> SessionDetail { get; set; } = new List<SessionDetailViewModel>();
    }
    public class UserRatingViewModel
    {
        public int CourseId { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRating { get; set; }
    }

    public class UserReviewViewModel
    {
        public int CourseId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime? ReviewDate { get; set; }
    }

    public class SessionDetailViewModel
    {
        public int SessionId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = null;
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public int VideoOrder { get; set; }
    }
}
