using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using System.Diagnostics;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
[QueryProperty(nameof(ItemTitle), "itemTitle")]
public partial class RequestRentalViewModel : ObservableObject
{
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private int _itemId;

    [ObservableProperty]
    private string _itemTitle = string.Empty;

    [ObservableProperty]
    private DateTime _startDate = DateTime.Today;

    [ObservableProperty]
    private DateTime _endDate = DateTime.Today.AddDays(1);

    [ObservableProperty]
    private bool _isSubmitting;

    public RequestRentalViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (EndDate <= StartDate)
        {
            await Shell.Current.DisplayAlert("Invalid Dates", "End date must be after start date.", "OK");
            return;
        }

        IsSubmitting = true;
        try
        {
            var startUtc = DateTime.SpecifyKind(StartDate, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(EndDate, DateTimeKind.Utc);
            var success = await _rentalService.RequestRentalAsync(ItemId, 1, startUtc, endUtc);
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Rental request submitted!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Unavailable", "This item is not available for the selected dates.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[RequestRentalViewModel] SubmitAsync error: {ex}");
            await Shell.Current.DisplayAlert("Error", "Failed to submit rental request.", "OK");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
