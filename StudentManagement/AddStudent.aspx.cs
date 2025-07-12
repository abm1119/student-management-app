using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentManagement
{
    public partial class AddStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStudentGrid();
            }
        }


        private void BindStudentGrid()
        {
            // Ensure DatabaseManager.GetStudents() is accessible and returns a DataTable
            gvStudents.DataSource = DatabaseManager.GetStudents();
            gvStudents.DataBind();
        }

        protected void BtnAddStudent_Click(object sender, EventArgs e)
        {
            string studentName = txtName.Text.Trim();
            string rollNumber = txtRollNumber.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(rollNumber))
            {
                ShowMessage("Student Name and Roll Number are required.", "error");
                return;
            }

            // Ensure DatabaseManager.AddStudent is accessible and handles database operations
            bool success = DatabaseManager.AddStudent(studentName, rollNumber, email);
            if (success)
            {
                ShowMessage("Student added successfully!", "success");
                txtName.Text = "";
                txtRollNumber.Text = "";
                txtEmail.Text = "";
                BindStudentGrid();
            }
            else
            {
                ShowMessage("Error adding student. Roll Number might already exist.", "error");
            }
        }

        protected void GvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            BindStudentGrid();
        }

        protected void GvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            BindStudentGrid();
        }

        protected void GvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvStudents.Rows[e.RowIndex];
                // Ensure DataKeyNames="StudentID" is set on the GridView in the ASPX markup
                int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
                // Ensure TextBoxes with these IDs exist in the EditItemTemplate of the GridView
                string studentName = ((TextBox)row.FindControl("txtEditStudentName")).Text.Trim();
                string email = ((TextBox)row.FindControl("txtEditEmail")).Text.Trim();

                // Ensure DatabaseManager.UpdateStudent is accessible and correctly updates the database
                bool success = DatabaseManager.UpdateStudent(studentId, studentName, email);

                if (success)
                {
                    ShowMessage("Student updated successfully!", "success");
                }
                else
                {
                    ShowMessage("Student not found or no changes made.", "info");
                }
                gvStudents.EditIndex = -1; // Exit edit mode
                BindStudentGrid(); // Rebind to show updated data
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating student: " + ex.Message, "error");
                // Log the full exception for debugging
                System.Diagnostics.Debug.WriteLine("Update Error in AddStudent.aspx.cs: " + ex.ToString());
            }
        }

        protected void GvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Ensure DataKeyNames="StudentID" is set on the GridView
                int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
                // Ensure DatabaseManager.DeleteStudent is accessible and correctly deletes from the database
                bool success = DatabaseManager.DeleteStudent(studentId);

                if (success)
                {
                    ShowMessage("Student deleted successfully!", "success");
                }
                else
                {
                    ShowMessage("Student not found or failed to delete.", "error");
                }
                BindStudentGrid(); // Rebind to reflect deletion
            }
            catch (SqlException sqlEx) // Catch specific database errors
            {
                // Check for foreign key constraint violation if a student has related records (e.g., grades)
                if (sqlEx.Message.ToLower().Contains("reference constraint") || sqlEx.Message.ToLower().Contains("foreign key"))
                {
                    ShowMessage("Cannot delete student. They have existing related records (e.g., grades). Please remove associated records first or ensure cascading deletes are set up in the database.", "error");
                }
                else
                {
                    ShowMessage("Database error deleting student: " + sqlEx.Message, "error");
                }
                // Log the full exception for debugging
                System.Diagnostics.Debug.WriteLine("SQL Delete Error in AddStudent.aspx.cs: " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting student: " + ex.Message, "error");
                // Log the full exception for debugging
                System.Diagnostics.Debug.WriteLine("General Delete Error in AddStudent.aspx.cs: " + ex.ToString());
            }
        }

        // This method handles custom commands if you add CommandField buttons with CommandName attributes
        // For example, if you had a button <asp:ButtonField CommandName="ViewDetails" ... />
        protected void GvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Example: Handle a custom command
            // if (e.CommandName == "ViewDetails")
            // {
            //     // The CommandArgument can be set in the ASPX markup, e.g., CommandArgument='<%# Eval("StudentID") %>'
            //     int studentId = Convert.ToInt32(e.CommandArgument);
            //     // Redirect to a details page or show details in a modal
            //     Response.Redirect($"StudentDetails.aspx?ID={studentId}");
            // }

            // Log that the command was fired for debugging purposes
            System.Diagnostics.Debug.WriteLine("GvStudents_RowCommand fired. CommandName: " + e.CommandName + ", Argument: " + (e.CommandArgument?.ToString() ?? "null"));
        }

        // Helper method to display messages to the user
        private void ShowMessage(string message, string type)
        {
            // ltlMessage should be an <asp:Literal> control in your AddStudent.aspx markup
            // The CSS classes 'message', 'success', 'error', 'info' should be defined in your site's stylesheet (e.g., in Site.Master)
            ltlMessage.Text = $"<div class='message {type}'>{Server.HtmlEncode(message)}</div>";
        }
    }
}
