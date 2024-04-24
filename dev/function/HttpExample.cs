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
        public String Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            //string name = req.Query["name"];
            //var connectionString = Environment.GetEnvironmentVariable("SqlConnection");
            var connectionString = "Server=tcp:sql-mmr-sbox-westus-001.database.windows.net,1433;Initial Catalog=db-mmr-sbox-westus-001;Persist Security Info=False;User ID=mmradministrator;Password=alBjFg8nen;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
            }
            return "Connection working";
        }
    }
}
