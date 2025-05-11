using EduTrack.Models;
using EduTrack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EduTrack.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesApiController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesApiController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: api/CoursesApi
        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            return _courseService.GetAllCourses();
        }

        // GET: api/CoursesApi/5
        [HttpGet("{id}")]
        public ActionResult<Course> GetCourse(int id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // GET: api/CoursesApi/search?term=mvc
        [HttpGet("search")]
        public ActionResult<IEnumerable<Course>> SearchCourses(string term)
        {
            return _courseService.SearchCourses(term);
        }

        // PUT: api/CoursesApi/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            if (_courseService.GetCourseById(id) == null)
            {
                return NotFound();
            }

            _courseService.UpdateCourse(course);
            return NoContent();
        }

        // POST: api/CoursesApi
        [HttpPost]
        [Authorize]
        public ActionResult<Course> PostCourse(Course course)
        {
            _courseService.AddCourse(course);
            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/CoursesApi/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteCourse(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }

            _courseService.DeleteCourse(id);
            return NoContent();
        }
    }
}
