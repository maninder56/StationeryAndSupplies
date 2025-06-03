using DataBaseContextLibrary;
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Services;
using System.Reflection.Metadata.Ecma335;
using Serilog; 

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Logging
builder.Host.UseSerilog((context, configuration)
    => configuration.ReadFrom.Configuration(
        context.Configuration.GetSection("Serilog").GetSection("Serilog"))); 

// Only needed if you are adding configuration manager manually
//builder.Configuration.AddUserSecrets<Program>();

string connectionString = builder.Configuration["ConnectionStrings:StationeryAndSuppliesDatabase"]
    ?? throw new InvalidOperationException("Failed to get connection string");

// get connection string from environment variables in production



// Add Database service
builder.Services.AddDbContext<StationeryAndSuppliesDatabaseContext>(options
    => options.UseMySQL(connectionString));

builder.Services.AddDatabaseServices();

// Add Razor page services


WebApplication app = builder.Build();



app.UseStaticFiles();

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();


// Configure the HTTP request pipeline.
//switch (app.Environment.IsDevelopment())
//{
//    case true:
//        //app.UseHttpLogging();
//        //app.UseDeveloperExceptionPage();
//        break;
//    case false:
//        //app.UseExceptionHandler();
//        break;
//}

app.MapGet("/", async (IProductInformationService p) => await p.GetAllCategoriesAsync());

app.Run();
