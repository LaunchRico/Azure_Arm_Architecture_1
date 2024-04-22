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

app.Run();