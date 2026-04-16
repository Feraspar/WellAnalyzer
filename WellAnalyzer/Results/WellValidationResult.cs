namespace WellAnalyzer.Results
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;

	/// <summary>
	/// Результат валидации импортированных строк.
	/// </summary>
	/// <param name="ValidRows">Валидные строки.</param>
	/// <param name="Errors">Список ошибок при валидации.</param>
	public record WellValidationResult(List<ImportedWellRow> ValidRows, List<ValidationError> Errors);
}