namespace WellAnalyzer.Abstractions
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using WellAnalyzer.Models;

	/// <summary>
	/// Интерфейс для сервиса экспорта информации в Json-строку.
	/// </summary>
	public interface IJsonExportService
	{
		#region Public Methods

		/// <summary>
		/// Экспортирует сводную информацию в Json строку.
		/// </summary>
		/// <param name="wellSummaries">Сводная информация по скважинам.</param>
		/// <param name="filePath">Путь для сохранения.</param>
		/// <param name="cancellationToken">Токен для отмены выполняемой операции.</param>
		Task ExportAsync(IReadOnlyCollection<WellSummary> wellSummaries, string filePath, CancellationToken cancellationToken = default);

		#endregion Public Methods
	}
}