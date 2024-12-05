using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day4Tests: BaseDayTests<Day4>
{
    protected override Day4 CreateSut() => new("TestData/Day4Sample.txt");
    
    [TestMethod]
    public void Solve_WithSampleInput_ReturnsExpectedResults()
    {
        // Act
        var result = Sut.Solve();

        // Assert
        Assert.AreEqual("18", result.part1);
        Assert.AreEqual(string.Empty, result.part2);
    }
}