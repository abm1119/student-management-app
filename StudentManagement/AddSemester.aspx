<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSemester.aspx.cs" Inherits="StudentManagement.AddSemester" MasterPageFile="~/MasterPage.Master" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add New Semester</h2>
     <div class="message-container">
        <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
    </div>

    <div class="form-group">
        <label for="txtSemesterName">Semester Name (e.g., Fall 2023, Spring 2024):</label>
        <asp:TextBox ID="txtSemesterName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
    </div>
    <asp:Button ID="btnAddSemester" runat="server" Text="Add Semester" OnClick="BtnAddSemester_Click" CssClass="btn btn-success" />

    <hr style="margin: 30px 0;" />
    <h2>Existing Semesters</h2>
    <asp:GridView ID="gvSemesters" runat="server" AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="SemesterID">
        <Columns>
            <asp:BoundField DataField="SemesterName" HeaderText="Semester Name" SortExpression="SemesterName" />
            <%-- Add Edit/Delete CommandFields if needed --%>
        </Columns>
         <EmptyDataTemplate>
            <div class="gridview-emptydatarow">No semesters found. Add semesters using the form above.</div>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
