namespace WellAnalyzer.Models
{
	/// <summary>
	/// Модель для ошибки валидации.
	/// </summary>
	public class ValidationError
	{
		#region Public Properties

		/// <summary>
		/// Номер строки.
		/// </summary>
		public int LineNumber { get; }

		/// <summary>
		/// Описание ошибки.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Id скважины.
		/// </summary>
		public string WellId { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="lineNumber">Номер строки.</param>
		/// <param name="wellId">Id скважины.</param>
		/// <param name="message">Описание ошибки.</param>
		public ValidationError(int lineNumber, string wellId, string message)
		{
			LineNumber = lineNumber;
			WellId = wellId;
			Message = message;
		}

		#endregion Public Constructors
	}
}