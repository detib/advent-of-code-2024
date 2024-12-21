using System.Numerics;

namespace Solutions.Day11;

internal class Day11Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 11);
    public bool IsActive => false;
    public string Name => "Plutonian Pebbles";
    public string Part1Result => "207683";

    public async Task ExecuteAsync()
    {
        var stones = (await File.ReadAllTextAsync("./Day11/input.txt")).Split(" ").ToList();

        const int blinks = 25;
        for (var i = 0; i < blinks; i++)
        {
            var stoneCount = stones.Count;
            var newStoneList = new List<string>(stones.Count);
            for (var index = 0; index < stoneCount; index++)
            {
                var stone = stones[index];
                if (stone.All(x => x == '0'))
                {
                    newStoneList.Add("1");
                    continue;
                }

                if (stone.Length % 2 == 0)
                {
                    var stoneHalfwayPoint = (int)Math.Floor((decimal)(stone.Length / 2));

                    var leftHalf = stone[..stoneHalfwayPoint];
                    var rightHalf = stone[stoneHalfwayPoint..].TrimStart('0');

                    newStoneList.Add(leftHalf);
                    newStoneList.Add(rightHalf.Length == 0 ? "0" : rightHalf);


                    continue;
                }

                var stoneValueIfNoRules = (BigInteger.Parse(stone)) * 2024;

                newStoneList.Add($"{stoneValueIfNoRules}");
            }

            stones = newStoneList;
        }

        Console.WriteLine(stones.Count);
    }
}

internal class Day11Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 11);
    public bool IsActive => false;
    public string Name => "Plutonian Pebbles";
    public string Part2Result => "244782991106220";

    public async Task ExecuteAsync()
    {
        var stones = (await File.ReadAllTextAsync("./Day11/input.txt")).Split(" ").ToList();

        BigInteger ans = 0;

        foreach (var stone in stones)
        {
            ans += FindStoneCount(stone, 75);
        }

        Console.WriteLine(ans);
    }

    private static readonly Dictionary<string, BigInteger> Cache = new();

    private static BigInteger FindStoneCount(string stone, int blinksDone = 0)
    {
        if (blinksDone == 0)
        {
            return 1;
        }

        if (stone.All(x => x == '0'))
        {
            var exists = Cache.TryGetValue($"1-{blinksDone - 1}", out var cacheItem);
            if (exists)
                return cacheItem;

            var result = FindStoneCount("1", blinksDone - 1);

            Cache.Add($"1-{blinksDone - 1}", result);

            return result;
        }

        if (stone.Length % 2 == 0)
        {
            var stoneHalfwayPoint = (int)Math.Floor((decimal)(stone.Length / 2));

            var leftHalf = stone[..stoneHalfwayPoint];
            var rightHalf = stone[stoneHalfwayPoint..].TrimStart('0');
            var rightHalf2 = rightHalf.Length == 0 ? "0" : rightHalf;

            var exists1 = Cache.TryGetValue($"{leftHalf}-{blinksDone - 1}", out var cacheItem1);
            if (!exists1)
            {
                cacheItem1 = FindStoneCount(leftHalf, blinksDone - 1);

                Cache.Add($"{leftHalf}-{blinksDone - 1}", cacheItem1);
            }


            var exists2 = Cache.TryGetValue($"{rightHalf2}-{blinksDone - 1}", out var cacheItem2);
            if (!exists2)
            {
                cacheItem2 = FindStoneCount(rightHalf2, blinksDone - 1);

                Cache.Add($"{rightHalf2}-{blinksDone - 1}", cacheItem2);
            }

            return cacheItem1 + cacheItem2;
        }

        var stoneValueIfNoRules = BigInteger.Parse(stone) * 2024;
        var exists3 = Cache.TryGetValue($"{stoneValueIfNoRules}-{blinksDone - 1}", out var cacheItem3);
        if (!exists3)
        {
            cacheItem3 = FindStoneCount($"{stoneValueIfNoRules}", blinksDone - 1);

            Cache.Add($"{stoneValueIfNoRules}-{blinksDone - 1}", cacheItem3);
        }

        return cacheItem3;
    }
}