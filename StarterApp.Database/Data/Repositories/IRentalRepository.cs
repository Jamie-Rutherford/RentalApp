using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IRentalRepository
{
    Task<List<Rental>> GetAllAsync();
    Task<Rental?> GetByIdAsync(int id);
    Task<List<Rental>> GetByBorrowerIdAsync(int borrowerId);
    Task<List<Rental>> GetByItemIdAsync(int itemId);
    Task<Rental> CreateAsync(Rental rental);
    Task UpdateAsync(Rental rental);
}
