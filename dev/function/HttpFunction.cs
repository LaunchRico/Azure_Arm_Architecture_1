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
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnection");

            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();
                var newTableName = "TestTable";

                var createTableCmdText = $@"
                    IF NOT EXISTS (
                        SELECT * 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_SCHEMA = 'dbo' 
                        AND TABLE_NAME = '{newTableName}')
                    BEGIN
                        CREATE TABLE dbo.{newTableName} (
                            ID INT IDENTITY(1,1) PRIMARY KEY,
                            Column1 NVARCHAR(MAX)
                        )
                    END";

                using (SqlCommand createTableCMD = new SqlCommand(createTableCmdText, conn))
                {
                    createTableCMD.ExecuteNonQuery();
                }
            }
            return new OkObjectResult("Table created");
        }
    }
}
