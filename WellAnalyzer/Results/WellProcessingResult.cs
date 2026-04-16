namespace WellAnalyzer.Results
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;

	/// <summary>
	/// Результат сборки сводок по скважинам со списком ошибок
	/// </summary>
	/// <param name="WellSummaries">Сводки по скважинам.</param>
	/// <param name="ValidationErrors">Весь список ошибок.</param>
	public record WellProcessingResult(List<WellSummary> WellSummaries, List<ValidationError> ValidationErrors);
}