using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace function
{
    public class QueueTrigger
    {
        private readonly ILogger<QueueTrigger> _logger;

        public QueueTrigger(ILogger<QueueTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(QueueTrigger))]
        public async Task Run([QueueTrigger("queuemmrsboxwestus001", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnection");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(); // Use async opening
                    // Prepare the SQL query to insert the message into the 'Name' column of the 'test' table
                    string insertQuery = "INSERT INTO test (Name) VALUES (@Name)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Use parameterization to safely include the message content
                        command.Parameters.AddWithValue("@Name", message.Body.ToString());

                        var result = await command.ExecuteNonQueryAsync(); // Execute the insert command
                    }
                }
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {
            } 
        }
    }
}
