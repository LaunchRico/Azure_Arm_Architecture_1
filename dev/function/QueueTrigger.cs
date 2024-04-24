using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace function
{
    public class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        public static void Run([QueueTrigger("queuemmrsboxwestus001", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

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
        }
    }
}
