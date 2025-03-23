using OnlineCourse.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourse.Service
{
    public interface ICourseService
    {
        Task<List<CourseViewModel>> GetAllCoursesAsync(int? categoryId = null);
        Task<CourseDetailViewModel> GetCourseDetailAsync(int courseId);
    }
}
