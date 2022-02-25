using System;
using System.ComponentModel.DataAnnotations;

namespace Week9Day5Demo.Models
{
    public class Student
    {
        [Key]
        [Required(ErrorMessage = "RollNo cannot be blank")]
        public int RollNo { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Range(0, 100, ErrorMessage = "Percentage is required and must be between 0 and 100")]
        public double Percentage { get; set; }
    }
}