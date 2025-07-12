<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCourse.aspx.cs" Inherits="StudentManagement.AddCourse" MasterPageFile="~/MasterPage.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add New Course</h2>
    <div class="message-container">
        <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
    </div>
    
    <div class="form-group">
        <label for="txtCourseCode">Course Code:</label>
        <asp:TextBox ID="txtCourseCode" runat="server" CssClass="form-control" required="required"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtCourseName">Course Name:</label>
        <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtCreditHours">Credit Hours:</label>
        <asp:TextBox ID="txtCreditHours" runat="server" type="number" min="1" CssClass="form-control" required="required"></asp:TextBox>
    </div>
    <asp:Button ID="btnAddCourse" runat="server" Text="Add Course" OnClick="BtnAddCourse_Click" CssClass="btn btn-success" />

    <hr style="margin: 30px 0;" />
    <h2>Existing Courses</h2>
    <asp:GridView ID="gvCourses" runat="server" AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="CourseID">
        <Columns>
            <asp:BoundField DataField="CourseCode" HeaderText="Code" SortExpression="CourseCode" />
            <asp:BoundField DataField="CourseName" HeaderText="Name" SortExpression="CourseName" />
            <asp:BoundField DataField="CreditHours" HeaderText="Credits" SortExpression="CreditHours" />
            <%-- Add Edit/Delete CommandFields if needed, similar to AddStudent.aspx --%>
        </Columns>
         <EmptyDataTemplate>
            <div class="gridview-emptydatarow">No courses found. Add courses using the form above.</div>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>