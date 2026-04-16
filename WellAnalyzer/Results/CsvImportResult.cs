namespace WellAnalyzer.Results
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;

	/// <summary>
	/// Результат импорта CSV-файла.
	/// </summary>
	/// /// <param name="Rows">Список успешно прочитанных строк.</param>
	/// <param name="Errors">Список ошибок при импорте файла.</param>
	public record CsvImportResult(List<ImportedWellRow> Rows, List<ValidationError> Errors);
}