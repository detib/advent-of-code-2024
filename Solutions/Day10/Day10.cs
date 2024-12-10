namespace Solutions.Day10;

internal class Day10 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 10);
    public bool IsActive => true;
    public string Part1Result => "";

    public async Task ExecuteAsync()
    {
        var trailheadMap = (await File.ReadAllLinesAsync("./Day10/input.txt")).Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToArray()).ToArray();

        int answer = 0;
        for (int i = 0; i < trailheadMap.Length; i++)
        {
            for (int j = 0; j < trailheadMap[i].Length; j++)
            {
                var trailHead = trailheadMap[i][j];
                if (trailHead == 0)
                {
                    answer += FindTrailHeadScore(trailheadMap, i, j);
                }
            }
        }

        Console.WriteLine(answer);
    }

    private readonly List<(int x, int y)> _directions =
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
                    if (trailheadPosition.Item1 + direction.x >= 0 && trailheadPosition.Item1 + direction.x < trailHead.Length &&
                        trailheadPosition.Item2 + direction.y >= 0 && trailheadPosition.Item2 + direction.y < trailHead[0].Length)
                    {
                        var nextItem = trailHead[trailheadPosition.Item1 + direction.x][trailheadPosition.Item2 + direction.y];

                        if (nextItem - 1 == trailheadPosition.value)
                        {
                            stack.Push((trailheadPosition.Item1 + direction.x, trailheadPosition.Item2 + direction.y, nextItem));
                        }
                    }
                }
            }
        }

        return visited.Count(x => x.value == 9);
    }
}