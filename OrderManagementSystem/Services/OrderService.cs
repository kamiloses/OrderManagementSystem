using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Entities;

namespace OrderManagementSystem.Services;

public class OrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }


    public IEnumerable<Order> GetAllOrders()
    {
        return _context.Orders.AsNoTracking();
    }

    public IEnumerable<Order> GetAllByUserIdAsNoTracking(int userId)
    {
        return _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId)
            .ToList();
    }

    public Order Create(Order order)
    {
        try
        {
            var added = _context.Orders.Add(order).Entity;
            _context.SaveChanges();
            return added;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while saving the order.", ex);
        }
    }


    public bool Update(Order order)
    {
        //jeśli chcesz znaleźć obiekt po jego Id, wystarczy, że przekazujesz to Id jako argument w metodzie Find():
        try
        {
            var existingOrder = _context.Orders.Find(order.Id);

            if (existingOrder == null)
            {
                return false;
            }

            existingOrder.Status = order.Status;

            _context.SaveChanges();

            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while updating the order.", ex);
        }
    }

    public bool DeleteOrderById(string id)
    {
        var order = _context.Orders.Find(id);
        _context.Orders.Remove(order);

        // Zapisz zmiany w bazie danych
        var result = _context.SaveChanges();

        // Jeśli zapisano co najmniej jedną zmianę, zwróć true
        return result > 0;
    }

    // ==========================
    // ASYNC
    // ==========================


    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.AsNoTracking().ToListAsync();
    }


    public async Task<IEnumerable<Order>> GetAllByUserAsync(int userId)
    {
        return await _context.Orders.AsNoTracking().Where(t => t.UserId == userId).ToListAsync();
    }


    public async Task<Order> CreateAsync(Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();

            return order;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while saving the order.", ex);
        }
    }

    public async Task<bool> UpdateAsync(Order order)
    {
        try
        {
            Order? existingOrder = await _context.Orders.FindAsync(order.Id);

            if (existingOrder == null)
            {
                return false;
            }

            existingOrder.Status =existingOrder.Status;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the order.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingOrder = await _context.Orders
            .FirstOrDefaultAsync(t => t.Id == id);

        if (existingOrder == null)
            return false;

        _context.Orders.Remove(existingOrder);
        await _context.SaveChangesAsync();

        return true;

        
    }
}


// AS NO TRACKING TYLKO DO MODYFIKACJI PUT,POST itp , DOMYSLNIE JEST TRACKING
//EF pobiera dane i zapomina o nich.
//Ty możesz je czytać, ale EF nie zauważy, jeśli coś w nich zmienisz.
//   SaveChanges() nic nie zrobi, bo EF nie ma pojęcia, że coś się zmieniło.