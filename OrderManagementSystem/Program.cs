using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Entities;
using OrderManagementSystem.Middlewares;
using OrderManagementSystem.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddControllers();
//1) rejestruje baze danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



//musze skonfigurować identity 
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


var app = builder.Build();
//3 w ten sposób dodaje middleware
app.UseMiddleware<SimpleMiddleware>();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();