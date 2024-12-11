namespace Solutions.Day10;

internal class Day10Part1 : IPart1Challenge
{

    public DateTime Day => new(2024, 12, 10);
    public bool IsActive => false;
    public string Name => "Hoof It";
    public string Part1Result => "566";

    public async Task ExecuteAsync()
    {
        var trailheadMap = (await File.ReadAllLinesAsync("./Day10/input.txt")).Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToArray()).ToArray();

        int trailHeadScore = 0;
        for (int i = 0; i < trailheadMap.Length; i++)
        {
            for (int j = 0; j < trailheadMap[i].Length; j++)
            {
                var trailHead = trailheadMap[i][j];
                if (trailHead == 0)
                {
                    trailHeadScore += FindTrailHeadScore(trailheadMap, i, j);
                }
            }
        }

        Console.WriteLine(trailHeadScore);
    }

    private readonly List<(int X, int Y)> _directions =
    [
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    ];

    private int FindTrailHeadScore(int[][] trailHead, int i, int j)
    {
        var visited = new Stack<(int, int, int value)>();
        var stack = new Stack<(int, int, int value)>();

        stack.Push((i, j, trailHead[i][j]));

        while (stack.Count > 0)
        {
            var trailheadPosition = stack.Pop();

            if (!visited.Contains((trailheadPosition)))
            {
                visited.Push(trailheadPosition);

                foreach (var direction in _directions)
                {
                    if (trailheadPosition.Item1 + direction.X >= 0 && trailheadPosition.Item1 + direction.X < trailHead.Length &&
                        trailheadPosition.Item2 + direction.Y >= 0 && trailheadPosition.Item2 + direction.Y < trailHead[0].Length)
                    {
                        var nextItem = trailHead[trailheadPosition.Item1 + direction.X][trailheadPosition.Item2 + direction.Y];

                        if (nextItem - 1 == trailheadPosition.value)
                        {
                            stack.Push((trailheadPosition.Item1 + direction.X, trailheadPosition.Item2 + direction.Y, nextItem));
                        }
                    }
                }
            }
        }

        return visited.Count(x => x.value == 9);
    }
}

internal class Day10Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 10);
    public bool IsActive => false;
    public string Name => "Hoof It";
    public string Part2Result => "1324";

    public async Task ExecuteAsync()
    {
        var trailheadMap = (await File.ReadAllLinesAsync("./Day10/input.txt")).Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToArray()).ToArray();

        int trailHeadRating = 0;
        for (int i = 0; i < trailheadMap.Length; i++)
        {
            for (int j = 0; j < trailheadMap[i].Length; j++)
            {
                var trailHead = trailheadMap[i][j];
                if (trailHead == 0)
                {
                    trailHeadRating += FindTrailHeadRating(trailheadMap, i, j);
                }
            }
        }

        Console.WriteLine(trailHeadRating);
    }

    private readonly List<(int X, int Y)> _directions =
    [
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    ];

    private int FindTrailHeadRating(int[][] trailHead, int i, int j)
    {
        var visited = new Stack<(int, int, int value)>();
        var stack = new Stack<(int, int, int value)>();

        stack.Push((i, j, trailHead[i][j]));

        while (stack.Count > 0)
        {
            var trailheadPosition = stack.Pop();

            // // if we do not keep track if we have been in that position before but we just add the position
            // // at the end of the search we would be adding the final position for each time we visit it,
            // // which means that for each distinct path we add the final position on the visited array
            //if (!visited.Contains((trailheadPosition)))
            //{
            visited.Push(trailheadPosition);

            foreach (var direction in _directions)
            {
                if (trailheadPosition.Item1 + direction.X >= 0 &&
                    trailheadPosition.Item1 + direction.X < trailHead.Length &&
                    trailheadPosition.Item2 + direction.Y >= 0 &&
                    trailheadPosition.Item2 + direction.Y < trailHead[0].Length)
                {
                    var nextItem =
                        trailHead[trailheadPosition.Item1 + direction.X][trailheadPosition.Item2 + direction.Y];

                    if (nextItem - 1 == trailheadPosition.value)
                    {
                        stack.Push((trailheadPosition.Item1 + direction.X, trailheadPosition.Item2 + direction.Y,
                            nextItem));
                    }
                }
            }
            //}
        }

        return visited.Count(x => x.value == 9);
    }
}