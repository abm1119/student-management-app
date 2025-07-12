using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public static class DatabaseManager
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["StudentAcademicDBConnectionString"].ConnectionString;
        }

        #region Student Methods
        public static bool AddStudent(string studentName, string rollNumber, string email)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string checkQuery = "SELECT COUNT(*) FROM Students WHERE RollNumber = @RollNumber";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@RollNumber", rollNumber);
                    con.Open();
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        con.Close();
                        return false;
                    }
                }

                string query = "INSERT INTO Students (StudentName, RollNumber, Email) VALUES (@StudentName, @RollNumber, @Email)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentName", studentName);
                    cmd.Parameters.AddWithValue("@RollNumber", rollNumber);
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);

                    if (con.State == ConnectionState.Closed) con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static DataTable GetStudents()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT StudentID, RollNumber, StudentName, Email FROM Students ORDER BY StudentName", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static bool UpdateStudent(int studentId, string studentName, string email)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "UPDATE Students SET StudentName = @StudentName, Email = @Email WHERE StudentID = @StudentID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentName", studentName);
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool DeleteStudent(int studentId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        #endregion

        #region Course Methods
        public static bool AddCourse(string courseCode, string courseName, int creditHours)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string checkQuery = "SELECT COUNT(*) FROM Courses WHERE CourseCode = @CourseCode";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@CourseCode", courseCode);
                    con.Open();
                    if ((int)checkCmd.ExecuteScalar() > 0) { con.Close(); return false; }
                }

                string query = "INSERT INTO Courses (CourseCode, CourseName, CreditHours) VALUES (@CourseCode, @CourseName, @CreditHours)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                    cmd.Parameters.AddWithValue("@CourseName", courseName);
                    cmd.Parameters.AddWithValue("@CreditHours", creditHours);
                    if (con.State == ConnectionState.Closed) con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static DataTable GetCourses()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT CourseID, CourseCode, CourseName, CreditHours FROM Courses ORDER BY CourseName", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        #endregion

        #region Semester Methods
        public static bool AddSemester(string semesterName)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string checkQuery = "SELECT COUNT(*) FROM Semesters WHERE SemesterName = @SemesterName";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@SemesterName", semesterName);
                    con.Open();
                    if ((int)checkCmd.ExecuteScalar() > 0) { con.Close(); return false; }
                }

                string query = "INSERT INTO Semesters (SemesterName) VALUES (@SemesterName)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SemesterName", semesterName);
                    if (con.State == ConnectionState.Closed) con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static DataTable GetSemesters()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT SemesterID, SemesterName FROM Semesters ORDER BY SemesterID DESC", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        #endregion

        #region Grade Methods
        public static bool AddGrade(int studentId, int courseId, int semesterId, string letterGrade)
        {
            decimal gradePoints = GradeMapping.GetGradePoint(letterGrade);
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "INSERT INTO Grades (StudentID, CourseID, SemesterID, LetterGrade, GradePoints) VALUES (@StudentID, @CourseID, @SemesterID, @LetterGrade, @GradePoints)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.Parameters.AddWithValue("@SemesterID", semesterId);
                    cmd.Parameters.AddWithValue("@LetterGrade", letterGrade);
                    cmd.Parameters.AddWithValue("@GradePoints", gradePoints);

                    con.Open();
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            System.Diagnostics.Debug.WriteLine("Attempted to insert duplicate grade: " + ex.Message);
                            return false;
                        }
                        throw;
                    }
                }
            }
        }

        public static List<CourseGrade> GetGradesForStudentSemester(int studentId, int semesterId)
        {
            List<CourseGrade> grades = new List<CourseGrade>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = @"
                    SELECT C.CourseName, C.CreditHours, G.LetterGrade 
                    FROM Grades G
                    JOIN Courses C ON G.CourseID = C.CourseID
                    WHERE G.StudentID = @StudentID AND G.SemesterID = @SemesterID
                    ORDER BY C.CourseName";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@SemesterID", semesterId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        grades.Add(new CourseGrade
                        {
                            CourseName = reader["CourseName"].ToString(),
                            CreditHours = Convert.ToInt32(reader["CreditHours"]),
                            LetterGrade = reader["LetterGrade"].ToString()
                        });
                    }
                }
            }
            return grades;
        }

        public static List<List<CourseGrade>> GetAllGradesForStudent(int studentId)
        {
            var allSemesterCourses = new List<List<CourseGrade>>();
            var semesterIds = new List<int>();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string semesterQuery = "SELECT DISTINCT SemesterID FROM Grades WHERE StudentID = @StudentID ORDER BY SemesterID";
                using (SqlCommand cmd = new SqlCommand(semesterQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        semesterIds.Add(Convert.ToInt32(reader["SemesterID"]));
                    }
                }
            }

            foreach (int semesterId in semesterIds)
            {
                allSemesterCourses.Add(GetGradesForStudentSemester(studentId, semesterId));
            }
            return allSemesterCourses;
        }
        #endregion

        #region Reporting/Statistics Data Methods
        public static DataTable GetOverallCGPADistribution()
        {
            DataTable dt = new DataTable();
            string query = @"
                WITH StudentCGPAs AS (
                    SELECT 
                        S.StudentID,
                        S.StudentName,
                        S.RollNumber,
                        CASE 
                            WHEN SUM(C.CreditHours) = 0 THEN 0
                            ELSE ROUND(SUM(G.GradePoints * C.CreditHours) * 1.0 / SUM(C.CreditHours), 2)
                        END AS CGPA
                    FROM Students S
                    LEFT JOIN Grades G ON S.StudentID = G.StudentID
                    LEFT JOIN Courses C ON G.CourseID = C.CourseID
                    GROUP BY S.StudentID, S.StudentName, S.RollNumber
                )
                SELECT CGPA, COUNT(*) AS NumberOfStudents
                FROM StudentCGPAs
                WHERE CGPA IS NOT NULL
                GROUP BY CGPA
                ORDER BY CGPA DESC;";
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetGradeDistributionForCourse(int courseId)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT LetterGrade, COUNT(*) AS NumberOfStudents
                FROM Grades
                WHERE CourseID = @CourseID
                GROUP BY LetterGrade
                ORDER BY LetterGrade;";
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        #endregion

        #region Enhanced Reporting Methods
        public static DataTable GetSemesterWiseCGPAs()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    S.SemesterName,
                    CASE 
                        WHEN CGPA >= 3.67 THEN 'A Grade (3.67-4.00)'
                        WHEN CGPA >= 3.00 THEN 'B Grade (3.00-3.66)'
                        WHEN CGPA >= 2.00 THEN 'C Grade (2.00-2.99)'
                        WHEN CGPA >= 1.00 THEN 'D Grade (1.00-1.99)'
                        ELSE 'F Grade (Below 1.00)'
                    END AS CGPAGroup,
                    COUNT(*) AS NumberOfStudents,
                    AVG(CGPA) AS AverageCGPA
                FROM (
                    SELECT 
                        G.SemesterID,
                        S.StudentID,
                        SUM(G.GradePoints * C.CreditHours) / NULLIF(SUM(C.CreditHours), 0) AS CGPA
                    FROM Grades G
                    JOIN Courses C ON G.CourseID = C.CourseID
                    JOIN Students S ON G.StudentID = S.StudentID
                    GROUP BY G.SemesterID, S.StudentID
                ) AS StudentCGPAs
                JOIN Semesters S ON StudentCGPAs.SemesterID = S.SemesterID
                GROUP BY S.SemesterName, 
                    CASE 
                        WHEN CGPA >= 3.67 THEN 'A Grade (3.67-4.00)'
                        WHEN CGPA >= 3.00 THEN 'B Grade (3.00-3.66)'
                        WHEN CGPA >= 2.00 THEN 'C Grade (2.00-2.99)'
                        WHEN CGPA >= 1.00 THEN 'D Grade (1.00-1.99)'
                        ELSE 'F Grade (Below 1.00)'
                    END
                ORDER BY S.SemesterName, CGPAGroup";

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetSubjectWisePerformance()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    C.CourseCode,
                    C.CourseName,
                    AVG(G.GradePoints) AS AverageGrade,
                    COUNT(DISTINCT G.StudentID) AS NumberOfStudents,
                    COUNT(CASE WHEN G.LetterGrade = 'F' THEN 1 END) AS Failures
                FROM Grades G
                JOIN Courses C ON G.CourseID = C.CourseID
                GROUP BY C.CourseCode, C.CourseName
                ORDER BY AverageGrade DESC";

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetDetailedStudentReport()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    ST.RollNumber,
                    ST.StudentName,
                    SEM.SemesterName,
                    C.CourseCode,
                    C.CourseName,
                    G.LetterGrade,
                    G.GradePoints,
                    C.CreditHours,
                    (G.GradePoints * C.CreditHours) AS QualityPoints
                FROM Grades G
                JOIN Students ST ON G.StudentID = ST.StudentID
                JOIN Courses C ON G.CourseID = C.CourseID
                JOIN Semesters SEM ON G.SemesterID = SEM.SemesterID
                ORDER BY ST.RollNumber, SEM.SemesterID, C.CourseCode";

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        #endregion
    }
}
