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
            if (Day19.IsDesignPossible(desiredDesign, towelPatterns, []) > 0)
            {
                answer++;
            }
        }

        Console.WriteLine(answer);
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

        long answer = 0;
        foreach (var desiredDesign in desiredDesigns)
            answer += Day19.IsDesignPossible(desiredDesign, towelPatterns.Where(desiredDesign.Contains).ToHashSet(), []);

        Console.WriteLine(answer);
    }
}

internal static class Day19
{
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