using Entities.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();



builder.Host.UseSerilog((
    HostBuilderContext context, 
    IServiceProvider services, 
    LoggerConfiguration loggerConfiguration)
    =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});


builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddDbContext<PersonsDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestProperties;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpLogging();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
