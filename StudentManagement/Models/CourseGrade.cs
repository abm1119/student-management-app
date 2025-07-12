using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public class CourseGrade
    {
        public string CourseName { get; set; }
        public int CreditHours { get; set; }
        public string LetterGrade { get; set; }
        // Ensure GradeMapping.GetGradePoint is accessible (it should be if GradeMapping class and its method are public)
        public decimal GradePoints => GradeMapping.GetGradePoint(LetterGrade);
        public decimal QualityPoints => GradePoints * CreditHours;
    }
}