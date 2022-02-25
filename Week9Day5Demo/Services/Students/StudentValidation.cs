using System;
using Week9Day5Demo.Models;

namespace Week9Day5Demo.Services.Students
{
    public class StudentValidation
    {
        public bool CheckAgePercentageLimit(Student student)
        {
            return student.DateOfBirth >= DateTime.Now.AddYears(-20) || student.Percentage > 0.50;
        }
    }
}
