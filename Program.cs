using EduTrack.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Changed from AddControllers to include MVC support

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EduTrack API", Version = "v1" });
});

// Register services as singletons to maintain in-memory state
builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EduTrack API v1"));
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Added for serving static files (CSS, JS, images)

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map both API and MVC controllers
app.MapControllers(); // For API controllers

// Add default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
