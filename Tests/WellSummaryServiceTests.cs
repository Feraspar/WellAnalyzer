namespace Tests
{
	using WellAnalyzer.Models;
	using WellAnalyzer.Services;

	public class WellSummaryServiceTests
	{
		#region Public Methods

		[Fact]
		public void BuildSummary_ValidWell_ReturnsCorrectSummary()
		{
			// Arrange
			Well well = new Well("A-001", 82.10, 55.20);

			well.AddInterval(new Interval(0, 10, "Sandstone", 0.18));
			well.AddInterval(new Interval(10, 25, "Limestone", 0.07));
			well.AddInterval(new Interval(25, 40, "Sandstone", 0.20));

			WellSummaryService sut = new WellSummaryService();

			// Act
			WellSummary result = sut.BuildSummary(well);

			// Assert
			Assert.Equal("A-001", result.WellId);
			Assert.Equal(40, result.TotalDepth);
			Assert.Equal(3, result.IntervalCount);

			double expectedAveragePorosity = ((10 * 0.18) + (15 * 0.07) + (15 * 0.20)) / 40;
			Assert.Equal(expectedAveragePorosity, result.AveragePorosity, 3);

			Assert.Equal("Sandstone", result.MostCommonRock);
		}

		#endregion Public Methods
	}
}