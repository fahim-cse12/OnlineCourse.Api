using OnlineCourse.Core.ViewModels;

namespace OnlineCourse.Service
{
    public interface ICourseCategoryService
    {
        Task<CourseCategoryViewModel?> GetByIdAsync(int id);
        Task<List<CourseCategoryViewModel>> GetAllCourseCategoriesAsync();
    }
}
