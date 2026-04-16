namespace WellAnalyzer
{
	using Avalonia;
	using Avalonia.Controls.ApplicationLifetimes;
	using Avalonia.Markup.Xaml;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using WellAnalyzer.Abstractions;
	using WellAnalyzer.Services;
	using WellAnalyzer.ViewModels;
	using WellAnalyzer.Views;

	/// <summary>
	/// Класс приложения Avalonia.
	/// </summary>
	public partial class App : Application
	{
		#region Public Properties

		/// <summary>
		/// Контейнер зависимостей приложения.
		/// </summary>
		public IServiceProvider Services { get; private set; } = null!;

		#endregion Public Properties

		#region Public Methods

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
			ServiceCollection services = new ServiceCollection();
			ConfigureServices(services);

			Services = services.BuildServiceProvider();

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new MainWindow
				{
					DataContext = Services.GetRequiredService<MainWindowViewModel>()
				};
			}

			base.OnFrameworkInitializationCompleted();
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Регистрирует сервисы приложения в контейнере зависимостей.
		/// </summary>
		/// <param name="services">Коллекция сервисов.</param>
		private static void ConfigureServices(ServiceCollection services)
		{
			services.AddSingleton<ICsvImportService, CsvImportService>();
			services.AddSingleton<IFilePickerService, FilePickerService>();
			services.AddSingleton<WellBuilderService>();
			services.AddSingleton<WellSummaryService>();
			services.AddSingleton<WellValidationService>();
			services.AddSingleton<IJsonExportService, JsonExportService>();
			services.AddSingleton<IWellFacade, WellFacade>();

			services.AddTransient<MainWindowViewModel>();
		}

		#endregion Private Methods
	}
}