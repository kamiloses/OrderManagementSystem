using Microsoft.AspNetCore.Identity;

namespace OrderManagementSystem.Entities;

public class User : IdentityUser<int>
{
    
    public ICollection<Order> Orders { get; set; }
    
    public int? Age{get;set;} 
    
}
