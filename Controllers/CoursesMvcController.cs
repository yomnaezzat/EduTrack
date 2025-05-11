using EduTrack.Models;
using EduTrack.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduTrack.Controllers
{
    public class CoursesMvcController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CoursesMvcController(ICourseService courseService, IWebHostEnvironment webHostEnvironment)
        {
            _courseService = courseService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: CoursesMvc
        public IActionResult Index()
        {
            var courses = _courseService.GetAllCourses();
            return View(courses);
        }

        // GET: CoursesMvc/Details/5
        public IActionResult Details(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: CoursesMvc/Create
        public IActionResult Create()
        {
            return View(new CourseCreateViewModel());
        }

        // POST: CoursesMvc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                Course course = new Course
                {
                    Title = courseViewModel.Title,
                    Description = courseViewModel.Description,
                    Price = courseViewModel.Price,
                    Category = courseViewModel.Category,
                    MaterialType = courseViewModel.MaterialType,
                    MaterialUrl = courseViewModel.MaterialUrl
                };

                // Handle file upload if present
                if (courseViewModel.CoverImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "courses");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + courseViewModel.CoverImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await courseViewModel.CoverImage.CopyToAsync(fileStream);
                    }

                    course.CoverImagePath = "/images/courses/" + uniqueFileName;
                }

                _courseService.AddCourse(course);
                return RedirectToAction(nameof(Index));
            }
            return View(courseViewModel);
        }

        // GET: CoursesMvc/Edit/5
        public IActionResult Edit(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }

            var courseViewModel = new CourseCreateViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Category = course.Category,
                MaterialType = course.MaterialType,
                MaterialUrl = course.MaterialUrl
            };

            ViewBag.CurrentCoverImage = course.CoverImagePath;

            return View(courseViewModel);
        }

        // POST: CoursesMvc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseCreateViewModel courseViewModel)
        {
            if (id != courseViewModel.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCourse = _courseService.GetCourseById(id);
                    if (existingCourse == null)
                    {
                        return NotFound();
                    }

                    existingCourse.Title = courseViewModel.Title;
                    existingCourse.Description = courseViewModel.Description;
                    existingCourse.Price = courseViewModel.Price;
                    existingCourse.Category = courseViewModel.Category;
                    existingCourse.MaterialType = courseViewModel.MaterialType;
                    existingCourse.MaterialUrl = courseViewModel.MaterialUrl;

                    // Handle file upload if present
                    if (courseViewModel.CoverImage != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "courses");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + courseViewModel.CoverImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await courseViewModel.CoverImage.CopyToAsync(fileStream);
                        }

                        // Delete the old image if exists
                        if (!string.IsNullOrEmpty(existingCourse.CoverImagePath))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                                existingCourse.CoverImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                            
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        existingCourse.CoverImagePath = "/images/courses/" + uniqueFileName;
                    }

                    _courseService.UpdateCourse(existingCourse);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    if (_courseService.GetCourseById(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            ViewBag.CurrentCoverImage = _courseService.GetCourseById(id)?.CoverImagePath;
            return View(courseViewModel);
        }

        // GET: CoursesMvc/Delete/5
        public IActionResult Delete(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: CoursesMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(course.CoverImagePath))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                    course.CoverImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _courseService.DeleteCourse(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: CoursesMvc/Search
        public IActionResult Search(string searchTerm)
        {
            var courses = _courseService.SearchCourses(searchTerm);
            ViewData["SearchTerm"] = searchTerm;
            return View("Index", courses);
        }
    }
}
