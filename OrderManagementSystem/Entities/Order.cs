using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Entities;

    public class Order
    {
        
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
      
         //DATA ANNOTATIONS
     //   [Required]
     //   [StringLength(50, MinimumLength = 2)]
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public int UserId { get; set; }


        public User? User { get; set; }
    }
