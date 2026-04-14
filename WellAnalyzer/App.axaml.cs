namespace WellAnalyzer
{
	using Avalonia;
	using Avalonia.Controls.ApplicationLifetimes;
	using Avalonia.Markup.Xaml;
	using WellAnalyzer.ViewModels;
	using WellAnalyzer.Views;

	/// <summary>
	/// Класс приложения Avalonia.
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Инициализирует XAML-разметку приложения.
		/// </summary>
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		/// <summary>
		/// Завершает инициализацию приложения и создает главное окно.
		/// </summary>
		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel(),
				};
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}