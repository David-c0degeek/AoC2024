using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day1Tests: BaseDayTests<Day1>
{
    protected override Day1 CreateSut() => new("TestData/Day1Sample.txt");
    

    [TestMethod]
    public void Solve_WithSampleInput_ReturnsExpectedResults()
    {
        // Act
        var result = Sut.Solve();

        // Assert
        Assert.AreEqual("11", result.part1);
        Assert.AreEqual("31", result.part2);
    }
}