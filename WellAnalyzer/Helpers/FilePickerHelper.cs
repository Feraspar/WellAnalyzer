namespace WellAnalyzer.Helpers
{
	using Avalonia.Controls;
	using Avalonia.Platform.Storage;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Хелпер для выбора файла из файловой системы.
	/// </summary>
	public static class FilePickerHelper
	{
		#region Public Methods

		/// <summary>
		/// Получает путь к CSV-файлу.
		/// </summary>
		/// <param name="window">UI окно.</param>
		/// <returns>Путь к файлу.</returns>
		public static async Task<string?> PickCsvFileAsync(Window window)
		{
			ArgumentNullException.ThrowIfNull(window);

			IReadOnlyList<IStorageFile> files = await window.StorageProvider.OpenFilePickerAsync(
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

			return files[0].Path.LocalPath;
		}

		#endregion Public Methods
	}
}