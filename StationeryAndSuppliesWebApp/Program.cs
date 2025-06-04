using DataBaseContextLibrary;
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Services;
using System.Reflection.Metadata.Ecma335;
using Serilog; 

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Logging
//builder.Host.UseSerilog((context, configuration)
//    => configuration.ReadFrom.Configuration(
//        context.Configuration.GetSection("Serilog").GetSection("Serilog"))); 

// Only needed if you are adding configuration manager manually
//builder.Configuration.AddUserSecrets<Program>();

// get connection string from environment variables in production
string connectionString = builder.Configuration["ConnectionStrings:StationeryAndSuppliesDatabase"]
    ?? throw new InvalidOperationException("Failed to get connection string");

// Add Database service
builder.Services.AddDbContext<StationeryAndSuppliesDatabaseContext>(options
    => options.UseMySQL(connectionString));

builder.Services.AddDatabaseServices();

// Add Razor page services
builder.Services.AddRazorPages();

WebApplication app = builder.Build();

//Add support to logging request with SERILOG
//app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
switch (app.Environment.IsDevelopment())
{
    case true:
        //app.UseHttpLogging();
        app.UseDeveloperExceptionPage();
        break;
    case false:
        app.UseStatusCodePagesWithReExecute("/{0}"); // need to add error page
        app.UseHsts();
        break;
}

app.UseHttpsRedirection(); 

// app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets(); 

app.Run();
