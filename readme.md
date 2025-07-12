# 🎓 Student Management Web App

A complete web-based application developed using **ASP.NET Web Forms/Core** in **Visual Studio 2022** to manage academic records of students in an educational institute. It is specifically designed for **Class Advisors** or **Class Teachers** to manage, monitor, and evaluate students throughout their academic semesters.

---

## 📽️ Demo
[Project Demo](https://www.playbook.com/s/computer-systems-engineeing/cUveKsnykAXH9XHjTppAuwDP?assetToken=ysdqT8vLdLbv2h1de2pYG6kg)  

---

## ✨ Features

- Add, update, and manage student records
- Assign students to courses (compulsory & optional)
- Track GPA and grade records for each course per student
- View student performance subject-wise and semester-wise
- Identify missing course enrollments
- Export academic records to Excel, Word, or PDF
- Filter and search students easily
- Basic UI animations and styles

---

## 🌿 Tech Stack

| Layer       | Technology               |
|-------------|---------------------------|
| Frontend    | ASP.NET Web Forms / Core |
| Backend     | C# (.NET Framework/Core) |
| Database    | SQL Server               |
| UI Controls | GridView, Chart Control  |
| Styling     | CSS, Bootstrap (optional) |
| Scripts     | JavaScript (`ui-animations.js`) |

---

## 📂 Project Structure

```
StudentManagement/
│
├── App_Data/                   # Database files
├── Models/                     # C# data models
├── ConnectedServices/          # Service references
├── Properties/                 # App properties
├── References/                 # Project references
│
├── AddCourse.aspx              # Add new courses
├── AddSemester.aspx            # Add semester info
├── AddStudent.aspx             # Add new students
├── Default.aspx                # Home/Dashboard
├── EnterGrades.aspx            # GPA/grade entry page
├── ViewReport.aspx             # Semester-wise reports
│
├── MasterPage.Master           # Shared layout
├── Site.css                    # Global styles
├── ui-animations.js            # JS for animations
├── Web.config                  # Configurations & DB connection
├── packages.config             # NuGet dependencies
├── WebAppico.ico               # Favicon
```

---

## 🚀 Getting Started

### 1. Clone the Repo
```bash
git clone https://github.com/abm1119/student-management-app.git
cd student-management-app
```

### 2. Open in Visual Studio 2022
- Open the solution file: `StudentManagement.sln`
- Restore any NuGet packages if prompted

### 3. Configure SQL Server
- Edit `Web.config` to set your connection string
```xml
<connectionStrings>
  <add name="StudentDB" connectionString="Data Source=.;Initial Catalog=StudentDB;Integrated Security=True" />
</connectionStrings>
```
- Run SQL scripts if provided inside `App_Data/`

### 4. Run the Application
- Press `F5` or run via IIS Express
- Visit: `http://localhost:[PORT]/Default.aspx`

---

## 📌 To Do 

- [ ] Add authentication and user roles
- [ ] Export reports to PDF
- [ ] Add search filters and performance charts
- [ ] Mobile responsive layout using Bootstrap
- [ ] Integrate Entity Framework (if not already)
- [ ] Add unit and integration testing

---

## 🧑‍💻 Author

**Abdul Basit Memon (ABM)**  
📧 [basitmemon67@gmail.com](mailto:basitmemon67@gmail.com)  
👤 [LinkedIn](https://pk.linkedin.com/in/abdul-basit-memon-614961166)  
👾 [GitHub](https://github.com/abm1119)

---

## 📃 License

This project is licensed under the [MIT License](LICENSE).

