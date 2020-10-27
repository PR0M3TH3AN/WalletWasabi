using NBitcoin;
using ReactiveUI;
using System.Reactive;
using WalletWasabi.Gui.ViewModels;
using WalletWasabi.Fluent.ViewModels.Dialogs;
using Global = WalletWasabi.Gui.Global;

namespace WalletWasabi.Fluent.ViewModels
{
	public class MainViewModel : ViewModelBase, IScreen, IDialogHost
	{
		private Global _global;
		private NavigationStateViewModel _navigationState;
		private StatusBarViewModel _statusBar;
		private string _title = "Wasabi Wallet";
		private DialogScreenViewModel _dialogScreen;
		private DialogViewModelBase _currentDialog;
		private NavBarViewModel _navBar;
		public MainViewModel(Global global)
		{
			_global = global;

			_dialogScreen = new DialogScreenViewModel();

			_navigationState = new NavigationStateViewModel()
			{
				MainScreen = () => this,
				DialogScreen = () => _dialogScreen,
				DialogHost = () => this
			};

			Network = global.Network;

			StatusBar = new StatusBarViewModel(global.DataDir, global.Network, global.Config, global.HostedServices, global.BitcoinStore.SmartHeaderChain, global.Synchronizer, global.LegalDocuments);

			NavBar = new NavBarViewModel(_navigationState, Router, global.WalletManager, global.UiConfig);
		}

		public static MainViewModel Instance { get; internal set; }

		public RoutingState Router { get; } = new RoutingState();

		public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

		public ReactiveCommand<Unit, Unit> GoBack => Router.NavigateBack;

		public Network Network { get; }

		public DialogScreenViewModel DialogScreen
		{
			get => _dialogScreen;
			set => this.RaiseAndSetIfChanged(ref _dialogScreen, value);
		}

		public DialogViewModelBase CurrentDialog
		{
			get => _currentDialog;
			set => this.RaiseAndSetIfChanged(ref _currentDialog, value);
		}

		public NavBarViewModel NavBar
		{
			get => _navBar;
			set => this.RaiseAndSetIfChanged(ref _navBar, value);
		}

		public StatusBarViewModel StatusBar
		{
			get => _statusBar;
			set => this.RaiseAndSetIfChanged(ref _statusBar, value);
		}

		public string Title
		{
			get => _title;
			internal set => this.RaiseAndSetIfChanged(ref _title, value);
		}

		public void Initialize()
		{
			// Temporary to keep things running without VM modifications.
			MainWindowViewModel.Instance = new MainWindowViewModel(_global.Network, _global.UiConfig, _global.WalletManager, null, null, false);

			StatusBar.Initialize(_global.Nodes.ConnectedNodes);

			if (Network != Network.Main)
			{
				Title += $" - {Network}";
			}
		}
	}
}
