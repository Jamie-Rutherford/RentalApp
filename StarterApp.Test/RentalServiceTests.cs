using Moq;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Database.Services;
using Xunit;

namespace StarterApp.Test;

public class RentalServiceTests
{
    [Fact]
    public async Task RequestRentalAsync_PastStartDate_ReturnsFalse()
    {
        // Arrange
        var mockRentalRepo = new Mock<IRentalRepository>();
        var mockItemRepo = new Mock<IItemRepository>();
        var service = new RentalService(mockRentalRepo.Object, mockItemRepo.Object);

        var pastDate = DateTime.Today.AddDays(-1);
        var endDate = DateTime.Today.AddDays(2);

        // Act
        var result = await service.RequestRentalAsync(1, 1, pastDate, endDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RequestRentalAsync_EndDateBeforeStartDate_ReturnsFalse()
    {
        // Arrange
        var mockRentalRepo = new Mock<IRentalRepository>();
        var mockItemRepo = new Mock<IItemRepository>();
        var service = new RentalService(mockRentalRepo.Object, mockItemRepo.Object);

        var startDate = DateTime.Today.AddDays(3);
        var endDate = DateTime.Today.AddDays(1);

        // Act
        var result = await service.RequestRentalAsync(1, 1, startDate, endDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RequestRentalAsync_ValidDates_ReturnsTrue()
    {
        // Arrange
        var mockRentalRepo = new Mock<IRentalRepository>();
        var mockItemRepo = new Mock<IItemRepository>();

        var item = new Item
        {
            Id = 1,
            Title = "Test Drill",
            DailyRate = 10,
            OwnerId = 1
        };

        mockRentalRepo.Setup(r => r.GetByItemIdAsync(1))
            .ReturnsAsync(new List<Rental>());
        mockItemRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(item);
        mockRentalRepo.Setup(r => r.CreateAsync(It.IsAny<Rental>()))
            .ReturnsAsync((Rental r) => r);

        var service = new RentalService(mockRentalRepo.Object, mockItemRepo.Object);

        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(3);

        // Act
        var result = await service.RequestRentalAsync(1, 2, startDate, endDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ApproveRentalAsync_RequestedRental_ReturnsTrue()
    {
        // Arrange
        var mockRentalRepo = new Mock<IRentalRepository>();
        var mockItemRepo = new Mock<IItemRepository>();

        var rental = new Rental
        {
            Id = 1,
            Status = "Requested",
            ItemId = 1,
            BorrowerId = 1
        };

        mockRentalRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(rental);
        mockRentalRepo.Setup(r => r.UpdateAsync(It.IsAny<Rental>()))
            .Returns(Task.CompletedTask);

        var service = new RentalService(mockRentalRepo.Object, mockItemRepo.Object);

        // Act
        var result = await service.ApproveRentalAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RejectRentalAsync_RequestedRental_StatusBecomesRejected()
    {
        // Arrange
        var mockRentalRepo = new Mock<IRentalRepository>();
        var mockItemRepo = new Mock<IItemRepository>();

        var rental = new Rental
        {
            Id = 1,
            Status = "Requested",
            ItemId = 1,
            BorrowerId = 1
        };

        mockRentalRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(rental);
        mockRentalRepo.Setup(r => r.UpdateAsync(It.IsAny<Rental>()))
            .Returns(Task.CompletedTask);

        var service = new RentalService(mockRentalRepo.Object, mockItemRepo.Object);

        // Act
        await service.RejectRentalAsync(1);

        // Assert
        Assert.Equal("Rejected", rental.Status);
    }

    [Fact]
    public async Task RequestRentalAsync_DoubleBooking_ReturnsFalse()
    {
        // Arrange
        var mockRentalRepo = new Mock<IRentalRepository>();
        var mockItemRepo = new Mock<IItemRepository>();

        var existingRental = new Rental
        {
            Id = 1,
            Status = "Approved",
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(5)
        };

        mockRentalRepo.Setup(r => r.GetByItemIdAsync(1))
            .ReturnsAsync(new List<Rental> { existingRental });

        var service = new RentalService(mockRentalRepo.Object, mockItemRepo.Object);

        // Act - try to book overlapping dates
        var result = await service.RequestRentalAsync(1, 2,
            DateTime.Today.AddDays(2),
            DateTime.Today.AddDays(4));

        // Assert
        Assert.False(result);
    }
}
