namespace AoC2024.Interfaces;

public interface IChallenge
{
    int Day { get; }
    (string part1, string part2) Solve();
}