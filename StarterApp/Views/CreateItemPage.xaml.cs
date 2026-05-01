using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class CreateItemPage : ContentPage
{
    public CreateItemPage(CreateItemViewModel viewModel)
    {
        try
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CreateItemPage init error: {ex}");
            throw;
        }
    }
}
