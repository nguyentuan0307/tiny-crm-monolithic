using Serilog;
using System.Reflection;
using TinyCRM.API.Extensions;
using TinyCRM.API.Middleware;
using TinyCRM.Application.Helper.AutoMapper;
using TinyCRM.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
            .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog(dispose: true);
});

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorizations();

builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(TinyCrmAutoMapper)));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(UserAutoMapper)));

builder.Services.AddControllers();
builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var environment = app.Services.GetRequiredService<IWebHostEnvironment>();
//app.Services.MigrateDatabase();
app.UseCustomerExceptionHandler(environment);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("Started running the application at {DateTime}", DateTime.Now);

await app.SeedDataAsync();

app.Run();