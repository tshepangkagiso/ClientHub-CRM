//services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My CRM API", Version = "v1" });
    c.OperationFilter<SwaggerFileOperationFilter>();
});

builder.Services.AddCors(o =>
{
    o.AddPolicy("EmployeeApp", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "http://localhost:5000").AllowAnyHeader().AllowAnyMethod();
    });

    o.AddPolicy("ClientApp", policy =>
    {
        policy.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthenticator, Authenticator>();
builder.Services.AddSingleton<IImageProcessor, ImageProcessor>();
builder.Services.AddSingleton<IPasswordEncryption, PasswordEncryption>();



//Middleware
var app = builder.Build();

app.UseCors("EmployeeApp");
app.UseCors("ClientApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    { 
        c.RoutePrefix = "swagger"; 
    });
}


app.UseHttpsRedirection();
app.MapControllers();


app.Run();


