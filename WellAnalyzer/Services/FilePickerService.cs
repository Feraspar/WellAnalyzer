namespace WellAnalyzer.Services
{
	using Avalonia;
	using Avalonia.Controls;
	using Avalonia.Controls.ApplicationLifetimes;
	using Avalonia.Platform.Storage;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using WellAnalyzer.Abstractions;

	/// <summary>
	/// Сервис выбора файла из файловой системы.
	/// </summary>
	public class FilePickerService : IFilePickerService
	{
		#region Public Methods

		/// <inheritdoc />
		public async Task<string?> PickCsvFileAsync()
		{
			IClassicDesktopStyleApplicationLifetime? desktopLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

			Window? mainWindow = desktopLifetime?.MainWindow;

			if (mainWindow is null)
			{
				return null;
			}

			IReadOnlyList<IStorageFile> files = await mainWindow.StorageProvider.OpenFilePickerAsync(
				new FilePickerOpenOptions
				{
					Title = "Select CSV file",
					AllowMultiple = false,
					FileTypeFilter =
					[
						new FilePickerFileType("CSV files")
						{
							Patterns = ["*.csv"]
						}
					]
				});

			if (files.Count == 0)
			{
				return null;
			}

			return files[0].TryGetLocalPath();
		}

		#endregion Public Methods
	}
}