namespace WellAnalyzer.Results
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;

	/// <summary>
	/// Результат валидации импортированных строк.
	/// </summary>
	public class WellValidationResult
	{
		#region Public Properties

		/// <summary>
		/// Список ошибок.
		/// </summary>
		public List<ValidationError> Errors { get; }

		/// <summary>
		/// Валидные строки.
		/// </summary>
		public List<ImportedWellRow> ValidRows { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="validRows">Валидные строки.</param>
		/// <param name="errors">Список ошибок.</param>
		public WellValidationResult(List<ImportedWellRow> validRows, List<ValidationError> errors)
		{
			ValidRows = validRows;
			Errors = errors;
		}

		#endregion Public Constructors
	}
}