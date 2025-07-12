using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public class CGPACalculator
    {
        public static decimal CalculateSGPA(List<CourseGrade> courses) // Ensure method is PUBLIC
        {
            if (courses == null || !courses.Any()) return 0.0m;

            // Filter out courses that might not count towards GPA based on their grade points.
            // This logic assumes that GradeMapping.GetGradePoint returns 0 for grades like 'W' (Withdrawal) or 'I' (Incomplete)
            // and these should not be included in GPA calculation's credit hours or quality points.
            // 'F' grades typically have 0 grade points but ARE included in GPA calculation.
            var gpaCourses = courses.Where(c => {
                // Check if the grade is explicitly 'F' or has points > 0.
                // This ensures 'F' is included, while other 0-point grades (like 'W', 'I' if configured that way) are excluded.
                // This might need adjustment based on how your GradeMapping handles non-GPA grades.
                bool isFGrate = c.LetterGrade.Equals("F", StringComparison.OrdinalIgnoreCase);
                bool hasPoints = GradeMapping.GetGradePoint(c.LetterGrade) > 0;
                return isFGrate || hasPoints;
            }).ToList();


            decimal totalQualityPoints = gpaCourses.Sum(c => c.QualityPoints);
            int totalCreditHoursAttempted = gpaCourses.Sum(c => c.CreditHours);

            if (totalCreditHoursAttempted == 0) return 0.0m;

            return Math.Round(totalQualityPoints / totalCreditHoursAttempted, 2);
        }

        public static decimal CalculateCGPA(List<List<CourseGrade>> allSemesterCourses) // Ensure method is PUBLIC
        {
            if (allSemesterCourses == null || !allSemesterCourses.Any()) return 0.0m;

            decimal totalQualityPointsOverall = 0;
            int totalCreditHoursOverall = 0;

            foreach (var semesterCourses in allSemesterCourses)
            {
                var gpaCoursesInSemester = semesterCourses.Where(c => {
                    bool isFGrate = c.LetterGrade.Equals("F", StringComparison.OrdinalIgnoreCase);
                    bool hasPoints = GradeMapping.GetGradePoint(c.LetterGrade) > 0;
                    return isFGrate || hasPoints;
                }).ToList();

                totalQualityPointsOverall += gpaCoursesInSemester.Sum(c => c.QualityPoints);
                totalCreditHoursOverall += gpaCoursesInSemester.Sum(c => c.CreditHours);
            }

            if (totalCreditHoursOverall == 0) return 0.0m;

            return Math.Round(totalQualityPointsOverall / totalCreditHoursOverall, 2);
        }
    }
}