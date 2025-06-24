// Start of file
using TestApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using testApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure and register the SQLite database context
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlite("Data Source=testapp.db"));

// Register distributed memory cache to store session data
builder.Services.AddDistributedMemoryCache();

// Register session services to enable server-side sessions
builder.Services.AddSession();

// Register IHttpContextAccessor to allow access to HttpContext in services and controllers
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add MVC services for controllers and Razor views
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Serve static files (e.g., CSS, JS, images) from wwwroot
app.UseStaticFiles();

// Enable routing 
app.UseRouting();

// Enable session  to read/write session data
app.UseSession();

// Configure the default controller route: /{controller=Home}/{action=Index}/{id?}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Start listening for incoming HTTP requests
app.Run();
// End of file
