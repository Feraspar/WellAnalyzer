namespace WellAnalyzer.Abstractions
{
	using System.Threading.Tasks;

	/// <summary>
	/// Интерфейс для сервиса выбора файла из файловой системы.
	/// </summary>
	public interface IFilePickerService
	{
		#region Public Methods

		/// <summary>
		/// Открывает диалоговое окно для выбора файла.
		/// </summary>
		/// <returns>Путь к файлу.</returns>
		Task<string?> PickCsvFileAsync();

		/// <summary>
		/// Открывает диалог выбора пути сохранения JSON-файла.
		/// </summary>
		/// <returns>Путь к файлу.</returns>
		Task<string?> PickJsonSaveFileAsync();

		#endregion Public Methods
	}
}