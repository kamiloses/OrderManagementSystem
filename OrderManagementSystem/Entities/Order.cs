namespace OrderManagementSystem.Entities;

    public class Order
    {
        
        //        //TODO EF STORE PROC JESZCZE ZROB ORAZ INCLUDING
        //2 Eager   Lazy i Explicit Loading

        //  todo    Exception handling (np. global exception filter)
       

        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public int UserId { get; set; }


        public User? User { get; set; }
    }
