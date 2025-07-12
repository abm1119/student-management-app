using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentManagement
{
    public partial class AddSemester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSemesterGrid();
            }
        }

        private void BindSemesterGrid()
        {
            gvSemesters.DataSource = DatabaseManager.GetSemesters();
            gvSemesters.DataBind();
        }

        protected void BtnAddSemester_Click(object sender, EventArgs e)
        {
            string semesterName = txtSemesterName.Text.Trim();
            if (string.IsNullOrEmpty(semesterName))
            {
                ShowMessage("Semester Name is required.", "error");
                return;
            }

            bool success = DatabaseManager.AddSemester(semesterName);
            if (success)
            {
                ShowMessage("Semester added successfully!", "success");
                txtSemesterName.Text = "";
                BindSemesterGrid();
            }
            else
            {
                ShowMessage("Error adding semester. It might already exist.", "error");
            }
        }

        private void ShowMessage(string message, string type)
        {
            ltlMessage.Text = $"<div class='message {type}'>{Server.HtmlEncode(message)}</div>";
        }


    }
}