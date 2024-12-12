namespace AoC2024.Challenges;

public class Day5(string? inputPath = null) : BaseDay(inputPath)
{
    public override int Day => 5;

    private record Order(int First, int Second);

    public override (string part1, string part2) Solve()
    {
        var (pageOrdering, orderingRules) = LoadInput();

        if (!ValidateInput(pageOrdering, orderingRules))
        {
            throw new ArgumentException("Invalid input detected");
        }

        var sumCorrectOrderMiddlePage = SumCorrectOrderMiddlePage(pageOrdering, orderingRules);
        var sumCorrectOrderMiddlePageAndRemaining = CorrectTheOrderAndSumRemaining(pageOrdering, orderingRules);

        return (sumCorrectOrderMiddlePage.ToString(), sumCorrectOrderMiddlePageAndRemaining.ToString());
    }

    private static bool ValidateInput(List<int[]> pageOrdering, List<Order> orderingRules)
    {
        if (pageOrdering.Count == 0 || orderingRules.Count == 0)
            return false;

        // Verify all page numbers are positive
        if (pageOrdering.Any(order => order.Any(page => page <= 0)) ||
            orderingRules.Any(rule => rule.First <= 0 || rule.Second <= 0))
            return false;

        // Basic cycle detection (could be enhanced for more complex cycles)
        foreach (var rule in orderingRules)
        {
            if (rule.First == rule.Second)
                return false;

            if (orderingRules.Any(r => r.First == rule.Second && r.Second == rule.First))
                return false;
        }

        return true;
    }

    private (List<int[]> Pages, List<Order> Rules) LoadInput()
    {
        var input = GetInput();

        var pageOrdering = new List<int[]>();
        var orderingRules = new List<Order>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.Contains(','))
            {
                var page = line.Split(',').Select(int.Parse).ToArray();
                pageOrdering.Add(page);
                continue;
            }

            if (line.Contains('|'))
            {
                var rule = line.Split('|').Select(int.Parse).ToList();
                var order = new Order(rule.First(), rule.Last());
                orderingRules.Add(order);
            }
        }

        return (pageOrdering, orderingRules);
    }

    private static int SumCorrectOrderMiddlePage(List<int[]> pageOrdering, List<Order> orderingRules)
    {
        // Pre-compute the ordering relationships
        var orderLookup = orderingRules
            .GroupBy(x => x.First)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Second).ToHashSet()
            );

        return pageOrdering
            .Where(pageOrder => IsValidOrder(pageOrder, orderLookup))
            .Sum(GetMiddle);
    }

    private static bool IsValidOrder(int[] pageOrder, Dictionary<int, HashSet<int>> orderLookup)
    {
        for (var i = 0; i < pageOrder.Length; i++)
        {
            var currentPage = pageOrder[i];

            // If this page has any ordering rules
            if (!orderLookup.TryGetValue(currentPage, out var mustComeBefore))
            {
                continue;
            }

            // Check if any page that should come after appears before
            for (var j = 0; j < i; j++)
            {
                if (mustComeBefore.Contains(pageOrder[j]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static int GetMiddle(int[] pageOrder) =>
        pageOrder.Length % 2 == 0 ? 0 : pageOrder[pageOrder.Length / 2];

    /// <summary>
    /// --- Part Two ---
    /// 
    /// While the Elves get to work printing the correctly-ordered updates, you have a little time to
    /// fix the rest of them.
    /// 
    /// For each of the incorrectly-ordered updates, use the page ordering rules to put the page numbers
    /// in the right order. For the above example, here are the three incorrectly-ordered updates
    /// and their correct orderings:
    /// 
    /// 75,97,47,61,53 becomes 97,75,47,61,53.
    /// 61,13,29 becomes 61,29,13.
    /// 97,13,75,29,47 becomes 97,75,47,29,13.
    /// 
    /// After taking only the incorrectly-ordered updates and ordering them correctly,
    /// their middle page numbers are 47, 29, and 47. Adding these together produces 123.
    /// 
    /// Find the updates which are not in the correct order. What do you get if you add up the middle page
    /// numbers after correctly ordering just those updates?
    /// 
    /// </summary>
    /// <param name="pageOrdering"></param>
    /// <param name="orderingRules"></param>
    /// <returns></returns>
    private static int CorrectTheOrderAndSumRemaining(List<int[]> pageOrdering, List<Order> orderingRules)
    {
        // Pre-compute the ordering relationships
        var orderLookup = orderingRules
            .GroupBy(x => x.First)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Second).ToHashSet()
            );

        var incorrectPages = pageOrdering
            .Where(x => !IsValidOrder(x, orderLookup));

        return incorrectPages
            .Select(x => CorrectPageOrder(x, orderLookup))
            .Sum(GetMiddle);
    }

    private static int[] CorrectPageOrder(int[] unorderedPages, Dictionary<int, HashSet<int>> orderLookup)
    {
        var remainingPages = unorderedPages.ToList();
        var result = new List<int>(unorderedPages.Length);

        while (remainingPages.Count != 0)
        {
            var nextPage = remainingPages.First(page => 
                !remainingPages.Any(otherPage => 
                    orderLookup.TryGetValue(otherPage, out var mustComeBefore) && 
                    mustComeBefore.Contains(page)
                )
            );

            result.Add(nextPage);
            remainingPages.Remove(nextPage);
        }

        return result.ToArray();
    }
}