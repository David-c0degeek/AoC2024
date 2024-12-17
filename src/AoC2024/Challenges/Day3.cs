using System.Text.RegularExpressions;

namespace AoC2024.Challenges;

/// <summary>
/// --- Day 3: Mull It Over ---
/// /// 
/// Our computers are having issues, so I have no idea if we have any Chief Historians in stock!
/// You're welcome to check the warehouse, though, says the mildly flustered shopkeeper at the
/// North Pole Toboggan Rental Shop. The Historians head out to take a look.
/// 
/// The shopkeeper turns to you. "Any chance you can see why our computers are having issues again?"
/// 
/// The computer appears to be trying to run a program, but its memory (your puzzle input) is corrupted.
/// All of the instructions have been jumbled up!
/// </summary>
/// <param name="inputPath"></param>
public partial class Day3(string? inputPath = null) : BaseDay(inputPath)
{
    public override int Day => 3;

    public override (int part1, int part2) Solve()
    {
        var input = GetInput();
        var singleInputLine = string.Join(string.Empty, input);
        
        var mulResult = Multiply(singleInputLine);
        var mulWithFilterResult = MultiplyWithFiltering(singleInputLine);

        return (mulResult, mulWithFilterResult);
    }
    
    [GeneratedRegex(@"mul\(\d{1,3},\d{1,3}\)")]
    private static partial Regex CorrectMulInputCommand();
    
    [GeneratedRegex(@"\d{1,3}")]
    private static partial Regex NumberRegex();

    /// <summary>
    ///  It seems like the goal of the program is just to multiply some numbers.
    ///  It does that with instructions like mul(X,Y), where X and Y are each 1-3 digit numbers.
    ///  For instance, mul(44,46) multiplies 44 by 46 to get a result of 2024. Similarly,
    ///  mul(123,4) would multiply 123 by 4.
    ///  
    ///  However, because the program's memory has been corrupted, there are also many invalid characters
    ///  that should be ignored, even if they look like part of a mul instruction.
    ///  Sequences like mul(4*, mul(6,9!, ?(12,34), or mul ( 2 , 4 ) do nothing.
    ///  
    ///  For example, consider the following section of corrupted memory:
    ///  
    ///  xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
    ///  
    ///  Only the four highlighted sections are real mul instructions.
    ///  Adding up the result of each instruction produces 161 (2*4 + 5*5 + 11*8 + 8*5).
    ///  
    ///  Scan the corrupted memory for uncorrupted mul instructions.
    ///  What do you get if you add up all of the results of the multiplications?
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static int Multiply(string input)
    {
        var correctCommands = CorrectMulInputCommand()
            .Matches(input);

        var sum = 0;
        
        foreach (Match correctCommand in correctCommands)
        {
            
            sum += Mul(correctCommand);
        }

        return sum;
    }

    [GeneratedRegex(@"(?:mul\(\d{1,3},\d{1,3}\)|(?:do|don't)\(\))")]
    private static partial Regex MulWithFilteringRegex();
    
    private static int MultiplyWithFiltering(string input)
    {
        var commandsWithFiltering = MulWithFilteringRegex()
            .Matches(input);
        
        var sum = 0;
        var doNotCount = false;
        
        foreach (Match commandWithFiltering in commandsWithFiltering)
        {
            if (string.Equals(commandWithFiltering.Value, "don't()"))
            {
                doNotCount = true;
                continue;
            }

            if (string.Equals(commandWithFiltering.Value, "do()"))
            {
                doNotCount = false;
                continue;
            }

            if (doNotCount)
            {
                continue;
            }
            
            sum += Mul(commandWithFiltering);
        }

        return sum;
    }

    private static int Mul(Match commandWithFiltering)
    {
        var numbers = NumberRegex().Matches(commandWithFiltering.Value);
        var firstNumber = int.Parse(numbers[0].Value);
        var secondNumber = int.Parse(numbers[1].Value);
        return firstNumber * secondNumber;
    }
}