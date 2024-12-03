using System.Reflection;
using AoC2024.Interfaces;

namespace AoC2024;

public static class Program
{
    public static void Main(string[] args)
    {
        var challenges = GetChallengesFromAssembly();

        foreach (var challenge in challenges)
        {
            var results = challenge.Solve();
            Console.WriteLine($"Day {challenge.Day}:");
            Console.WriteLine($"Part 1: {results.part1}");
            Console.WriteLine($"Part 2: {results.part2}");
            Console.WriteLine();
        }
    }

    private static IOrderedEnumerable<IChallenge> GetChallengesFromAssembly()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(IChallenge).IsAssignableFrom(t))
            .Select(Activator.CreateInstance)
            .Cast<IChallenge>()
            .OrderBy(c => c.Day);
    }
}