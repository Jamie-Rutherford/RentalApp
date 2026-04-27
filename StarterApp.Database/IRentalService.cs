namespace StarterApp.Database.Services;

public interface IRentalService
{
    Task<bool> RequestRentalAsync(int itemId, int borrowerId, DateTime startDate, DateTime endDate);
    Task<bool> ApproveRentalAsync(int rentalId);
    Task<bool> RejectRentalAsync(int rentalId);
}
