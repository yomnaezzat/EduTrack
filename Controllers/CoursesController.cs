using EduTrack.Models;
using EduTrack.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: api/courses
        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            return _courseService.GetAllCourses();
        }

        // GET: api/courses/5
        [HttpGet("{id}")]
        public ActionResult<Course> GetCourse(int id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound(new { message = $"Course with ID {id} not found" });
            }

            return course;
        }

        // GET: api/courses/search
        [HttpGet("search")]
        public ActionResult<IEnumerable<Course>> SearchCourses(
            string title = null, 
            string category = null,
            decimal? minPrice = null, 
            decimal? maxPrice = null)
        {
            var courses = _courseService.GetAllCourses();

            // Filter by title
            if (!string.IsNullOrWhiteSpace(title))
            {
                courses = courses.Where(c => 
                    c.Title.ToLower().Contains(title.ToLower())).ToList();
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                courses = courses.Where(c => 
                    c.Category.ToLower().Contains(category.ToLower())).ToList();
            }

            // Filter by min price
            if (minPrice.HasValue)
            {
                courses = courses.Where(c => c.Price >= minPrice.Value).ToList();
            }

            // Filter by max price
            if (maxPrice.HasValue)
            {
                courses = courses.Where(c => c.Price <= maxPrice.Value).ToList();
            }

            return courses;
        }

        // PUT: api/courses/5
        [HttpPut("{id}")]
        public IActionResult PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest(new { message = "ID in URL does not match ID in request body" });
            }

            if (string.IsNullOrWhiteSpace(course.Title))
            {
                ModelState.AddModelError("Title", "Title is required");
            }

            if (course.Price < 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than or equal to 0");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCourse = _courseService.GetCourseById(id);
            if (existingCourse == null)
            {
                return NotFound(new { message = $"Course with ID {id} not found" });
            }

            _courseService.UpdateCourse(course);
            return NoContent();
        }

        // POST: api/courses
        [HttpPost]
        public ActionResult<Course> PostCourse(Course course)
        {
            if (string.IsNullOrWhiteSpace(course.Title))
            {
                ModelState.AddModelError("Title", "Title is required");
            }

            if (course.Price < 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than or equal to 0");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _courseService.AddCourse(course);
            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/courses/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound(new { message = $"Course with ID {id} not found" });
            }

            _courseService.DeleteCourse(id);
            return NoContent();
        }
    }
}
