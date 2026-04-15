namespace WellAnalyzer.Services
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using WellAnalyzer.Abstractions;
	using WellAnalyzer.Models;
	using WellAnalyzer.Results;

	/// <summary>
	/// Сервис для импорта CSV-файла.
	/// </summary>
	public class CsvImportService : ICsvImportService
	{
		#region Public Methods

		/// <inheritdoc />
		public async Task<CsvImportResult> ImportAsync(string filePath, CancellationToken cancellationToken = default)
		{
			List<ImportedWellRow> rows = new List<ImportedWellRow>();
			List<ValidationError> errors = new List<ValidationError>();

			if (string.IsNullOrWhiteSpace(filePath))
			{
				errors.Add(new ValidationError(0, string.Empty, "File path is empty."));
				return new CsvImportResult(rows, errors);
			}

			if (!File.Exists(filePath))
			{
				errors.Add(new ValidationError(0, string.Empty, "File does not exist."));
				return new CsvImportResult(rows, errors);
			}

			try
			{
				using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

				using StreamReader reader = new StreamReader(fileStream);

				int lineNumber = 0;

				while (!reader.EndOfStream)
				{
					cancellationToken.ThrowIfCancellationRequested();

					string? line = await reader.ReadLineAsync();
					lineNumber++;

					if (string.IsNullOrWhiteSpace(line))
					{
						errors.Add(new ValidationError(lineNumber, string.Empty, "Line is empty"));
						continue;
					}

					ImportedWellRow? row = TryParseLine(line, lineNumber, out ValidationError? error);

					if (row is not null)
					{
						rows.Add(row);
					}
					else if (error is not null)
					{
						errors.Add(error);
					}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				errors.Add(new ValidationError(0, string.Empty, $"Failed to read file: {ex.Message}"));
			}

			return new CsvImportResult(rows, errors);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Пытается распарсить строку в данные.
		/// </summary>
		/// <param name="line">Входная строка.</param>
		/// <param name="lineNumber">Номер строки.</param>
		/// <param name="error">Ошибка.</param>
		/// <returns>Модель данных строки.</returns>
		private ImportedWellRow? TryParseLine(string line, int lineNumber, out ValidationError? error)
		{
			error = null;

			string[] parts = line.Split(';');

			if (parts.Length != 7)
			{
				error = new ValidationError(lineNumber, string.Empty, "Invalid format.");
				return null;
			}

			string wellId = parts[0].Trim();
			string xString = parts[1].Trim();
			string yString = parts[2].Trim();
			string depthFromString = parts[3].Trim();
			string depthToString = parts[4].Trim();
			string rock = parts[5].Trim();
			string porosityString = parts[6].Trim();

			if (string.IsNullOrWhiteSpace(wellId))
			{
				error = new ValidationError(lineNumber, string.Empty, "WellId must not be empty.");
				return null;
			}

			bool isXParsed = double.TryParse(xString, NumberStyles.Float, CultureInfo.InvariantCulture, out double x);

			bool isYParsed = double.TryParse(yString, NumberStyles.Float, CultureInfo.InvariantCulture, out double y);

			bool isDepthFromParsed = double.TryParse(depthFromString, NumberStyles.Float, CultureInfo.InvariantCulture, out double depthFrom);

			bool isDepthToParsed = double.TryParse(depthToString, NumberStyles.Float, CultureInfo.InvariantCulture, out double depthTo);

			bool isPorosityParsed = double.TryParse(porosityString, NumberStyles.Float, CultureInfo.InvariantCulture, out double porosity);

			if (!isXParsed || !isYParsed || !isDepthFromParsed || !isDepthToParsed || !isPorosityParsed)
			{
				error = new ValidationError(lineNumber, wellId, "Invalid numeric value in line.");
				return null;
			}

			return new ImportedWellRow(lineNumber, wellId, x, y, depthFrom, depthTo, porosity, rock);
		}

		#endregion Private Methods
	}
}