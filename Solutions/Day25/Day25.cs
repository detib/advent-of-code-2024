namespace Solutions.Day25;

internal class Day25 : IPart1Challenge, IPart2Challenge
{
    public DateTime Day => new(2024, 12, 25);
    public string Name => "Code Chronicle";
    public bool IsActive => false;
    public string Part1Result => "2770";
    public string Part2Result => "\u2B50";

    public async Task ExecuteAsync()
    {
        var input = (await File.ReadAllTextAsync("./Day25/input.txt")).Split("\r\n\r\n");

        var locks = new List<char[][]>();
        var keys = new List<char[][]>();

        foreach (var map in input)
        {
            var t = map.Split("\r\n");

            if (t[0].All(x => x == '#'))
            {
                locks.Add(t[1..].Select(x => x.ToCharArray()).ToArray());
            }
            if (t[^1].All(x => x == '#'))
            {
                keys.Add(t[..^1].Select(x => x.ToCharArray()).ToArray());
            }
        }

        var locksHeights = locks.Select(GetHeight).ToList();
        var keysHeights = keys.Select(GetHeight).ToList();

        var answer = 0;

        foreach (var lockHeight in locksHeights)
        {
            foreach (var keyHeight in keysHeights)
            {
                var canOpen = true;
                for (var i = 0; i < lockHeight.Count; i++)
                {
                    if (lockHeight[i] + keyHeight[i] > 5)
                    {
                        canOpen = false;
                    }
                }

                if (canOpen)
                    answer++;
            }
        }

        Console.WriteLine(answer);
    }

    private static List<int> GetHeight(char[][] grid)
    {
        var counts = new List<int>(new int[5]);

        foreach (var row in grid)
        {
            for (var col = 0; col < row.Length; col++)
            {
                if (row[col] == '#')
                {
                    counts[col]++;
                }
            }
        }

        return counts;
    }
}