using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.Services
{
    public class CourseService : ICourseService
    {
        private static List<Course> _courses = new List<Course>
        {
            new Course
            {
                CourseId = 1,
                Title = "Introduction to Programming with C#",
                Description = "Learn the basics of C# programming",
                Price = 49.99m,
                Category = "Programming",
                MaterialType = "PDF",
                MaterialUrl = "/materials/csharp-intro.pdf",
                CreatedAt = DateTime.Now
            },
            new Course
            {
                CourseId = 2,
                Title = "ASP.NET MVC Fundamentals",
                Description = "Build web applications using ASP.NET MVC",
                Price = 79.99m,
                Category = "Web Development",
                MaterialType = "Video",
                MaterialUrl = "https://www.example.com/aspnet-mvc-course",
                CreatedAt = DateTime.Now
            },
            new Course
            {
                CourseId = 3,
                Title = "Machine Learning Basics",
                Description = "Introduction to machine learning concepts",
                Price = 99.99m,
                Category = "Data Science",
                MaterialType = "Text",
                MaterialUrl = "/materials/ml-basics.html",
                CreatedAt = DateTime.Now
            },
            new Course
            {
                CourseId = 4,
                Title = "JavaScript for Beginners",
                Description = "Learn the fundamentals of JavaScript programming",
                Price = 39.99m,
                Category = "Web Development",
                MaterialType = "Video",
                MaterialUrl = "https://www.example.com/js-beginners",
                CreatedAt = DateTime.Now
            }
        };

        private static int _nextId = 5;

        public List<Course> GetAllCourses()
        {
            return _courses;
        }

        public Course GetCourseById(int id)
        {
            return _courses.FirstOrDefault(c => c.CourseId == id);
        }

        public List<Course> SearchCourses(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return _courses;

            searchTerm = searchTerm.ToLower();
            return _courses.Where(c => 
                c.Title.ToLower().Contains(searchTerm) || 
                (c.Description != null && c.Description.ToLower().Contains(searchTerm)) ||
                c.Category.ToLower().Contains(searchTerm))
                .ToList();
        }

        public void AddCourse(Course course)
        {
            course.CourseId = _nextId++;
            course.CreatedAt = DateTime.Now;
            _courses.Add(course);
        }

        public void UpdateCourse(Course course)
        {
            var existingCourse = _courses.FirstOrDefault(c => c.CourseId == course.CourseId);
            if (existingCourse != null)
            {
                existingCourse.Title = course.Title;
                existingCourse.Description = course.Description;
                existingCourse.Price = course.Price;
                existingCourse.Category = course.Category;
                existingCourse.MaterialType = course.MaterialType;
                existingCourse.MaterialUrl = course.MaterialUrl;
            }
        }

        public void DeleteCourse(int id)
        {
            var course = _courses.FirstOrDefault(c => c.CourseId == id);
            if (course != null)
            {
                _courses.Remove(course);
            }
        }
    }
}
