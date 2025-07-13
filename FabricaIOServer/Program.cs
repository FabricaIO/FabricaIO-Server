using FabricaIOServer.FabricaIOLib;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
string databasePath = Path.Join(builder.Environment.WebRootPath, "database", "device.db");
string connectionString = $"Data Source={databasePath};";
Console.WriteLine(connectionString);
builder.Services.AddDbContext<DeviceContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.MapOpenApi();
app.UseSwaggerUi(options =>
{
    options.DocumentPath = "/openapi/v1.json";
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Start the background thread to scape repositories
DbContextOptionsBuilder<DeviceContext> optionsBuilder = new DbContextOptionsBuilder<DeviceContext>();
optionsBuilder.UseSqlite(connectionString);
RepoScrapper scrapper = new(new DeviceContext(optionsBuilder.Options));
Thread backgroundScrapper = new Thread(new ThreadStart(scrapper.scrapperLoop));
backgroundScrapper.Start();

app.Run();
