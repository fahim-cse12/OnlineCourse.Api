using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Service;

namespace OnlineCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseCategoryController : ControllerBase
    {
        private readonly ICourseCategoryService _courseCategoryService;
        public CourseCategoryController(ICourseCategoryService courseCategoryService)
        {
            this._courseCategoryService = courseCategoryService; 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var courseCategory = await _courseCategoryService.GetByIdAsync(id);
            if (courseCategory == null) 
            { 
                return NotFound();
            }

            return Ok(courseCategory);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courseCategoryList = await _courseCategoryService.GetAllCourseCategoriesAsync();

            return Ok(courseCategoryList);
        }
    }
}
