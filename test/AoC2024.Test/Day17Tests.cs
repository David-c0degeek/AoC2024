using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day17Tests : BaseDayTests<Day17>
{
    protected override Day17 CreateSut() => new("TestData/Day17Sample.txt");

    [TestMethod]
    public void Solve_WithSampleInput_ReturnsExpectedResults()
    {
        // Act
        var result = Sut.Solve(true, false);

        // Assert
        Assert.AreEqual("4,6,3,5,6,3,5,2,1,0", result.part1);
        Assert.AreEqual(string.Empty, result.part2);
    }
    
    [TestMethod]
    public void Solve_WithSampleInputPart2_ReturnsExpectedResults()
    {
        // Arrange
        var sut = new Day17("TestData/Day17Sample2.txt");
        
        // Act
        var result = sut.Solve(false, true);

        // Assert
        Assert.AreEqual(string.Empty, result.part1);
        Assert.AreEqual("117440", result.part2);
    }
 
}