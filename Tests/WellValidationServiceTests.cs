namespace Tests
{
	using System.Collections.Generic;
	using WellAnalyzer.Models;
	using WellAnalyzer.Services;

	public class WellValidationServiceTests
	{
		#region Public Methods

		[Fact]
		public void Validate_OverlappingIntervals_ReturnsOverlapError()
		{
			// Arrange
			List<ImportedWellRow> rows = new List<ImportedWellRow>
			{
				new ImportedWellRow(
					lineNumber: 1,
					wellId: "A-001",
					x: 82.10,
					y: 55.20,
					depthFrom: 0,
					depthTo: 10,
					rock: "Sandstone",
					porosity: 0.18),

				new ImportedWellRow(
					lineNumber: 2,
					wellId: "A-001",
					x: 82.10,
					y: 55.20,
					depthFrom: 9,
					depthTo: 20,
					rock: "Limestone",
					porosity: 0.07)
			};

			WellValidationService sut = new WellValidationService();

			// Act
			var result = sut.Validate(rows);

			// Assert
			Assert.Single(result.Errors);
			Assert.Single(result.ValidRows);

			Assert.Equal(2, result.Errors[0].LineNumber);
			Assert.Equal("A-001", result.Errors[0].WellId);
			Assert.Contains("Interval overlaps with previous interval", result.Errors[0].Message);

			Assert.Equal(1, result.ValidRows[0].LineNumber);
		}

		[Fact]
		public void Validate_RowWithInvalidPorosity_ReturnsError()
		{
			// Arrange
			List<ImportedWellRow> rows = new List<ImportedWellRow>
			{
				new ImportedWellRow(
					lineNumber:  1,
					wellId: "A-001",
					x: 82.10,
					y: 55.20,
					depthFrom: 0,
					depthTo: 10,
					rock: "Sandstone",
					porosity: 1.5)
			};

			WellValidationService sut = new WellValidationService();

			// Act
			var result = sut.Validate(rows);

			// Assert
			Assert.Single(result.Errors);
			Assert.Empty(result.ValidRows);

			Assert.Equal(1, result.Errors[0].LineNumber);
			Assert.Equal("A-001", result.Errors[0].WellId);
			Assert.Equal("Porosity must be in range [0..1].", result.Errors[0].Message);
		}

		[Fact]
		public void Validate_ValidData_ReturnsValidRows()
		{
			// Arrange
			List<ImportedWellRow> rows = new List<ImportedWellRow>
			{
				new ImportedWellRow(
					lineNumber: 1,
					wellId: "A-001",
					x: 82.10,
					y: 55.20,
					depthFrom: 0,
					depthTo: 10,
					rock: "Sandstone",
					porosity: 0.18),

				new ImportedWellRow(
					lineNumber: 2,
					wellId: "A-001",
					x: 82.10,
					y: 55.20,
					depthFrom: 11,
					depthTo: 20,
					rock: "Limestone",
					porosity: 0.07)
			};

			WellValidationService sut = new WellValidationService();

			// Act
			var result = sut.Validate(rows);

			// Assert
			Assert.Empty(result.Errors);
			Assert.Equal(2, result.ValidRows.Count);

			Assert.Equal("A-001", result.ValidRows[0].WellId);
		}

		#endregion Public Methods
	}
}