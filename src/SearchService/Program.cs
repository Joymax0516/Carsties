using System.Net;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Models;
using ZstdSharp.Unsafe;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

var app = builder.Build();

app.MapControllers();
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try{
        await DbInitializer.InitDb(app);
    }
    catch(Exception e)
    {
        Console.WriteLine(e);
    }
});
app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
   => HttpPolicyExtensions
     .HandleTransientHttpError()
     .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
     .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));

