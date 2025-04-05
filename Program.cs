using InvestmentManagementService;

var builder = WebApplication.CreateBuilder(args);

// Configure for Docker environment
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: true, reloadOnChange: true);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
