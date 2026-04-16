namespace WellAnalyzer.Services
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using WellAnalyzer.Abstractions;
	using WellAnalyzer.Models;
	using WellAnalyzer.Results;

	/// <summary>
	/// Сервис сборки всей сводки по скважине.
	/// </summary>
	public class WellFacade : IWellFacade
	{
		#region Private Fields

		/// <inheritdoc <see cref="ICsvImportService" />
		private readonly ICsvImportService _csvImportService;

		/// <inheritdoc <see cref="WellBuilderService" />
		private readonly WellBuilderService _wellBuilderService;

		/// <inheritdoc <see cref="WellSummaryService" />
		private readonly WellSummaryService _wellSummaryService;

		/// <inheritdoc <see cref="WellValidationService" />
		private readonly WellValidationService _wellValidationService;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="csvImportService">Сервис для импорта CSV-файла.</param>
		/// <param name="wellBuilderService">Сервис сборки моделей скважин из импортированных строк.</param>
		/// <param name="wellSummaryService">Сервис расчета сводки по скважине.</param>
		/// <param name="wellValidationService">Сервис валидации данных по скважине.</param>
		public WellFacade(ICsvImportService csvImportService, WellBuilderService wellBuilderService, WellSummaryService wellSummaryService, WellValidationService wellValidationService)
		{
			_csvImportService = csvImportService;
			_wellBuilderService = wellBuilderService;
			_wellSummaryService = wellSummaryService;
			_wellValidationService = wellValidationService;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <inheritdoc />
		public async Task<WellProcessingResult> BuildWellSummariesAsync(string filePath, CancellationToken cancellationToken)
		{
			CsvImportResult importResult = await _csvImportService.ImportAsync(filePath, cancellationToken);
			WellValidationResult validationResult = _wellValidationService.Validate(importResult.Rows);

			List<ValidationError> allErrors = new List<ValidationError>();
			allErrors.AddRange(importResult.Errors);
			allErrors.AddRange(validationResult.Errors);

			List<Well> wells = _wellBuilderService.Build(validationResult.ValidRows);
			List<WellSummary> wellSummaries = new List<WellSummary>();

			foreach (var well in wells)
			{
				WellSummary wellSummary = _wellSummaryService.BuildSummary(well);
				wellSummaries.Add(wellSummary);
			}

			return new WellProcessingResult(wellSummaries, allErrors);
		}

		#endregion Public Methods
	}
}