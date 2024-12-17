using AoC2024.Models;

namespace AoC2024.Challenges;

/// <summary>
/// --- Day 4: Ceres Search ---
/// "Looks like the Chief's not here. Next!"
///  One of The Historians pulls out a device and pushes the only button on it.
///  After a brief flash, you recognize the interior of the Ceres monitoring station!
/// </summary>
/// <param name="inputPath"></param>
public class Day4(string? inputPath = null) : BaseDay(inputPath)
{
    public override int Day => 4;

    public override (int part1, int part2) Solve()
    {
        var input = LoadInput();

        var findXmas = FindXmas(input);
        var findMas = FindXWord(input, "MAS".ToCharArray());

        return (findXmas, findMas);
    }

    private char[][] LoadInput() =>
        GetInput()
            .Select(line => line.ToCharArray())
            .ToArray();

    private static readonly char[] Xmas = ['X', 'M', 'A', 'S'];
    
    private static readonly Direction[] Directions =
    [
        new(0, -1), // Left
        new(0, 1), // Right
        new(-1, 0), // Up
        new(1, 0), // Down
        new(-1, -1), // Left-Up
        new(-1, 1), // Right-Up
        new(1, -1), // Left-Down
        new(1, 1) // Right-Down
    ];

    /// <summary>
    /// As the search for the Chief continues, a small Elf who lives on the station tugs on your shirt; 
    /// she'd like to know if you could help her with her word search (your puzzle input). She only has to find one word: XMAS.
    /// 
    /// This word search allows words to be horizontal, vertical, diagonal, written backwards, or even overlapping other words. 
    /// It's a little unusual, though, as you don't merely need to find one instance of XMAS - you need to find all of them. 
    /// Here are a few ways XMAS might appear, where irrelevant characters have been replaced with .:
    /// 
    /// ..X...
    /// .SAMX.
    /// .A..A.
    /// XMAS.S
    /// .X....
    /// 
    /// The actual word search will be full of letters instead. For example:
    /// 
    /// MMMSXXMASM
    /// MSAMXMSMSA
    /// AMXSXMAAMM
    /// MSAMASMSMX
    /// XMASAMXAMM
    /// XXAMMXXAMA
    /// SMSMSASXSS
    /// SAXAMASAAA
    /// MAMMMXMMMM
    /// MXMXAXMASX
    /// 
    /// In this word search, XMAS occurs a total of 18 times; here's the same word search again, 
    /// but where letters not involved in any XMAS have been replaced with .:
    /// 
    /// ....XXMAS.
    /// .SAMXMS...
    /// ...S..A...
    /// ..A.A.MS.X
    /// XMASAMX.MM
    /// X.....XA.A
    /// S.S.S.S.SS
    /// .A.A.A.A.A
    /// ..M.M.M.MM
    /// .X.X.XMASX
    /// 
    /// Take a look at the little Elf's word search. How many times does XMAS appear?
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static int FindXmas(char[][] input)
    {
        var sum = 0;
        var rows = input.Length;
        var cols = input[0].Length;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                sum += Directions.Sum(direction => CheckDirection(input, row, col, direction));
            }
        }

        return sum;
    }

    /// <summary>
    /// 
    /// The Elf looks quizzically at you. Did you misunderstand the assignment?
    /// 
    /// Looking for the instructions, you flip over the word search to find that this isn't actually an XMAS puzzle; it's an X-MAS puzzle in which you're supposed to find two MAS in the shape of an X. One way to achieve that is like this:
    /// 
    /// M.S
    /// .A.
    /// M.S
    /// 
    /// Irrelevant characters have again been replaced with . in the above diagram. Within the X, each MAS can be written forwards or backwards.
    /// 
    /// Here's the same example from before, but this time all of the X-MASes have been kept instead:
    /// 
    /// .M.S......
    /// ..A..MSMS.
    /// .M.S.MAA..
    /// ..A.ASMSM.
    /// .M.S.M....
    /// ..........
    /// S.S.S.S.S.
    /// .A.A.A.A..
    /// M.M.M.M.M.
    /// ..........
    /// 
    /// In this example, an X-MAS appears 9 times.
    /// Flip the word search from the instructions back over to the word search side and try again. How many times does an X-MAS appear? 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="wordToSearch"></param>
    /// <returns></returns>
    private static int FindXWord(char[][] input, char[] wordToSearch)
    {
        // Word must have odd length to have a center character
        if (wordToSearch.Length % 2 == 0) return 0;

        var count = 0;
        var halfLength = wordToSearch.Length / 2;
        var centerChar = wordToSearch[halfLength];

        // Get reversed word for checking both directions
        var reversedWord = wordToSearch.Reverse().ToArray();

        // Using the diagonal directions from Directions array
        var diagonals = new[]
        {
            (Directions[4], Directions[7]), // Left-Up and Right-Down
            (Directions[5], Directions[6]) // Right-Up and Left-Down
        };

        for (var row = halfLength; row < input.Length - halfLength; row++)
        {
            for (var col = halfLength; col < input[0].Length - halfLength; col++)
            {
                if (input[row][col] == centerChar &&
                    IsValidXWord(input, row, col, diagonals[0].Item1, diagonals[0].Item2, wordToSearch, reversedWord,
                        halfLength) &&
                    IsValidXWord(input, row, col, diagonals[1].Item1, diagonals[1].Item2, wordToSearch, reversedWord,
                        halfLength))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static bool IsValidXWord(char[][] input, int row, int col, Direction dir1, Direction dir2,
        char[] word, char[] reversedWord, int halfLength)
    {
        // Check if either the forward or reversed word matches in this diagonal
        return IsValidWordInDirection(input, row, col, dir1, dir2, word, halfLength) ||
               IsValidWordInDirection(input, row, col, dir1, dir2, reversedWord, halfLength);
    }

    private static bool IsValidWordInDirection(char[][] input, int row, int col, Direction dir1, Direction dir2,
        char[] word, int halfLength)
    {
        // Check first half of the word (going in dir1 direction)
        for (var i = 1; i <= halfLength; i++)
        {
            var checkRow = row + (dir1.Row * i);
            var checkCol = col + (dir1.Col * i);

            if (!CheckValidPosition(input, checkRow, checkCol) ||
                input[checkRow][checkCol] != word[halfLength - i])
                return false;
        }

        // Check second half of the word (going in dir2 direction)
        for (var i = 1; i <= halfLength; i++)
        {
            var checkRow = row + (dir2.Row * i);
            var checkCol = col + (dir2.Col * i);

            if (!CheckValidPosition(input, checkRow, checkCol) ||
                input[checkRow][checkCol] != word[halfLength + i])
                return false;
        }

        return true;
    }

    private static int CheckDirection(char[][] input, int row, int col, Direction direction)
    {
        for (var i = 0; i < Xmas.Length; i++)
        {
            var newRow = row + direction.Row * i;
            var newCol = col + direction.Col * i;

            if (!CheckValidPosition(input, newRow, newCol) || input[newRow][newCol] != Xmas[i])
            {
                return 0;
            }
        }

        return 1;
    }

    private static bool CheckValidPosition(char[][] input, int row, int col)
    {
        return row >= 0 && row < input.Length &&
               col >= 0 && col < input[0].Length;
    }
}