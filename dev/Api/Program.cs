using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var app = builder.Build();

app.MapGet("/", (IConfiguration config) => {
	string? storageConnectionString = config["AzureWebJobsStorage"];
	if (storageConnectionString == null) 
	{
		return "jeu";
	}
	return storageConnectionString;
});

//app.MapGet("/insertmessage", insertMessage);

app.Run();

/*
string getConnectionString()
{
	var connectionString = builder.Configuration.GetConnectionString("StorageConnection");
    return connectionString;
}

QueueClient getQueueClient()
{
	var connectionString = getConnectionString();
	var queueName = "queuemmrsboxwestus001";
	var queueClient = new QueueClient(connectionString, queueName);
	return queueClient;
}

async Task<string> insertMessage(HttpContext context)
{
	var queryParams = context.Request.Query;

	if (queryParams.ContainsKey("message"))
	{
		var message = queryParams["message"];
		var queueClient = getQueueClient();
		await queueClient.CreateIfNotExistsAsync();
		await queueClient.SendMessageAsync(message);
		return "Message inserted";
	}
	else
	{
		return "Message not found";
	}
}
*/