var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IWebApiExecutor, WebApiExecutor>();

builder.Services.AddHttpClient("CRM_API", c =>
{
    c.BaseAddress = new Uri("http://localhost:4000/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();       // Must be before routing
app.UseRouting();

app.UseAuthorization(); // Optional if using [SessionAuthorize]

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

