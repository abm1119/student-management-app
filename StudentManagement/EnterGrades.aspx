<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnterGrades.aspx.cs" Inherits="StudentManagement.EnterGrades" MasterPageFile="~/MasterPage.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- Additional head content if needed --%>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Enter Grades for Student</h2>
    <div class="message-container">
        <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
    </div>

    <div class="form-row" style="display: flex; gap: 20px; margin-bottom: 20px;">
        <div class="form-group" style="flex: 1;">
            <label for="ddlStudent">Select Student (Roll Number):</label>
            <asp:DropDownList ID="ddlStudent" runat="server" CssClass="form-control" DataTextField="RollNumber" DataValueField="StudentID" AutoPostBack="true" OnSelectedIndexChanged="DdlStudent_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group" style="flex: 1;">
            <label for="ddlSemester">Select Semester:</label>
            <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-control" DataTextField="SemesterName" DataValueField="SemesterID" AutoPostBack="true" OnSelectedIndexChanged="DdlSemester_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    
    <asp:Panel ID="pnlAddGrade" runat="server" Visible="false">
        <hr />
        <h3>Add New Grade for: <asp:Label ID="lblSelectedStudentInfo" runat="server"></asp:Label></h3>
         <div class="form-row" style="display: flex; gap: 20px; align-items: flex-end;">
            <div class="form-group" style="flex: 2;">
                <label for="ddlCourse">Select Course:</label>
                <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-control" DataTextField="CourseName" DataValueField="CourseID"></asp:DropDownList>
            </div>
            <div class="form-group" style="flex: 1;">
                <label for="ddlLetterGrade">Letter Grade:</label>
                <asp:DropDownList ID="ddlLetterGrade" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group" style="flex: 0 0 auto;">
                 <asp:Button ID="btnAddGrade" runat="server" Text="Add Grade" OnClick="BtnAddGrade_Click" CssClass="btn btn-success" />
            </div>
        </div>
    </asp:Panel>
    
    <hr />
    <h3>Current Grades & GPA</h3>
    <asp:Panel ID="pnlGradesDisplay" runat="server" Visible="false">
        <asp:GridView ID="gvGrades" runat="server" AutoGenerateColumns="false" CssClass="gridview" ShowFooter="true"
            OnRowDataBound="GvGrades_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CourseName" HeaderText="Course" />
                <asp:BoundField DataField="CreditHours" HeaderText="Credits" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="LetterGrade" HeaderText="Grade" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="GradePoints" HeaderText="Points" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="QualityPoints" HeaderText="Quality Pts" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            </Columns>
            <EmptyDataTemplate>
                <div class="gridview-emptydatarow">No grades entered for this student in this semester yet.</div>
            </EmptyDataTemplate>
            <FooterStyle CssClass="gridview-footer" />
        </asp:GridView>
        <div style="margin-top: 20px; font-size: 1.2em; display:flex; justify-content:space-around; background-color: #e9ecef; padding:15px; border-radius:var(--border-radius);">
            <p><strong>SGPA (Semester GPA): <asp:Label ID="lblSGPA" runat="server" Text="0.00"></asp:Label></strong></p>
            <p><strong>CGPA (Cumulative GPA): <asp:Label ID="lblCGPA" runat="server" Text="0.00"></asp:Label></strong></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlNoSelection" runat="server" Visible="true">
        <p class="message info">Please select a student and a semester to view or add grades.</p>
    </asp:Panel>
</asp:Content>