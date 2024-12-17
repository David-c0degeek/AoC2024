using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day6Tests : BaseDayTests<Day6>
{
    protected override Day6 CreateSut() => new("TestData/Day6Sample.txt");
    
    [TestMethod]
    public void Solve_WithSampleInput_ReturnsExpectedResults()
    {
        // Act
        var result = Sut.Solve();

        // Assert
        Assert.AreEqual(41, result.part1);
        Assert.AreEqual(6, result.part2);
    }
}