using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace LocalFunctionProj
{
    public class HttpExample
    {
        private readonly ILogger _logger;

        public HttpExample(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpExample>();
        }

        [Function("HttpExample")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            //string name = req.Query["name"];
            //var connectionString = Environment.GetEnvironmentVariable("SqlConnection");
            var connectionString = "Server=tcp:sql-mmr-sbox-westus-001.database.windows.net,1433;Initial Catalog=db-mmr-sbox-westus-001;Persist Security Info=False;User ID=mmradministrator;Password=alBjFg8nen;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;";
         
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(); // Use async opening
                    using (SqlCommand command = new SqlCommand("SELECT 1", connection))
                    {
                        var result = await command.ExecuteScalarAsync();
                        if (result != null)
                        {
                            return "Connection working and SQL command executed successfully.";
                        }
                        else
                        {
                            return "Connection working but SQL command did not return any results.";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return $"SQL Exception: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"General Exception: {ex.Message}";
            }
        }
    }
}
