using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AZDBFunctionPOST 
{
    public static class GetCourses
    {
        [FunctionName("GetCourses")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<Course> _lst = new List<Course>();
            string _statement = "SELECT courseid,coursename,rating from Course";
            string _connection_string = "Server=tcp:tks007dbserver.database.windows.net,1433;Initial Catalog=tks007DB;Persist Security Info=False;User ID=tks007;Password=Tksantra007))&;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection _connection = new SqlConnection(_connection_string);
            _connection.Open();
            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);
            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Course _course = new Course()
                    {
                        CourseID = _reader.GetInt32(0),
                        CourseName = _reader.GetString(1),
                        Rating = _reader.GetInt32(2)
                    };

                    _lst.Add(_course);
                }
            }
            _connection.Close();
            // Return the HTTP status code of 200 OK and the list of courses
            return new OkObjectResult(_lst);
        }
    }
}
