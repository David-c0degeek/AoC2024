using System.Reflection;
using AoC2024.Interfaces;

namespace AoC2024;

public static class Program
{
    public static void Main()
    {
        var challenges = GetChallengesFromAssembly();

        var daysToRun = new[] { 17 };
        
        foreach (var challenge in challenges)
        {
            if (!daysToRun.Contains(challenge.Day))
            {
                continue;
            }
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
            .Select(t => (IChallenge)Activator.CreateInstance(t, [null])!)
            .OrderBy(c => c.Day);
    }
}