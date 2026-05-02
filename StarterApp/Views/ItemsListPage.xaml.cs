using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class ItemsListPage : ContentPage
{
    private readonly ItemsListViewModel _viewModel;

    public ItemsListPage(ItemsListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadItemsCommand.Execute(null);
    }

    private bool _isNavigating;

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (_isNavigating) return;
        if (sender is Element element && element.BindingContext is Item item)
        {
            _isNavigating = true;
            _viewModel.RequestRentalCommand.Execute(item);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isNavigating = false;
    }
}
