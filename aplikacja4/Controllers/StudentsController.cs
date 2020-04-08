using System.Data.SqlClient;
using aplikacja4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace aplikacja4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        string connectionString = "Data Source=db-mssql;Initial Catalog=s16478;Integrated Security=True";
       
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = new List<Student>();
            // zadanie 4.1
            using (var client = new SqlConnection(connectionString))   
            using (var command = new SqlCommand())
            {
                command.Connection = client;
                // -------------   start zadanie 4.2
                command.CommandText = "SELECT Student.FirstName, Student.LastName, Student.BirthDate, Enrollment.Semester, Studies.Name From Enrollment JOIN Student on Enrollment.IdEnrollment = Student.IdEnrollment JOIN Studies on Enrollment.IdStudy = Studies.IdStudy";
                client.Open();
                SqlDataReader dataReader = command.ExecuteReader();  // strumień typu forward only
                while (dataReader.Read())
                {
                    var st = new Student();
                    st.FirstName = dataReader["FirstName"].ToString();
                    st.LastName = dataReader["LastName"].ToString();
                    st.BirthDate = dataReader["BirthDate"].ToString().Replace(" 00:00:00", "").ToString();
                    st.Semester = (int)dataReader["Semester"];
                    st.Studies = dataReader["Name"].ToString();
                    students.Add(st);  // mam liste studentów sparsowaną do formatu JSON
                    // -------------   end zadanie 4.2
                }
            }
            return Ok(students);
        }


        [HttpGet("{IdStudy}")]
        public IActionResult GetStudent(string IndexNumber)
        {
            using (var client = new SqlConnection(connectionString))    // zadanie 4.1
            using (var command = new SqlCommand())
            {
                command.Connection = client;             
                command.CommandText = "SELECT Student.FirstName, Student.LastName, Enrollment.Semester, Student.IdEnrollment FROM Enrollment JOIN Student on Enrollment.IdEnrollment = Student.IdEnrollment WHERE Student.IndexNumber = '"+IndexNumber+"'";

                client.Open();
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    return Ok(st);
                }

            }


            return NotFound();
        }



    }
}