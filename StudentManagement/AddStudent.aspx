<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddStudent.aspx.cs" Inherits="StudentManagement.AddStudent" MasterPageFile="~/MasterPage.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add New Student</h2>
    <div class="message-container">
        <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
    </div>
    
    <div class="form-group">
        <label for="txtName">Student Name:</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtRollNumber">Roll Number:</label>
        <asp:TextBox ID="txtRollNumber" runat="server" CssClass="form-control" required="required"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtEmail">Email:</label>
        <asp:TextBox ID="txtEmail" runat="server" type="email" CssClass="form-control"></asp:TextBox>
    </div>
    <asp:Button ID="btnAddStudent" runat="server" Text="Add Student" OnClick="BtnAddStudent_Click" CssClass="btn btn-success" />

    <hr style="margin: 30px 0;" />
    <h2>Existing Students</h2>
    <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False" CssClass="gridview"
        DataKeyNames="StudentID" OnRowCommand="GvStudents_RowCommand" OnRowDeleting="GvStudents_RowDeleting" OnRowEditing="GvStudents_RowEditing" OnRowUpdating="GvStudents_RowUpdating" OnRowCancelingEdit="GvStudents_RowCancelingEdit">
        <Columns>
            <asp:BoundField DataField="RollNumber" HeaderText="Roll Number" SortExpression="RollNumber" ReadOnly="true" />
            <asp:TemplateField HeaderText="Student Name" SortExpression="StudentName">
                <ItemTemplate><%# Eval("StudentName") %></ItemTemplate>
                <EditItemTemplate><asp:TextBox ID="txtEditStudentName" runat="server" Text='<%# Bind("StudentName") %>' CssClass="form-control"></asp:TextBox></EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email" SortExpression="Email">
                 <ItemTemplate><%# Eval("Email") %></ItemTemplate>
                 <EditItemTemplate><asp:TextBox ID="txtEditEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" type="email"></asp:TextBox></EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="true" EditText="Edit" ControlStyle-CssClass="btn btn-secondary btn-sm" HeaderText="Actions" />
            <asp:CommandField ShowDeleteButton="true" DeleteText="Delete" ControlStyle-CssClass="btn btn-danger btn-sm" />
        </Columns>
        <EmptyDataTemplate>
            <div class="gridview-emptydatarow">No students found. Add students using the form above.</div>
        </EmptyDataTemplate>
    </asp:GridView>
    <style>.btn-sm { padding: 5px 10px; font-size: 0.9em; margin-right: 5px; }</style>
</asp:Content>
