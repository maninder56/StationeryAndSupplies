using DataBaseContextLibrary;
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Services; 

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Logging


string connectionString = ""; 

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();

    // build connection string from usersecrets 
}

// get connection string from environment variables in production


// Add Database service
builder.Services.AddDbContext<StationeryAndSuppliesDatabaseContext>(options 
    => options.UseMySQL(connectionString));

builder.Services.AddDatabaseServices();

// Add Razor page services


WebApplication app = builder.Build();


// Configure the HTTP request pipeline.
switch (app.Environment.IsDevelopment())
{
    case true:
        //app.UseHttpLogging();
        //app.UseDeveloperExceptionPage();
        break;
    case false:
        //app.UseExceptionHandler();
        break;
}

app.MapGet("/", () => "Hello World!");

app.Run();
