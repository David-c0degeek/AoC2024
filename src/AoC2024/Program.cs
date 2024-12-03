using AoC2024.Challenges;
using AoC2024.Interfaces;

namespace AoC2024;

public static class Program
{
    public static void Main(string[] args)
    {
        var challenges = new List<IChallenge>
        {
            new Day1(),
            new Day2()
        };

        foreach (var challenge in challenges)
        {
            var results = challenge.Solve();
            Console.WriteLine($"Day {challenge.Day}:");
            Console.WriteLine($"Part 1: {results.part1}");
            Console.WriteLine($"Part 2: {results.part2}");
            Console.WriteLine();
        }
    }
}