using EduTrack.Controllers;
using EduTrack.Models;
using EduTrack.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EduTrack.Tests
{
    public class CoursesControllerTests
    {
        private readonly Mock<ICourseService> _mockService;
        private readonly CoursesController _controller;
        private readonly List<Course> _courses;

        public CoursesControllerTests()
        {
            // Setup mock data
            _courses = new List<Course>
            {
                new Course { CourseId = 1, Title = "Test Course 1", Description = "Description 1", Price = 19.99m, Category = "Category1" },
                new Course { CourseId = 2, Title = "Test Course 2", Description = "Description 2", Price = 29.99m, Category = "Category2" },
                new Course { CourseId = 3, Title = "Advanced Course", Description = "Advanced Description", Price = 39.99m, Category = "Category1" }
            };

            // Setup mock service
            _mockService = new Mock<ICourseService>();
            _mockService.Setup(s => s.GetAllCourses()).Returns(_courses);
            _mockService.Setup(s => s.GetCourseById(It.IsAny<int>())).Returns((int id) => _courses.FirstOrDefault(c => c.CourseId == id));
            _mockService.Setup(s => s.SearchCourses(It.IsAny<string>())).Returns((string term) => 
                _courses.Where(c => c.Title.Contains(term) || c.Description.Contains(term) || c.Category.Contains(term)).ToList());
            
            // Setup controller with mock service
            _controller = new CoursesController(_mockService.Object);
        }

        [Fact]
        public void GetCourses_ReturnsAllCourses()
        {
            // Act
            var result = _controller.GetCourses();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Course>>>(result);
            var courses = Assert.IsAssignableFrom<IEnumerable<Course>>(actionResult.Value);
            Assert.Equal(3, courses.Count());
        }

        [Fact]
        public void GetCourse_WithValidId_ReturnsCourse()
        {
            // Act
            var result = _controller.GetCourse(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Course>>(result);
            var course = Assert.IsType<Course>(actionResult.Value);
            Assert.Equal(1, course.CourseId);
            Assert.Equal("Test Course 1", course.Title);
        }

        [Fact]
        public void GetCourse_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetCourse(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void SearchCourses_ByTitle_ReturnsMatchingCourses()
        {
            // Act
            var result = _controller.SearchCourses(title: "Advanced");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Course>>>(result);
            var courses = Assert.IsAssignableFrom<IEnumerable<Course>>(actionResult.Value);
            Assert.Single(courses);
            Assert.Equal("Advanced Course", courses.First().Title);
        }

        [Fact]
        public void SearchCourses_ByCategory_ReturnsMatchingCourses()
        {
            // Act
            var result = _controller.SearchCourses(category: "Category1");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Course>>>(result);
            var courses = Assert.IsAssignableFrom<IEnumerable<Course>>(actionResult.Value);
            Assert.Equal(2, courses.Count());
        }

        [Fact]
        public void SearchCourses_ByPriceRange_ReturnsMatchingCourses()
        {
            // Act
            var result = _controller.SearchCourses(minPrice: 25, maxPrice: 35);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Course>>>(result);
            var courses = Assert.IsAssignableFrom<IEnumerable<Course>>(actionResult.Value);
            Assert.Single(courses);
            Assert.Equal(29.99m, courses.First().Price);
        }

        [Fact]
        public void PostCourse_WithValidCourse_ReturnsCreatedAtAction()
        {
            // Arrange
            var newCourse = new Course { Title = "New Course", Description = "New Description", Price = 49.99m, Category = "New Category" };
            
            // Act
            var result = _controller.PostCourse(newCourse);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Course>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Course>(createdAtActionResult.Value);
            Assert.Equal(newCourse.Title, returnValue.Title);
        }

        [Fact]
        public void PostCourse_WithInvalidCourse_ReturnsBadRequest()
        {
            // Arrange
            var invalidCourse = new Course { Title = "", Price = -10, Category = "" };
            _controller.ModelState.AddModelError("Title", "Title is required");
            _controller.ModelState.AddModelError("Price", "Price must be positive");
            
            // Act
            var result = _controller.PostCourse(invalidCourse);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Course>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public void PutCourse_WithValidIdAndCourse_ReturnsNoContent()
        {
            // Arrange
            var courseToUpdate = new Course { CourseId = 1, Title = "Updated Title", Description = "Updated Description", Price = 59.99m, Category = "Updated Category" };
            
            // Act
            var result = _controller.PutCourse(1, courseToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCourse_WithValidId_ReturnsNoContent()
        {
            // Act
            var result = _controller.DeleteCourse(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.DeleteCourse(1), Times.Once);
        }

        [Fact]
        public void DeleteCourse_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = _controller.DeleteCourse(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
