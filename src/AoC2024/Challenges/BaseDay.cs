﻿using AoC2024.Interfaces;

namespace AoC2024.Challenges;

public abstract class BaseDay(string? inputPath = null) : IChallenge
{
    public abstract int Day { get; }

    protected string[] GetInput() => 
        File.ReadAllLines(inputPath ?? $"Inputs/Day{Day}.txt")
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();

    public abstract (int part1, int part2) Solve();
}