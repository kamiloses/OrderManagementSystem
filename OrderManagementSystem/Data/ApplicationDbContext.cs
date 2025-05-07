using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entities;
namespace OrderManagementSystem.Data;
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    
           //Fluent api
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<Order>()
            .HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId);
        
        
        
        //seedowanie
        // modelBuilder.Entity<User>().HasData(
        //     new User { Id = 1, UserName = "kamiloses", Email = "admin@example.com", NormalizedEmail = "ADMIN@EXAMPLE.COM", EmailConfirmed = true, PasswordHash = "adminpasswordhash" }
        // );
        //
        
        
    }
}