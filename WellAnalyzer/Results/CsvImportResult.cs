namespace WellAnalyzer.Results
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;

	/// <summary>
	/// Результат импорта CSV-файла.
	/// </summary>
	public class CsvImportResult
	{
		#region Public Properties

		/// <summary>
		/// Список ошибок при импорте файла.
		/// </summary>
		public List<ValidationError> Errors { get; }

		/// <summary>
		/// Список успешно прочитанных строк.
		/// </summary>
		public List<ImportedWellRow> Rows { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="rows">Список успешно прочитанных строк.</param>
		/// <param name="errors">Список ошибок при импорте файла.</param>
		public CsvImportResult(List<ImportedWellRow> rows, List<ValidationError> errors)
		{
			Rows = rows;
			Errors = errors;
		}

		#endregion Public Constructors
	}
}