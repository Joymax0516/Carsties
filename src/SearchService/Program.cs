using System.Net;
using System.Text.Json.Serialization;
using MassTransit;
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
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint("search-auction-created", e=> {
            e.UseMessageRetry(r => r.Interval(5,5));
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});
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

