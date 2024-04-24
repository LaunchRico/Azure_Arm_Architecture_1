using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace function
{
    public static class HttpFunction
    {
        [FunctionName("HttpFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            //string name = req.Query["name"];
            var connectionString = Environment.GetEnvironmentVariable("SqlConnection");
            var insertCmdText = $"INSERT INTO dbo.test (Column1) VALUES (@value)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                /*
                using (SqlCommand insertCmd = new SqlCommand(insertCmdText, conn))
                {
                    // Assuming 'message' holds the value to be inserted into the table
                    insertCmd.Parameters.AddWithValue("@value", name);
                    insertCmd.ExecuteNonQuery();
                }
                */
                return new OkObjectResult("Connection Open");
            }
        }
    }
}
