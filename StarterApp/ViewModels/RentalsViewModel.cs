using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace StarterApp.ViewModels;

public partial class RentalsViewModel : ObservableObject
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private ObservableCollection<Rental> _incomingRentals = new();

    [ObservableProperty]
    private ObservableCollection<Rental> _outgoingRentals = new();

    [ObservableProperty]
    private bool _isLoading;

    public RentalsViewModel(IRentalRepository rentalRepository, IRentalService rentalService)
    {
        _rentalRepository = rentalRepository;
        _rentalService = rentalService;
    }

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    [RelayCommand]
    public async Task LoadRentalsAsync()
    {
        HasError = false;
        ErrorMessage = string.Empty;
        IsLoading = true;
        try
        {
            var all = await _rentalRepository.GetAllAsync();
            IncomingRentals = new ObservableCollection<Rental>(
                all.Where(r => r.Item?.OwnerId == 1 && r.Status == "Requested"));
            var outgoing = await _rentalRepository.GetByBorrowerIdAsync(1);
            OutgoingRentals = new ObservableCollection<Rental>(outgoing);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"Failed to load rentals: {ex.Message}";
            Debug.WriteLine($"[RentalsViewModel] LoadRentalsAsync error: {ex}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task ApproveRental(int rentalId)
    {
        await _rentalService.ApproveRentalAsync(rentalId);
        await LoadRentalsAsync();
    }

    [RelayCommand]
    public async Task RejectRental(int rentalId)
    {
        await _rentalService.RejectRentalAsync(rentalId);
        await LoadRentalsAsync();
    }
}
