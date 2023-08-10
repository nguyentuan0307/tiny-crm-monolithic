using System.Reflection;
using System.Text.Json.Serialization;
using TinyCRM.API.Extensions;
using TinyCRM.API.Middleware;
using TinyCRM.Application.Helper.AutoMapper;
using TinyCRM.Infrastructure.Helper.AutoMapper;
using TinyCRM.Infrastructure.Logger;

var builder = WebApplication.CreateBuilder(args);

LoggerService.ConfigureLogger(builder.Configuration);

builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorizations();

builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.AddDatabase(builder.Configuration);
await builder.Services.ApplyMigrateAsync();

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(TinyCrmAutoMapper)));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(InfraAutoMapper)));

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

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