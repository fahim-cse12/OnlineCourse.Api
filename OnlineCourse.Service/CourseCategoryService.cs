using OnlineCourse.Core.ViewModels;
using OnlineCourse.Data;

namespace OnlineCourse.Service
{
    public class CourseCategoryService : ICourseCategoryService
    {
        private readonly ICourseCategoryRepository courseCategoryRepository;

        public CourseCategoryService(ICourseCategoryRepository courseCategoryRepository)
        {
            this.courseCategoryRepository = courseCategoryRepository;
        }
        public async Task<List<CourseCategoryViewModel>> GetAllCourseCategoriesAsync()
        {
            var dataList = await courseCategoryRepository.GetAllCourseCategoriesAsync();
            return dataList.Select(s=> new CourseCategoryViewModel
            {
                CategoryId = s.CategoryId,
                CategoryName = s.CategoryName,
                Description = s.Description
            }).ToList();
        }

        public async Task<CourseCategoryViewModel?> GetByIdAsync(int id)
        {
           var modelData = await courseCategoryRepository.GetByIdAsync(id);
            return modelData == null ? null :  new CourseCategoryViewModel
            {
                CategoryId = modelData.CategoryId,
                CategoryName = modelData.CategoryName,
                Description = modelData.Description
            };
        }
    }
}
