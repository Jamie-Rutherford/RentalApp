using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IItemRepository _itemRepository;

    public RentalService(IRentalRepository rentalRepository, IItemRepository itemRepository)
    {
        _rentalRepository = rentalRepository;
        _itemRepository = itemRepository;
    }

    public async Task<bool> RequestRentalAsync(int itemId, int borrowerId, DateTime startDate, DateTime endDate)
    {
        // Validate dates
        if (startDate < DateTime.Today || endDate <= startDate)
            return false;

        // Check for double booking
        var existingRentals = await _rentalRepository.GetByItemIdAsync(itemId);
        bool isDoubleBooked = existingRentals.Any(r =>
            r.Status != "Rejected" &&
            r.StartDate < endDate &&
            r.EndDate > startDate);

        if (isDoubleBooked)
            return false;

        // Calculate price
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == null) return false;

        int days = (endDate - startDate).Days;
        decimal totalPrice = item.DailyRate * days;

        var rental = new Rental
        {
            ItemId = itemId,
            BorrowerId = borrowerId,
            StartDate = startDate,
            EndDate = endDate,
            TotalPrice = totalPrice,
            Status = "Requested"
        };

        await _rentalRepository.CreateAsync(rental);
        return true;
    }

    public async Task<bool> ApproveRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental == null || rental.Status != "Requested")
            return false;

        rental.Status = "Approved";
        await _rentalRepository.UpdateAsync(rental);
        return true;
    }

    public async Task<bool> RejectRentalAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental == null || rental.Status != "Requested")
            return false;

        rental.Status = "Rejected";
        await _rentalRepository.UpdateAsync(rental);
        return true;
    }
}
