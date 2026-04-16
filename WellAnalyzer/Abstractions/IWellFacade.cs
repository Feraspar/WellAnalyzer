namespace WellAnalyzer.Abstractions
{
	using System.Threading;
	using System.Threading.Tasks;
	using WellAnalyzer.Results;

	/// <summary>
	/// Интерфейс для сервиса сборки всей сводки по скважине.
	/// </summary>
	public interface IWellFacade
	{
		#region Public Methods

		/// <summary>
		/// Собирает сводку по скважине.
		/// </summary>
		/// <param name="filePath">Путь к файлу.</param>
		/// <param name="cancellationToken">Токен для отмены выполняемой операции.</param>
		/// <returns>Результат со списком ошибок и сводками по скважинам.</returns>
		Task<WellProcessingResult> BuildWellSummariesAsync(string filePath, CancellationToken cancellationToken);

		#endregion Public Methods
	}
}