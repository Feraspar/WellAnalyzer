namespace WellAnalyzer.Abstractions
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;

	/// <summary>
	/// Интерфейс для сервиса сборки моделей скважин из импортированных строк.
	/// </summary>
	public interface IWellBuilderService
	{
		#region Public Methods

		/// <summary>
		/// Собирает модели скважин из валидных строк.
		/// </summary>
		/// <param name="rows">Валидные строки.</param>
		/// <returns>Список моделей скважин.</returns>
		List<Well> Build(List<ImportedWellRow> rows);

		#endregion Public Methods
	}
}