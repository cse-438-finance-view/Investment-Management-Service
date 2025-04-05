using InvestmentManagementService;
using InvestmentManagementService.Infrastructure.Converters;
using InvestmentManagementService.Infrastructure.Swagger;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Listen(IPAddress.Any, 80);
//    serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
//    {
//        listenOptions.UseHttps(options =>
//        {
//            options.CheckCertificateRevocation = false;
//            options.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.NoCertificate;
//        });
//    });
//});

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: true, reloadOnChange: true);
}

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyNullableJsonConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Investment Management API", Version = "v1" });
    options.SchemaFilter<DateOnlySchemaFilter>();
});
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// HTTPS yönlendirmesi
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
