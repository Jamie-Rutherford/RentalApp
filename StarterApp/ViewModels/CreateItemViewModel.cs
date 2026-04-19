using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : ObservableObject
{
    private readonly IItemRepository _itemRepository;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private decimal _dailyRate;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private string _location = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public CreateItemViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [RelayCommand]
    public async Task SaveItemAsync()
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description))
        {
            ErrorMessage = "Please fill in all required fields.";
            return;
        }

        if (DailyRate <= 0)
        {
            ErrorMessage = "Daily rate must be greater than zero.";
            return;
        }

        var item = new Item
        {
            Title = Title,
            Description = Description,
            DailyRate = DailyRate,
            Category = Category,
            Location = Location,
            OwnerId = 1 // We'll link to real logged-in user later
        };

        await _itemRepository.CreateAsync(item);
        await Shell.Current.GoToAsync("..");
    }
}
