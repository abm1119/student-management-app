using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentManagement
{
    public partial class ViewReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCoursesForReportDropdown();
            }
        }
        private void LoadCoursesForReportDropdown()
        {
            DataTable dtCourses = DatabaseManager.GetCourses();
            var courseItems = new List<ListItem>
            {
                new ListItem("-- Select Course --", "")
            };

            foreach (DataRow row in dtCourses.Rows)
            {
                courseItems.Add(new ListItem(
                    $"{row["CourseCode"]} - {row["CourseName"]}",
                    row["CourseID"].ToString()));
            }

            ddlCourseForReport.DataSource = courseItems;
            ddlCourseForReport.DataTextField = "Text";
            ddlCourseForReport.DataValueField = "Value";
            ddlCourseForReport.DataBind();
        }

        // Chart Handlers
        protected void BtnLoadCgpaDistChart_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCgpa = DatabaseManager.GetOverallCGPADistribution();
                if (dtCgpa.Rows.Count == 0)
                {
                    ShowMessage("No CGPA data available to generate chart.", "info");
                    ClearChart("cgpaDistChart");
                    return;
                }

                var labels = dtCgpa.AsEnumerable()
                    .Select(r => r.Field<decimal>("CGPA").ToString("N2"))
                    .ToList();
                var dataPoints = dtCgpa.AsEnumerable()
                    .Select(r => r.Field<int>("NumberOfStudents"))
                    .ToList();

                var serializer = new JavaScriptSerializer();
                string script = BuildChartScript(
                    "cgpaDistChart",
                    "bar",
                    "CGPA Distribution",
                    serializer.Serialize(labels),
                    serializer.Serialize(dataPoints),
                    "Number of Students"
                );

                ScriptManager.RegisterStartupScript(this, GetType(), "CgpaDistChartScript", script, true);
                ShowMessage("CGPA Distribution chart loaded.", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading CGPA chart: " + ex.Message, "error");
                ClearChart("cgpaDistChart");
            }
        }

        protected void BtnLoadCourseGradeDistChart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCourseForReport.SelectedValue))
            {
                ShowMessage("Please select a course to view its grade distribution.", "error");
                ClearChart("courseGradeDistChart");
                return;
            }

            try
            {
                int courseId = Convert.ToInt32(ddlCourseForReport.SelectedValue);
                DataTable dtGrades = DatabaseManager.GetGradeDistributionForCourse(courseId);
                if (dtGrades.Rows.Count == 0)
                {
                    ShowMessage($"No grade data available for {ddlCourseForReport.SelectedItem.Text}.", "info");
                    ClearChart("courseGradeDistChart");
                    return;
                }

                var labels = dtGrades.AsEnumerable()
                    .Select(r => r.Field<string>("LetterGrade"))
                    .ToList();
                var dataPoints = dtGrades.AsEnumerable()
                    .Select(r => r.Field<int>("NumberOfStudents"))
                    .ToList();

                var serializer = new JavaScriptSerializer();
                string script = BuildChartScript(
                    "courseGradeDistChart",
                    "pie",
                    $"Grade Distribution for {ddlCourseForReport.SelectedItem.Text}",
                    serializer.Serialize(labels),
                    serializer.Serialize(dataPoints),
                    "Number of Students"
                );

                ScriptManager.RegisterStartupScript(this, GetType(), "CourseGradeDistChartScript", script, true);
                ShowMessage("Grade distribution chart loaded.", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading course grade chart: " + ex.Message, "error");
                ClearChart("courseGradeDistChart");
            }
        }

        private void ClearChart(string canvasId)
        {
            string script = $@"
                var existing = Chart.getChart('{canvasId}');
                if(existing) existing.destroy();
                var ctx = document.getElementById('{canvasId}').getContext('2d');
                ctx.clearRect(0,0,ctx.canvas.width,ctx.canvas.height);
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), $"Clear_{canvasId}", script, true);
        }

        private string BuildChartScript(string canvasId, string chartType, string title, string labelsJson, string dataJson, string datasetLabel)
        {
            var colors = new[]
            {
                "rgba(54,162,235,0.7)", "rgba(75,192,192,0.7)", "rgba(255,206,86,0.7)",
                "rgba(255,159,64,0.7)", "rgba(153,102,255,0.7)", "rgba(255,99,132,0.7)"
            };
            var borders = colors.Select(c => c.Replace("0.7", "1")).ToArray();
            var serializer = new JavaScriptSerializer();

            return $@"
                var existing = Chart.getChart('{canvasId}'); if(existing) existing.destroy();
                var ctx = document.getElementById('{canvasId}').getContext('2d');
                new Chart(ctx, {{
                    type: '{chartType}',
                    data: {{ labels: {labelsJson}, datasets:[{{ label: '{datasetLabel}', data: {dataJson}, backgroundColor: {serializer.Serialize(colors)}, borderColor: {serializer.Serialize(borders)}, borderWidth:1 }}] }},
                    options: {{
                        plugins: {{ title: {{ display:true, text:'{title}' }} }},
                        scales: {{ y: {{ beginAtZero:true }} }}
                    }}
                }});
            ";
        }

        // New Report Handlers
        protected void BtnSemesterCGPA_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DatabaseManager.GetSemesterWiseCGPAs();
                gvSemesterCGPA.DataSource = dt;
                gvSemesterCGPA.DataBind();
                gvSemesterCGPA.Visible = true;
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading semester-wise CGPA data: " + ex.Message, "error");
            }
        }

        protected void BtnSubjectWise_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DatabaseManager.GetSubjectWisePerformance();
                gvSubjectWise.DataSource = dt;
                gvSubjectWise.DataBind();
                gvSubjectWise.Visible = true;
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading subject-wise data: " + ex.Message, "error");
            }
        }

        protected void BtnDetailedReport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DatabaseManager.GetDetailedStudentReport();
                gvDetailedReport.DataSource = dt;
                gvDetailedReport.DataBind();
                gvDetailedReport.Visible = true;
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading detailed report: " + ex.Message, "error");
            }
        }

        // Export Utilities
        private void ExportGridToExcel(GridView grid, string fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename={fileName}.xls");
            Response.Charset = string.Empty;
            Response.ContentType = "application/vnd.ms-excel";

            using (var sw = new StringWriter())
            {
                using (var hw = new HtmlTextWriter(sw))
                {
                    grid.AllowPaging = false;
                    grid.RenderControl(hw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }

        private void ExportGridToWord(GridView grid, string fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename={fileName}.doc");
            Response.Charset = string.Empty;
            Response.ContentType = "application/msword";

            using (var sw = new StringWriter())
            {
                using (var hw = new HtmlTextWriter(sw))
                {
                    grid.AllowPaging = false;
                    grid.RenderControl(hw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }

        // Export Handlers
        protected void BtnExportSemesterCGPA_Click(object sender, EventArgs e)
        {
            BtnSemesterCGPA_Click(sender, e);
            ExportGridToExcel(gvSemesterCGPA, "SemesterCGPAReport");
        }

        protected void BtnExportSubjectWise_Click(object sender, EventArgs e)
        {
            BtnSubjectWise_Click(sender, e);
            ExportGridToExcel(gvSubjectWise, "SubjectPerformanceReport");
        }

        protected void BtnExportDetailed_Click(object sender, EventArgs e)
        {
            BtnDetailedReport_Click(sender, e);
            ExportGridToWord(gvDetailedReport, "DetailedStudentReport");
        }

        protected void GvDetailedReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetailedReport.PageIndex = e.NewPageIndex;
            BtnDetailedReport_Click(sender, e);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required for export functionality
        }

        private void ShowMessage(string message, string type)
        {
            ltlMessage.Text = $"<div class='message {type}'>{Server.HtmlEncode(message)}</div>";
        }
    }
}