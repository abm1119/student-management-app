using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentManagement
{
    public partial class AddCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCourseGrid();
            }
        }

        private void BindCourseGrid()
        {
            gvCourses.DataSource = DatabaseManager.GetCourses();
            gvCourses.DataBind();
        }

        protected void BtnAddCourse_Click(object sender, EventArgs e)
        {
            string courseCode = txtCourseCode.Text.Trim();
            string courseName = txtCourseName.Text.Trim();
            int creditHours;

            if (string.IsNullOrEmpty(courseCode) || string.IsNullOrEmpty(courseName) || !int.TryParse(txtCreditHours.Text, out creditHours))
            {
                ShowMessage("All fields are required and Credit Hours must be a valid number.", "error");
                return;
            }
            if (creditHours <= 0)
            {
                ShowMessage("Credit Hours must be greater than zero.", "error");
                return;
            }

            bool success = DatabaseManager.AddCourse(courseCode, courseName, creditHours);
            if (success)
            {
                ShowMessage("Course added successfully!", "success");
                txtCourseCode.Text = "";
                txtCourseName.Text = "";
                txtCreditHours.Text = "";
                BindCourseGrid();
            }
            else
            {
                ShowMessage("Error adding course. Course Code might already exist.", "error");
            }
        }

        private void ShowMessage(string message, string type)
        {
            ltlMessage.Text = $"<div class='message {type}'>{Server.HtmlEncode(message)}</div>";
        }
    }
}