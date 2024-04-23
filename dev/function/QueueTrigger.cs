using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace function
{
    public class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        public void Run([QueueTrigger("queuemmrsboxwestus001", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
