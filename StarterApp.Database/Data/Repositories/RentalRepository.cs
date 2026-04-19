using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rental>> GetAllAsync()
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Borrower)
            .ToListAsync();
    }

    public async Task<Rental?> GetByIdAsync(int id)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Borrower)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Rental>> GetByBorrowerIdAsync(int borrowerId)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Where(r => r.BorrowerId == borrowerId)
            .ToListAsync();
    }

    public async Task<List<Rental>> GetByItemIdAsync(int itemId)
    {
        return await _context.Rentals
            .Include(r => r.Borrower)
            .Where(r => r.ItemId == itemId)
            .ToListAsync();
    }

    public async Task<Rental> CreateAsync(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }
}
