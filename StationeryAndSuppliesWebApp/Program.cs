using DataBaseContextLibrary;
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Services;
using System.Reflection.Metadata.Ecma335;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Logging
//builder.Host.UseSerilog((context, configuration)
//    => configuration.ReadFrom.Configuration(
//        context.Configuration.GetSection("Serilog").GetSection("Serilog"))); 

// Only needed if you are adding configuration manager manually
//builder.Configuration.AddUserSecrets<Program>();

string connectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration["ConnectionStrings:StationeryAndSuppliesDatabase"]
        ?? throw new InvalidOperationException("Failed to get connection string");
}
else
{
    // get connection string from environment variables in production
    connectionString = builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Failed to get connection string");
}


// Add Database service
builder.Services.AddDbContext<StationeryAndSuppliesDatabaseContext>(options
    => options.UseMySQL(connectionString));

builder.Services.AddDatabaseServices();
builder.Services.AddAccountService();

// Add cookie authentication 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // change it later for production
        options.SlidingExpiration = true;

        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Account/AccessDenied";
        
        //options.SessionStore  need to use it later to store cart items in session
    });

// To access HttpContext for custom components
builder.Services.AddHttpContextAccessor();

// Add Razor page services
builder.Services.AddRazorPages();


WebApplication app = builder.Build();

//Add support to logging request with SERILOG
//app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection(); 
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/MissingPage");

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets(); 

app.Run();
