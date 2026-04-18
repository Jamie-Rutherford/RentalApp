namespace StarterApp.Database.Data.Repositories;

public interface IItemRepository
{
    Task<List<Database.Models.Item>> GetAllAsync();
    Task<Database.Models.Item?> GetByIdAsync(int id);
    Task<Database.Models.Item> CreateAsync(Database.Models.Item item);
    Task UpdateAsync(Database.Models.Item item);
    Task DeleteAsync(int id);
}
