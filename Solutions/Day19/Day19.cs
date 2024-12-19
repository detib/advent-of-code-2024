namespace Solutions.Day19;

internal class Day19Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 19);
    public bool IsActive => false;
    public string Name => "Linen Layout";
    public string Part1Result => "287";
    public async Task ExecuteAsync()
    {
        var input = (await File.ReadAllTextAsync("./Day19/input.txt")).Split("\r\n\r\n");
        var towelPatterns = input[0].Split(", ").OrderBy(x => x.Length).ToHashSet();

        var desiredDesigns = input[1].Split("\r\n").ToArray();

        var answer = 0;
        foreach (var desiredDesign in desiredDesigns)
        {
            if (IsDesignPossible(desiredDesign, towelPatterns))
            {
                answer++;
            }
        }

        Console.WriteLine(answer);
    }

    public static bool IsDesignPossible(string desiredDesign, HashSet<string> patterns)
    {
        var longestPattern = patterns.MaxBy(x => x.Length)!;
        var stack = new Stack<(int firstIndex, int secondIndex, string currentTowel)>();

        stack.Push((0, 1, string.Empty));

        var seen = new HashSet<(int, int, string)>();

        while (stack.Count > 0)
        {
            var item = stack.Pop();

            if (item.currentTowel == desiredDesign)
                return true;

            seen.Add((item.firstIndex, item.secondIndex, item.currentTowel));

            for (var secondIndex = item.secondIndex; secondIndex <= desiredDesign.Length; secondIndex++)
            {
                var towelSection = desiredDesign[item.firstIndex..secondIndex];

                if (item.currentTowel + towelSection != desiredDesign[..secondIndex])
                    break;

                if (secondIndex - item.firstIndex > longestPattern.Length)
                    break;

                if (patterns.Contains(towelSection) && !seen.Contains((secondIndex, secondIndex + 1, item.currentTowel + towelSection)))
                {
                    stack.Push((secondIndex, secondIndex + 1, item.currentTowel + towelSection));
                }
            }
        }

        return false;
    }
}

internal class Day19Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 19);
    public bool IsActive => false;
    public string Name => "Linen Layout";
    public string Part2Result => "571894474468161";
    public async Task ExecuteAsync()
    {
        var input = (await File.ReadAllTextAsync("./Day19/input.txt")).Split("\r\n\r\n");
        var towelPatterns = input[0].Split(", ").OrderBy(x => x.Length).ToHashSet();

        var desiredDesigns = input[1].Split("\r\n").ToArray();

        var cache = new Dictionary<string, long>();
        var answer = desiredDesigns.Sum(desiredDesign => IsDesignPossible(desiredDesign, towelPatterns.Where(desiredDesign.Contains).ToHashSet(), cache));

        Console.WriteLine(answer);
    }

    public static long IsDesignPossible(string desiredDesign, HashSet<string> patterns, Dictionary<string, long> cache)
    {

        return RecursiveIsDesignPossible(desiredDesign);

        long RecursiveIsDesignPossible(string currentDesign)
        {
            if (currentDesign == string.Empty)
            {
                return 1;
            }

            if (cache.TryGetValue(currentDesign, out var cachedResult))
            {
                return cachedResult;
            }

            var count = 0L;

            foreach (var pattern in patterns)
            {
                if (pattern.Length > currentDesign.Length)
                    continue;

                if (!currentDesign.StartsWith(pattern))
                    continue;

                count += RecursiveIsDesignPossible(currentDesign[pattern.Length..]);
            }

            cache[currentDesign] = count;

            return count;
        }
    }
}