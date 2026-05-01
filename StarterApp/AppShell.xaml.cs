using StarterApp.ViewModels;
using StarterApp.Views;

namespace StarterApp;

public partial class AppShell : Shell
{
	public AppShell(AppShellViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();

		Routing.RegisterRoute("itemslist", typeof(ItemsListPage));
		Routing.RegisterRoute("rentals", typeof(RentalsPage));
		Routing.RegisterRoute("createitem", typeof(CreateItemPage));
	}
}
