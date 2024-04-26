using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var app = builder.Build();

app.MapGet("/", (IConfiguration config, string? value) => {
	string queueName = "queuemmrsboxwestus001";
	string? storageConnectionString = config["AzureWebJobsStorage"];
	if (storageConnectionString == null) 
	{
		return "No connection string";
	}
	QueueClient queueClient = new QueueClient(storageConnectionString, queueName, new QueueClientOptions
	{
		MessageEncoding = QueueMessageEncoding.Base64
	});
	string storeValue = value ?? "Default";
	queueClient.SendMessageAsync(storeValue).Wait();
	return "Message created";
});

app.Run();