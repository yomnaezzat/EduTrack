using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.Services
{
    public interface ICourseService
    {
        List<Course> GetAllCourses();
        Course GetCourseById(int id);
        List<Course> SearchCourses(string searchTerm);
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int id);
    }
}
