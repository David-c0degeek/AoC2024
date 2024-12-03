using AoC2024.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2024.Test;

public abstract class BaseDayTests<T> where T : BaseDay
{
    protected T Sut = null!;
    protected abstract T CreateSut();

    [TestInitialize]
    public void Initialize()
    {
        Sut = CreateSut();
    }
}