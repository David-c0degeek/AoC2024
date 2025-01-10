using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day5Tests : BaseDayTests<Day5>
{
    protected override Day5 CreateSut() => new("TestData/Day5Sample.txt");
    
    [TestMethod]
    public void Solve_WithSampleInput_ReturnsExpectedResults()
    {
        // Act
        var result = Sut.Solve();

        // Assert
        Assert.AreEqual("143", result.part1);
        Assert.AreEqual("123", result.part2);
    }
}