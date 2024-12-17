using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day2Tests : BaseDayTests<Day2>
{
  protected override Day2 CreateSut() => new("TestData/Day2Sample.txt");

    [TestMethod]
    public void Solve_WithSampleInput_ReturnsExpectedResults()
    {
        // Act
        var result = Sut.Solve();

        // Assert
        Assert.AreEqual(2, result.part1);
        Assert.AreEqual(4, result.part2);
    }
}