using AoC2024.Interfaces;

namespace AoC2024.Challenges;

public abstract class BaseDay : IChallenge
{
    private readonly string _inputPath;

    protected BaseDay(string? inputPath = null)
    {
        _inputPath = inputPath ?? $"Inputs/Day{Day}.txt";
    }

    public abstract int Day { get; }

    protected string[] GetInput() => 
        File.ReadAllLines(_inputPath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();

    public abstract (string part1, string part2) Solve();
}