namespace WellAnalyzer
{
	using Avalonia;
	using System;

	/// <summary>
	/// Точка входа в приложение.
	/// </summary>
	internal sealed class Program
	{
		[STAThread]
		public static void Main(string[] args) => BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);

		/// <summary>
		/// Собирает экземпляр AppBuilder для запуска приложения.
		/// </summary>
		/// <returns>Настроенный экземпляр AppBuilder.</returns>
		public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect()
#if DEBUG
	            .WithDeveloperTools()
#endif
				.WithInterFont()
				.LogToTrace();
	}
}
