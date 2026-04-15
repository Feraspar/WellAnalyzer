namespace WellAnalyzer.Abstractions
{
	using System.Threading;
	using System.Threading.Tasks;
	using WellAnalyzer.Results;

	/// <summary>
	/// Интерфейс для сервиса импорта CSV-файла.
	/// </summary>
	public interface ICsvImportService
	{
		#region Public Methods

		/// <summary>
		/// Имортирует CSV-файл.
		/// </summary>
		/// <param name="filePath">Путь к файлу.</param>
		/// <param name="cancellationToken">Токен для отмены выполняемой операции.</param>
		/// <returns>Результат импорта.</returns>
		Task<CsvImportResult> ImportAsync(string filePath, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}