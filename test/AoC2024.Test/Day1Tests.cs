using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

[TestClass]
public class Day1Tests
{
    private readonly List<int> _leftList = [3, 4, 2, 1, 3, 3];
    private readonly List<int> _rightList = [4, 3, 5, 3, 9, 3];
    
    [TestMethod]
    public void CalculateDistance_WithSampleInput_ReturnsExpectedResult()
    {
        // Arrange
        const int expectedValue = 11;
        
        // Act
        var result = Day1.CalculateDistance(_leftList, _rightList);
        
        // Assert
        Assert.AreEqual(expectedValue, result); 
    }

    [TestMethod]
    public void CalculateSimilarity_WithSampleInput_ReturnsExpectedResult()
    {
        // Arrange
        const int expectedValue = 31;

        // Act
        var result = Day1.CalculateSimilarity(_leftList, _rightList);
        
        // Assert
        Assert.AreEqual(expectedValue, result); 
    }
}