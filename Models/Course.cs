using System;
using System.ComponentModel.DataAnnotations;

namespace EduTrack.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string MaterialUrl { get; set; }

        // Material type (PDF, Text, YouTube)
        [StringLength(20)]
        public string MaterialType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
