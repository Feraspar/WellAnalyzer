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
		/// Добавляет ошибку валидации для строки.
		/// </summary>
		/// <param name="errors">Список ошибок.</param>
		/// <param name="row">Входящая строка.</param>
		/// <param name="message">Сообщение о</param>
		private void AddError(List<ValidationError> errors, ImportedWellRow row, string message)
		{
			errors.Add(new ValidationError(row.LineNumber, row.WellId, message));
		}

		/// <summary>
		/// Проверяет, что DepthFrom не отрицателен.
		/// </summary>
		/// <param name="row">Входящая строка.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <returns>Валидна ли строка.</returns>
		private bool ValidateDepthFrom(ImportedWellRow row, List<ValidationError> errors)
		{
			if (row.DepthFrom >= 0)
			{
				return true;
			}

			AddError(errors, row, "DepthFrom must be greater than or equal to 0.");
			return false;
		}

		/// <summary>
		/// Проверяет, что DepthFrom меньше DepthTo.
		/// </summary>
		/// <param name="row">Входящая строка.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <returns>Валидна ли строка.</returns>
		private bool ValidateDepthRange(ImportedWellRow row, List<ValidationError> errors)
		{
			if (row.DepthFrom < row.DepthTo)
			{
				return true;
			}

			AddError(errors, row, "DepthFrom must be less than DepthTo.");
			return false;
		}

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
		/// Проверяет диапазон пористости.
		/// </summary>
		/// <param name="row">Входящая строка.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <returns>Валидна ли строка.</returns>
		private bool ValidatePorosity(ImportedWellRow row, List<ValidationError> errors)
		{
			if (row.Porosity >= 0 && row.Porosity <= 1)
			{
				return true;
			}

			AddError(errors, row, "Porosity must be in range [0..1].");
			return false;
		}

		/// <summary>
		/// Проверяет, что порода указана.
		/// </summary>
		/// <param name="row">Входящая строка.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <returns>Валидна ли строка.</returns>
		private bool ValidateRock(ImportedWellRow row, List<ValidationError> errors)
		{
			if (!string.IsNullOrWhiteSpace(row.Rock))
			{
				return true;
			}

			AddError(errors, row, "Rock must not be empty.");
			return false;
		}

		/// <summary>
		/// Проверяет данные в строке.
		/// </summary>
		/// <param name="row">Входящая строка.</param>
		/// <param name="errors">Список ошибок.</param>
		/// <returns>Валидна ли строка.</returns>
		private bool ValidateRow(ImportedWellRow row, List<ValidationError> errors)
		{
			bool depthRangeValid = ValidateDepthRange(row, errors);
			bool depthFromValid = ValidateDepthFrom(row, errors);
			bool porosityValid = ValidatePorosity(row, errors);
			bool rockValid = ValidateRock(row, errors);

			return depthRangeValid && depthFromValid && porosityValid && rockValid;
		}

		#endregion Private Methods
	}
}