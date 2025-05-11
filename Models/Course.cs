using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EduTrack.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters")]
        public string Category { get; set; }

        [StringLength(255)]
        public string MaterialUrl { get; set; }

        [StringLength(20)]
        public string MaterialType { get; set; }

        [StringLength(255)]
        public string CoverImagePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class CourseCreateViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters")]
        public string Category { get; set; }

        [StringLength(255)]
        public string MaterialUrl { get; set; }

        [StringLength(20)]
        public string MaterialType { get; set; }

        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }
    }
}
