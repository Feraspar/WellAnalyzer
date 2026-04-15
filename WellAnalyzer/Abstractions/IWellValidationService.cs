namespace WellAnalyzer.Abstractions
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;
	using WellAnalyzer.Results;

	/// <summary>
	/// Интерфейс для сервиса валидации данных по скважине.
	/// </summary>
	public interface IWellValidationService
	{
		#region Public Methods

		/// <summary>
		/// Вадириует данные из входящих строк.
		/// </summary>
		/// <param name="rows">Список строк.</param>
		/// <returns>Список ошибок.</returns>
		WellValidationResult Validate(List<ImportedWellRow> rows);

		#endregion Public Methods
	}
}