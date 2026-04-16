namespace WellAnalyzer.Abstractions
{
	using WellAnalyzer.Models;

	/// <summary>
	/// Интерфейс для сервиса расчета сводки по скважине.
	/// </summary>
	public interface IWellSummaryService
	{
		#region Public Methods

		/// <summary>
		/// Собирает сводку по скважине.
		/// </summary>
		/// <param name="well">Модель скважины.</param>
		/// <returns>Сводка по скважине.</returns>
		WellSummary BuildSummary(Well well);

		#endregion Public Methods
	}
}