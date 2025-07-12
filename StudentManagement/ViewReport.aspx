<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" EnableEventValidation="false" Inherits="StudentManagement.ViewReport" MasterPageFile="~/MasterPage.Master"  %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <h2>Academic Reports & Visualizations</h2>
    <div class="message-container">
        <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
    </div>

    <!-- CGPA Distribution -->
    <div class="report-section">
        <h3>Overall CGPA Distribution</h3>
        <asp:Button ID="btnLoadCgpaDistChart" runat="server" Text="Load CGPA Distribution Chart" OnClick="BtnLoadCgpaDistChart_Click" CssClass="btn btn-info" />
        <div style="max-width: 700px; margin: 20px auto;">
            <canvas id="cgpaDistChart"></canvas>
        </div>
    </div>

    <hr style="margin: 30px 0;" />

    <!-- Grade Distribution -->
    <div class="report-section">
        <h3>Grade Distribution per Course</h3>
        <div class="form-group" style="max-width:400px;">
            <label for="ddlCourseForReport">Select Course:</label>
            <asp:DropDownList ID="ddlCourseForReport" runat="server" CssClass="form-control" DataTextField="CourseName" DataValueField="CourseID"></asp:DropDownList>
        </div>
        <asp:Button ID="btnLoadCourseGradeDistChart" runat="server" Text="Load Course Grade Chart" OnClick="BtnLoadCourseGradeDistChart_Click" CssClass="btn btn-info" />
        <div style="max-width: 700px; margin: 20px auto;">
            <canvas id="courseGradeDistChart"></canvas>
        </div>
    </div>

    <hr style="margin: 30px 0;" />

    <!-- Semester-wise CGPA -->
    <div class="report-section">
        <h3>1. Semester-wise CGPA Distribution</h3>
        <asp:Button ID="btnSemesterCGPA" runat="server" Text="Generate Report" OnClick="BtnSemesterCGPA_Click" CssClass="btn btn-info" />
        <asp:Button ID="btnExportSemesterCGPA" runat="server" Text="Export to Excel" OnClick="BtnExportSemesterCGPA_Click" CssClass="btn btn-success" />
        <asp:GridView ID="gvSemesterCGPA" runat="server" CssClass="gridview" Visible="false">
            <EmptyDataTemplate>No semester-wise data available</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <!-- Subject-wise Performance -->
    <div class="report-section">
        <h3>2. Subject-wise Performance Analysis</h3>
        <asp:Button ID="btnSubjectWise" runat="server" Text="Generate Report" OnClick="BtnSubjectWise_Click" CssClass="btn btn-info" />
        <asp:Button ID="btnExportSubjectWise" runat="server" Text="Export to Excel" OnClick="BtnExportSubjectWise_Click" CssClass="btn btn-success" />
        <asp:GridView ID="gvSubjectWise" runat="server" CssClass="gridview" Visible="false">
            <EmptyDataTemplate>No subject-wise data available</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <!-- Detailed Student Academic Report -->
    <div class="report-section">
        <h3>3. Detailed Student Academic Report</h3>
        <asp:Button ID="btnDetailedReport" runat="server" Text="Generate Report" OnClick="BtnDetailedReport_Click" CssClass="btn btn-info" />
        <asp:Button ID="btnExportDetailed" runat="server" Text="Export to Word" OnClick="BtnExportDetailed_Click" CssClass="btn btn-success" />
        <asp:GridView ID="gvDetailedReport" runat="server" CssClass="gridview" Visible="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="GvDetailedReport_PageIndexChanging">
            <EmptyDataTemplate>No student records found</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <style>
        .report-section {
            margin-bottom: 40px;
            padding: 20px;
            background-color: #fff;
            border-radius: var(--border-radius);
            box-shadow: var(--box-shadow);
        }
    </style>
</asp:Content>