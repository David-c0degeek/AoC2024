﻿namespace AoC2024.Challenges;

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

    public override (string part1, string part2) Solve()
    {
        var input = LoadInput();

        var findXmas = FindXmas(input);
        var findMas = FindMas(input);

        return (findXmas.ToString(), findMas.ToString());
    }

    private char[][] LoadInput() =>
        GetInput()
            .Select(line => line.ToCharArray())
            .ToArray();

    private static readonly char[] Xmas = ['X', 'M', 'A', 'S'];

    private record Direction(int Row, int Col);
    
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
    /// <returns></returns>
    private static int FindMas(char[][] input)
    {
        var count = 0;
    
        var diagonals = new[]
        {
            (Directions[4], Directions[7]),  // Left-Up and Right-Down
            (Directions[5], Directions[6])   // Right-Up and Left-Down
        };

        for (var row = 1; row < input.Length - 1; row++)
        {
            for (var col = 1; col < input[0].Length - 1; col++)
            {
                if (input[row][col] == 'A' && 
                    IsValidMs(input, row, col, diagonals[0].Item1, diagonals[0].Item2) && 
                    IsValidMs(input, row, col, diagonals[1].Item1, diagonals[1].Item2))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private static bool IsValidMs(char[][] input, int row, int col, Direction dir1, Direction dir2)
    {
        var pos1Row = row + dir1.Row;
        var pos1Col = col + dir1.Col;
        var pos2Row = row + dir2.Row;
        var pos2Col = col + dir2.Col;

        return CheckValidPosition(input, pos1Row, pos1Col) && 
               CheckValidPosition(input, pos2Row, pos2Col) &&
               ((input[pos1Row][pos1Col] == 'M' && input[pos2Row][pos2Col] == 'S') ||
                (input[pos1Row][pos1Col] == 'S' && input[pos2Row][pos2Col] == 'M'));
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