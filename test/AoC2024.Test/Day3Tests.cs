using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day3Tests : BaseDayTests<Day3>
{
    protected override Day3 CreateSut() => new("TestData/Day3Sample.txt");
    
    [TestMethod]
    public void Solve_WithSampleInputPart1_ReturnsExpectedResults()
    {
        // Arrange
        var sut = new Day3("TestData/Day3Sample.txt");
        
        // Act
        var result = sut.Solve();

        // Assert
        Assert.AreEqual(161, result.part1);
        Assert.AreEqual(161, result.part2);
    }
    
    [TestMethod]
    public void Solve_WithSampleInputPart2_ReturnsExpectedResults()
    {
        // Arrange
        var sut = new Day3("TestData/Day3Sample2.txt");
        
        // Act
        var result = sut.Solve();

        // Assert
        Assert.AreEqual(161, result.part1);
        Assert.AreEqual(48, result.part2);
    }
}