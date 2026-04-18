using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : ObservableObject
{
    private readonly IItemRepository _itemRepository;

    [ObservableProperty]
    private ObservableCollection<Item> _items = new();

    [ObservableProperty]
    private bool _isLoading;

    public ItemsListViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [RelayCommand]
    public async Task LoadItemsAsync()
    {
        IsLoading = true;
        var items = await _itemRepository.GetAllAsync();
        Items = new ObservableCollection<Item>(items);
        IsLoading = false;
    }
}
