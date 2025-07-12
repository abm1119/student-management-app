using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentManagement
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            try
            {
                // 1) Total Students
                var dtStudents = DatabaseManager.GetStudents();
                lblTotalStudents.Text = (dtStudents?.Rows.Count ?? 0).ToString();

                // 2) Total Courses
                var dtCourses = DatabaseManager.GetCourses();
                lblTotalCourses.Text = (dtCourses?.Rows.Count ?? 0).ToString();

                // 3) Average CGPA from grouped distribution
                var dtCgpas = DatabaseManager.GetOverallCGPADistribution();
                lblAverageCGPA.Text = CalculateAverageCGPA(dtCgpas);
            }
            catch (Exception ex)
            {
                // On error, display placeholders and log for diagnostics
                lblTotalStudents.Text = "Error";
                lblTotalCourses.Text = "Error";
                lblAverageCGPA.Text = "Error";

                System.Diagnostics.Debug.WriteLine(
                    $"[Dashboard] Error loading stats: {ex.GetType().Name} – {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Computes weighted average CGPA given a DataTable with columns:
        ///   - "CGPA" (decimal)
        ///   - "NumberOfStudents" (int)
        /// </summary>
        private string CalculateAverageCGPA(DataTable distribution)
        {
            if (distribution?.Rows.Count == 0)
                return "0.00";

            decimal weightedSum = 0;
            int studentCount = 0;

            foreach (DataRow row in distribution.Rows)
            {
                if (row["CGPA"] != DBNull.Value && row["NumberOfStudents"] != DBNull.Value)
                {
                    var cgpa = Convert.ToDecimal(row["CGPA"]);
                    var count = Convert.ToInt32(row["NumberOfStudents"]);

                    weightedSum += cgpa * count;
                    studentCount += count;
                }
            }

            return studentCount > 0
                ? (weightedSum / studentCount).ToString("N2")
                : "0.00";
        }
    }
}
    