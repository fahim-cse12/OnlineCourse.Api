using Microsoft.EntityFrameworkCore;
using OnlineCourse.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourse.Data
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DbAaf752LmsContext _dbContext;

        public CourseRepository(DbAaf752LmsContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<List<CourseViewModel>> GetAllCoursesAsync(int? categoryId = null)
        {
            var query = _dbContext.Courses.Include(c => c.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(i=> i.CategoryId == categoryId);
            }

            var courses = await query.Select(s=> new CourseViewModel
            {
                CourseId = s.CourseId,
                CategoryId = s.CategoryId,
                Title = s.Title,
                Description = s.Description,
                Price = s.Price,
                CourseType = s.CourseType,
                SeatsAvailable = s.SeatsAvailable,
                Duration = s.Duration,
                InstructorId = s.InstructorId,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Category = new CourseCategoryViewModel
                { 
                    CategoryId = s.Category.CategoryId,
                    CategoryName = s.Category.CategoryName,
                    Description = s.Category.Description,
                },
                UserRating = new UserRatingViewModel 
                {
                    CourseId=s.CourseId,
                    AverageRating = s.Reviews.Any() ? Convert.ToDecimal(s.Reviews.Average(r => r.Rating)) : 0,
                    TotalRating = s.Reviews.Count(),
                }
            }).ToListAsync();

            return courses;
        }

        public async Task<CourseDetailViewModel> GetCourseDetailAsync(int courseId)
        {
            var course = await _dbContext.Courses.Include(c => c.Category)
                                                 .Include(c => c.Reviews)
                                                 .Include(c => c.SessionDetails)
                                                 .Where(c => c.CourseId == courseId)
                                                 .Select(c => new CourseDetailViewModel
                                                 {
                                                     CourseId = courseId,
                                                     Title = c.Title,
                                                     Description = c.Description,
                                                     Price = c.Price,
                                                     CourseType = c.CourseType,
                                                     SeatsAvailable = c.SeatsAvailable,
                                                     Duration = c.Duration,
                                                     CategoryId = c.CategoryId  ,
                                                     InstructorId = c.InstructorId ,
                                                     StartDate = c.StartDate,
                                                     EndDate = c.EndDate,
                                                     Category = new CourseCategoryViewModel
                                                     {
                                                         CategoryId = c.Category.CategoryId ,
                                                         CategoryName = c.Category.CategoryName ,
                                                         Description = c.Category.Description,
                                                     }, 
                                                     Reviews = c.Reviews.Select(r=> new UserReviewViewModel
                                                     {
                                                         CourseId=r.CourseId,
                                                         UserName = r.User.DisplayName,
                                                         Rating = r.Rating,
                                                         Comments = r.Comments,
                                                         ReviewDate = r.ReviewDate,
                                                     }).OrderByDescending(i=> i.Rating).Take(10).ToList(),
                                                     SessionDetail = c.SessionDetails.Select(s=> new SessionDetailViewModel
                                                     {
                                                         SessionId = s.SessionId,
                                                         CourseId = s.CourseId,
                                                         Title= s.Title,
                                                         Description = s.Description,
                                                         VideoOrder = s.VideoOrder,
                                                         VideoUrl = s.VideoUrl
                                                     }).OrderBy(o=> o.VideoOrder).ToList(),
                                                     UserRating = new UserRatingViewModel
                                                     {
                                                         CourseId = c.CourseId,
                                                         AverageRating = c.Reviews.Any() ? Convert.ToDecimal(c.Reviews.Average(r=> r.Rating)):  0,
                                                         TotalRating = c.Reviews.Count()
                                                     }
                                                 }).FirstOrDefaultAsync();

            return course;
        }
    }
}
