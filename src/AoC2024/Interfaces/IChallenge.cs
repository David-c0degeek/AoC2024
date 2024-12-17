namespace AoC2024.Interfaces;

public interface IChallenge
{
    int Day { get; }
    (int part1, int part2) Solve();
}