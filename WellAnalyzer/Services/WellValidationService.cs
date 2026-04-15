namespace WellAnalyzer.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using WellAnalyzer.Abstractions;
	using WellAnalyzer.Models;
	using WellAnalyzer.Results;

	/// <summary>
	/// Сервис валидации данных по скважине.
	/// </summary>
	public class WellValidationService : IWellValidationService
	{
		#region Public Methods

		/// <inheritdoc />
		public WellValidationResult Validate(List<ImportedWellRow> rows)
		{
			List<ValidationError> errors = new List<ValidationError>();
			List<ImportedWellRow> validRows = new List<ImportedWellRow>();

			foreach (ImportedWellRow row in rows)
			{
				bool isValid = ValidateRow(row, errors);

				if (isValid)
				{
					validRows.Add(row);
				}
			}

			HashSet<int> invalidLineNumbers = ValidateOverlaps(validRows, errors);

			List<ImportedWellRow> finalValidRows = validRows.Where(row => !invalidLineNumbers.Contains(row.LineNumber)).ToList();

			return new WellValidationResult(finalValidRows, errors);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Проверяет пересечения по интервалам в скважине.
		/// </summary>
		/// <param name="rows">Список строк</param>
		/// <param name="errors">Список ошибок.</param>
		private HashSet<int> ValidateOverlaps(List<ImportedWellRow> rows, List<ValidationError> errors)
		{
			HashSet<int> invalidLineNumbers = new HashSet<int>();

			List<IGrouping<string, ImportedWellRow>> rowsByWell = rows.GroupBy(row => row.WellId).ToList();

			foreach (IGrouping<string, ImportedWellRow> group in rowsByWell)
			{
				List<ImportedWellRow> orderedRows = group.OrderBy(row => row.DepthFrom).ToList();

				for (int i = 1; i < orderedRows.Count; i++)
				{
					ImportedWellRow previousRow = orderedRows[i - 1];
					ImportedWellRow currentRow = orderedRows[i];

					if (currentRow.DepthFrom < previousRow.DepthTo)
					{
						errors.Add(new ValidationError(currentRow.LineNumber, currentRow.WellId, $"Interval overlaps with previous interval [{previousRow.DepthFrom}; {previousRow.DepthTo}]."));
						invalidLineNumbers.Add(currentRow.LineNumber);
					}
				}
			}

			return invalidLineNumbers;
		}

		/// <summary>
		/// Проверяет данные в строке.
		/// </summary>
		/// <param name="row">Входящая строка.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <returns>Валидна ли строка.</returns>
		private bool ValidateRow(ImportedWellRow row, List<ValidationError> errors)
		{
			bool isValid = true;

			if (row.DepthFrom >= row.DepthTo)
			{
				errors.Add(new ValidationError(row.LineNumber, row.WellId, "DepthFrom must be less than DepthTo."));
				isValid = false;
			}

			if (row.DepthFrom < 0)
			{
				errors.Add(new ValidationError(row.LineNumber, row.WellId, "DepthFrom must be greater than or equal to 0."));
				isValid = false;
			}

			if (row.Porosity < 0 || row.Porosity > 1)
			{
				errors.Add(new ValidationError(row.LineNumber, row.WellId, "Porosity must be in range [0..1]."));
				isValid = false;
			}

			if (string.IsNullOrWhiteSpace(row.Rock))
			{
				errors.Add(new ValidationError(row.LineNumber, row.WellId, "Rock must not be empty."));
				isValid = false;
			}

			return isValid;
		}

		#endregion Private Methods
	}
}