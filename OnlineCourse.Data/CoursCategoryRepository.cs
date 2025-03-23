using Microsoft.EntityFrameworkCore;
using OnlineCourse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourse.Data
{
    public class CoursCategoryRepository(DbAaf752LmsContext dbContext) : ICourseCategoryRepository
    {
        private readonly DbAaf752LmsContext _dbContext = dbContext;
        public List<CourseCategory> GetAllCourseCategories()
        {
           var courseCategories = _dbContext.CourseCategories;
            return courseCategories.ToList();
        }

        public Task<List<CourseCategory>> GetAllCourseCategoriesAsync()
        {
            var courseCategories = _dbContext.CourseCategories.ToListAsync();
            return courseCategories;
        }

        public Task<CourseCategory?> GetByIdAsync(int id)
        {
            var data = _dbContext.CourseCategories.FindAsync(id).AsTask();
            return data;
        }
    }
}
