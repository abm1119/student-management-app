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
    public partial class EnterGrades : System.Web.UI.Page
    {

        private decimal totalSemesterCredits = 0;
        private decimal totalSemesterQualityPoints = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudents();
                LoadSemesters();
                LoadCourses();
                LoadLetterGrades();
                UpdatePanelsVisibility();
            }
        }

        private void LoadStudents()
        {
            DataTable dtStudents = DatabaseManager.GetStudents();
            ddlStudent.DataSource = dtStudents;
            // Display RollNumber - StudentName for better identification
            // We need to add a combined column to the DataTable or iterate
            List<ListItem> studentItems = new List<ListItem>();
            studentItems.Add(new ListItem("-- Select Student --", ""));
            foreach (DataRow row in dtStudents.Rows)
            {
                studentItems.Add(new ListItem($"{row["RollNumber"]} - {row["StudentName"]}", row["StudentID"].ToString()));
            }
            ddlStudent.DataSource = studentItems;
            ddlStudent.DataTextField = "Text";
            ddlStudent.DataValueField = "Value";
            ddlStudent.DataBind();
        }

        private void LoadSemesters()
        {
            ddlSemester.DataSource = DatabaseManager.GetSemesters();
            ddlSemester.DataTextField = "SemesterName";
            ddlSemester.DataValueField = "SemesterID";
            ddlSemester.DataBind();
            ddlSemester.Items.Insert(0, new ListItem("-- Select Semester --", ""));
        }

        private void LoadCourses()
        {
            DataTable dtCourses = DatabaseManager.GetCourses();
            List<ListItem> courseItems = new List<ListItem>();
            courseItems.Add(new ListItem("-- Select Course --", ""));
            foreach (DataRow row in dtCourses.Rows)
            {
                courseItems.Add(new ListItem($"{row["CourseCode"]} - {row["CourseName"]} ({row["CreditHours"]} Cr)", row["CourseID"].ToString()));
            }
            ddlCourse.DataSource = courseItems;
            ddlCourse.DataTextField = "Text";
            ddlCourse.DataValueField = "Value";
            ddlCourse.DataBind();
        }

        private void LoadLetterGrades()
        {
            ddlLetterGrade.DataSource = GradeMapping.GetValidLetterGradesForEntry();
            ddlLetterGrade.DataBind();
            ddlLetterGrade.Items.Insert(0, new ListItem("-- Select Grade --", ""));
        }

        protected void DdlStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleSelectionChange();
        }

        protected void DdlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleSelectionChange();
        }

        private void HandleSelectionChange()
        {
            UpdatePanelsVisibility();
            if (!string.IsNullOrEmpty(ddlStudent.SelectedValue) && !string.IsNullOrEmpty(ddlSemester.SelectedValue))
            {
                LoadStudentSemesterGrades();
                CalculateAndDisplayOverallCGPA();
                lblSelectedStudentInfo.Text = $"{ddlStudent.SelectedItem.Text} for {ddlSemester.SelectedItem.Text}";
            }
            else
            {
                ClearGradesDisplay();
                lblSelectedStudentInfo.Text = "";
            }
        }

        private void UpdatePanelsVisibility()
        {
            bool selectionMade = !string.IsNullOrEmpty(ddlStudent.SelectedValue) && !string.IsNullOrEmpty(ddlSemester.SelectedValue);
            pnlAddGrade.Visible = selectionMade;
            pnlGradesDisplay.Visible = selectionMade;
            pnlNoSelection.Visible = !selectionMade;
        }

        private void ClearGradesDisplay()
        {
            gvGrades.DataSource = null;
            gvGrades.DataBind();
            lblSGPA.Text = "0.00";
            lblCGPA.Text = "0.00"; // CGPA should also be cleared or re-evaluated
        }

        protected void BtnAddGrade_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlStudent.SelectedValue) ||
                string.IsNullOrEmpty(ddlSemester.SelectedValue) ||
                string.IsNullOrEmpty(ddlCourse.SelectedValue) ||
                string.IsNullOrEmpty(ddlLetterGrade.SelectedValue))
            {
                ShowMessage("Please select student, semester, course, and grade.", "error");
                return;
            }

            int studentId = Convert.ToInt32(ddlStudent.SelectedValue);
            int semesterId = Convert.ToInt32(ddlSemester.SelectedValue);
            int courseId = Convert.ToInt32(ddlCourse.SelectedValue);
            string letterGrade = ddlLetterGrade.SelectedValue;

            bool success = DatabaseManager.AddGrade(studentId, courseId, semesterId, letterGrade);

            if (success)
            {
                ShowMessage("Grade added successfully!", "success");
                LoadStudentSemesterGrades();
                CalculateAndDisplayOverallCGPA();
                ddlCourse.SelectedIndex = 0; // Reset course and grade dropdowns
                ddlLetterGrade.SelectedIndex = 0;
            }
            else
            {
                ShowMessage("Error adding grade. It might already exist for this student, course, and semester.", "error");
            }
        }

        private void LoadStudentSemesterGrades()
        {
            if (string.IsNullOrEmpty(ddlStudent.SelectedValue) || string.IsNullOrEmpty(ddlSemester.SelectedValue))
            {
                ClearGradesDisplay();
                return;
            }

            int studentId = Convert.ToInt32(ddlStudent.SelectedValue);
            int semesterId = Convert.ToInt32(ddlSemester.SelectedValue);

            totalSemesterCredits = 0; // Reset for footer calculation
            totalSemesterQualityPoints = 0; // Reset for footer calculation

            List<CourseGrade> currentSemesterGrades = DatabaseManager.GetGradesForStudentSemester(studentId, semesterId);
            gvGrades.DataSource = currentSemesterGrades;
            gvGrades.DataBind();

            decimal sgpa = CGPACalculator.CalculateSGPA(currentSemesterGrades);
            lblSGPA.Text = sgpa.ToString("N2");
        }

        protected void GvGrades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CourseGrade grade = (CourseGrade)e.Row.DataItem;
                totalSemesterCredits += grade.CreditHours;
                totalSemesterQualityPoints += grade.QualityPoints;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Totals / SGPA";
                e.Row.Cells[0].ColumnSpan = 1; // Adjust if you have more columns before credits
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[1].Text = totalSemesterCredits.ToString(); // Total Credits
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                e.Row.Cells[3].Text = lblSGPA.Text; // SGPA (calculated already)
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                e.Row.Cells[4].Text = totalSemesterQualityPoints.ToString("N2"); // Total Quality Points
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        private void CalculateAndDisplayOverallCGPA()
        {
            if (string.IsNullOrEmpty(ddlStudent.SelectedValue))
            {
                lblCGPA.Text = "0.00";
                return;
            }
            int studentId = Convert.ToInt32(ddlStudent.SelectedValue);
            List<List<CourseGrade>> allGrades = DatabaseManager.GetAllGradesForStudent(studentId);
            decimal cgpa = CGPACalculator.CalculateCGPA(allGrades);
            lblCGPA.Text = cgpa.ToString("N2");
        }

        private void ShowMessage(string message, string type)
        {
            ltlMessage.Text = $"<div class='message {type}'>{Server.HtmlEncode(message)}</div>";
        }

    }
}