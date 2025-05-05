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
                Id = 1,
                Title = "Introduction to Programming with C#",
                Description = "Learn the basics of C# programming",
                Price = 49.99m,
                MaterialType = "PDF",
                MaterialUrl = "/materials/csharp-intro.pdf",
                CreatedAt = DateTime.Now
            },
            new Course
            {
                Id = 2,
                Title = "ASP.NET MVC Fundamentals and Best Practices",
                Description = "Build web applications using ASP.NET MVC",
                Price = 79.99m,
                MaterialType = "YouTube",
                MaterialUrl = "https://www.youtube.com/watch?v=example",
                CreatedAt = DateTime.Now
            }
        };

        private static int _nextId = 3;

        public List<Course> GetAllCourses()
        {
            return _courses;
        }

        public Course GetCourseById(int id)
        {
            return _courses.FirstOrDefault(c => c.Id == id);
        }

        public List<Course> SearchCourses(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return _courses;

            searchTerm = searchTerm.ToLower();
            return _courses.Where(c => 
                c.Title.ToLower().Contains(searchTerm) || 
                (c.Description != null && c.Description.ToLower().Contains(searchTerm)))
                .ToList();
        }

        public void AddCourse(Course course)
        {
            course.Id = _nextId++;
            course.CreatedAt = DateTime.Now;
            _courses.Add(course);
        }

        public void UpdateCourse(Course course)
        {
            var existingCourse = _courses.FirstOrDefault(c => c.Id == course.Id);
            if (existingCourse != null)
            {
                existingCourse.Title = course.Title;
                existingCourse.Description = course.Description;
                existingCourse.Price = course.Price;
                existingCourse.MaterialType = course.MaterialType;
                existingCourse.MaterialUrl = course.MaterialUrl;
            }
        }

        public void DeleteCourse(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                _courses.Remove(course);
            }
        }
    }
}
