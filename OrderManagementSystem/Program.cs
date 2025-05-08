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
//1 AddIdentity<User, IdentityRole<int>> – dodaje obsługę użytkowników (User) i ról (IdentityRole<int>) do aplikacji.
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()//ApplicationDbContext będzie zródłme danych do przechowywania encji
    .AddDefaultTokenProviders();//Dodaje tokeny potrzebne np. do: resetu hasła,


builder.Services.ConfigureApplicationCookie(options =>
{//w ten sposób jeżeli uzytkownik nie jest zalogowany lub nie  ma dostępu to zostanie przeniesiony na te 2 endpointy w zaleznosci jak w spring security
    options.LoginPath = "/api/user/login";
    options.AccessDeniedPath = "/api/user/denied";
});

var app = builder.Build();
//3 w ten sposób dodaje middleware
app.UseMiddleware<SimpleMiddleware>();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();