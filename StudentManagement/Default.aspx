<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StudentManagement.Default" MasterPageFile="~/MasterPage.Master" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="dashboard-container">
        <h1 class="title">🎓 Academic Dashboard</h1>
        <p class="lead">Track student progress, manage records, and explore insightful stats.</p>
        <hr class="divider" />

        <div class="dashboard-grid">
            <div class="widget stat-box gradient-sky">
                <h3>Total Students</h3>
                <asp:Label ID="lblTotalStudents" runat="server" CssClass="stat-value" Text="0" />
            </div>
            <div class="widget stat-box gradient-blush">
                <h3>Total Courses</h3>
                <asp:Label ID="lblTotalCourses" runat="server" CssClass="stat-value" Text="0" />
            </div>
            <div class="widget stat-box gradient-mint">
                <h3>Average CGPA</h3>
                <asp:Label ID="lblAverageCGPA" runat="server" CssClass="stat-value" Text="N/A" />
            </div>
        </div>

        <div class="actions-panel">
            <h2>⚡ Quick Actions</h2>
            <div class="action-buttons">
                <a href="AddStudent.aspx" class="btn-accent">➕ Add New Student</a>
                <a href="EnterGrades.aspx" class="btn-accent">📝 Enter Grades</a>
                <a href="ViewReport.aspx" class="btn-accent">📊 View Reports</a>
            </div>
        </div>

       
    </div>

    <style>
        :root {
            --primary: #5468ff;
            --text: #2c2c2c;
            --muted-text: #6c757d;
            --bg: #f4f6fa;
            --card-radius: 16px;
        }

        body {
            background-color: var(--bg);
        }

        .dashboard-container {
            font-family: 'Segoe UI', sans-serif;
            padding: 40px 25px;
            color: var(--text);
            background-color: var(--bg);
        }

        .title {
            font-size: 2.5rem;
            font-weight: 700;
            margin-bottom: 5px;
            color: var(--text);
        }

        .lead {
            font-size: 1.2rem;
            margin-bottom: 30px;
            color: var(--muted-text);
        }

        .divider {
            border: none;
            height: 1px;
            background-color: #e0e0e0;
            margin-bottom: 40px;
        }

        .dashboard-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
            gap: 25px;
            margin-bottom: 50px;
        }

        .widget {
            border-radius: var(--card-radius);
            padding: 25px;
            box-shadow: 0 6px 18px rgba(0, 0, 0, 0.04);
            color: #fff;
            transition: transform 0.3s ease;
        }

        .widget:hover {
            transform: translateY(-3px);
        }

        .stat-box h3 {
            font-size: 1.1rem;
            font-weight: 600;
            margin-bottom: 8px;
        }

        .stat-value {
            font-size: 2.8rem;
            font-weight: 800;
            display: block;
        }

        /* Premium gradient cards */
        .gradient-sky {
            background: linear-gradient(135deg, #a1c4fd 0%, #c2e9fb 100%);
            color: #1a1a1a;
        }

        .gradient-blush {
            background: linear-gradient(135deg, #fbc2eb 0%, #a6c1ee 100%);
            color: #1a1a1a;
        }

        .gradient-mint {
            background: linear-gradient(135deg, #d4fc79 0%, #96e6a1 100%);
            color: #1a1a1a;
        }

        .actions-panel {
            background: #fff;
            padding: 30px;
            border-radius: var(--card-radius);
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        .actions-panel h2 {
            margin-bottom: 20px;
            color: var(--primary);
            font-size: 1.5rem;
        }

        .action-buttons {
            display: flex;
            flex-wrap: wrap;
            gap: 15px;
        }

        .btn-accent {
            background-color: var(--primary);
            color: #fff;
            padding: 12px 24px;
            border-radius: 10px;
            font-weight: 600;
            font-size: 1rem;
            text-decoration: none;
            transition: 0.2s ease;
        }

        .btn-accent:hover {
            background-color: #3f51d5;
            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
            transform: translateY(-2px);
        }

        .footer {
            margin-top: 60px;
            text-align: center;
            color: #999;
            font-size: 0.95rem;
        }

        .footer strong {
            color: var(--primary);
        }

        @media screen and (max-width: 768px) {
            .stat-value {
                font-size: 2.2rem;
            }

            .dashboard-container {
                padding: 25px 15px;
            }
        }
    </style>
</asp:Content>
