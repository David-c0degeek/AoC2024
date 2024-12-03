using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day2Tests
{
    private readonly List<List<int>> _sampleInput =
    [
        new() { 7, 6, 4, 2, 1 },
        new() { 1, 2, 7, 8, 9 },
        new() { 9, 7, 6, 2, 1 },
        new() { 1, 3, 2, 4, 5 },
        new() { 8, 6, 4, 4, 1 },
        new() { 1, 3, 6, 7, 9 }
    ];

    [TestMethod]
    public void CalculateSafeReports_WithSampleInput_ReturnsTwo()
    {
        // Act
        var result = Day2.CalculateSafeReports(_sampleInput);
        
        // Assert
        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void CalculateSafeReportsWithProblemDampner_WithSampleInput_ReturnsFour()
    {
        // Act
        var result = Day2.CalculateSafeReportsWithProblemDampener(_sampleInput);
        
        // Assert
        Assert.AreEqual(4, result);
    }
}