using System.Reflection.Metadata;

namespace Solutions.Day12;

internal class Day12Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 12);
    public bool IsActive => false;
    public string Name => "Garden Groups";
    public string Part1Result => "1375574";
    public async Task ExecuteAsync()
    {
        var map = (await File.ReadAllLinesAsync("./Day12/input.txt")).Select(x => x.ToCharArray()).ToArray();

        var answer = 0;
        List<(int X, int Y)> alreadyVisitedSpots = [];

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (alreadyVisitedSpots.Contains((i, j)))
                    continue;
                answer += FindPriceOfRegion(map, i, j, alreadyVisitedSpots);
            }
        }

        Console.WriteLine(answer);
    }

    private readonly List<(int X, int Y)> _directions =
    [
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    ];

    private int FindPriceOfRegion(char[][] map, int i, int j, List<(int X, int Y)> alreadyCalculatedSpots)
    {
        var visited = new Stack<(int X, int Y)>();
        var stack = new Stack<(int X, int Y)>();

        stack.Push((i, j));
        var perimeter = 0;
        while (stack.Count > 0)
        {
            var currentPlant = stack.Pop();

            if (!visited.Contains((currentPlant)))
            {
                visited.Push(currentPlant);

                foreach (var direction in _directions)
                {
                    if (currentPlant.X + direction.X >= 0 && currentPlant.X + direction.X < map.Length &&
                        currentPlant.Y + direction.Y >= 0 && currentPlant.Y + direction.Y < map[0].Length)
                    {
                        var nextItem = map[currentPlant.X + direction.X][currentPlant.Y + direction.Y];

                        if (nextItem == map[currentPlant.X][currentPlant.Y])
                        {
                            stack.Push((currentPlant.X + direction.X, currentPlant.Y + direction.Y));
                        }
                        else
                        {
                            perimeter++;
                        }
                    }
                    else
                    {
                        perimeter++;
                    }
                }
            }
        }

        alreadyCalculatedSpots.AddRange(visited);

        return visited.Count * perimeter;
    }
}

internal class Day12Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 12);
    public bool IsActive => false;
    public string Name => "Garden Groups";
    public string Part2Result => "830566";
    public async Task ExecuteAsync()
    {
        var map = (await File.ReadAllLinesAsync("./Day12/input.txt")).Select(x => x.ToCharArray()).ToArray();

        var answer = 0;
        List<(int X, int Y)> alreadyVisitedSpots = [];

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (alreadyVisitedSpots.Contains((i, j)))
                    continue;
                answer += FindPriceOfRegion(map, i, j, alreadyVisitedSpots);
            }
        }

        Console.WriteLine(answer);
    }

    private readonly List<(int X, int Y)> _directions =
    [
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    ];

    private int FindPriceOfRegion(char[][] map, int i, int j, List<(int X, int Y)> alreadyCalculatedSpots)
    {
        var visited = new Stack<(int X, int Y)>();
        var stack = new Stack<(int X, int Y)>();

        stack.Push((i, j));
        var sides = 0;
        var corners = 0;
        while (stack.Count > 0)
        {
            var currentPlant = stack.Pop();

            if (!visited.Contains(currentPlant))
            {
                visited.Push(currentPlant);

                var currentPlantValue = map[currentPlant.X][currentPlant.Y];

                var upValue = currentPlant.X - 1 >= 0 ? map[currentPlant.X - 1][currentPlant.Y] : ' ';
                var rightValue = currentPlant.Y + 1 < map[currentPlant.X].Length ? map[currentPlant.X][currentPlant.Y + 1] : ' ';
                var downValue = currentPlant.X + 1 < map.Length ? map[currentPlant.X + 1][currentPlant.Y] : ' ';
                var leftValue = currentPlant.Y - 1 >= 0 ? map[currentPlant.X][currentPlant.Y - 1] : ' ';

                if (upValue != currentPlantValue && rightValue != currentPlantValue)
                    corners++;

                if (rightValue != currentPlantValue && downValue != currentPlantValue)
                    corners++;

                if (downValue != currentPlantValue && leftValue != currentPlantValue)
                    corners++;
                
                if (leftValue != currentPlantValue && upValue != currentPlantValue)
                    corners++;

                var upLeftValue = (currentPlant.X - 1 >= 0 && currentPlant.Y - 1 >= 0) ? map[currentPlant.X - 1][currentPlant.Y - 1] : ' ';
                var upRightValue = (currentPlant.X - 1 >= 0 && currentPlant.Y + 1 < map[currentPlant.X].Length) ? map[currentPlant.X - 1][currentPlant.Y + 1] : ' ';
                var downRightValue = (currentPlant.X + 1 < map.Length && currentPlant.Y + 1 < map[currentPlant.X].Length) ? map[currentPlant.X + 1][currentPlant.Y + 1] : ' ';
                var downLeftValue = (currentPlant.X + 1 < map.Length && currentPlant.Y - 1 >= 0) ? map[currentPlant.X + 1][currentPlant.Y - 1] : ' ';

                // A A A
                // A A A
                // A A .
                if (upValue == currentPlantValue && leftValue == currentPlantValue && upLeftValue != currentPlantValue)
                    corners++;

                if (upValue == currentPlantValue && rightValue == currentPlantValue && upRightValue != currentPlantValue)
                    corners++;

                if (downValue == currentPlantValue && rightValue == currentPlantValue && downRightValue != currentPlantValue)
                    corners++;

                if (downValue == currentPlantValue && leftValue == currentPlantValue && downLeftValue != currentPlantValue)
                    corners++;


                foreach (var direction in _directions)
                {
                    if (currentPlant.X + direction.X >= 0 && currentPlant.X + direction.X < map.Length &&
                        currentPlant.Y + direction.Y >= 0 && currentPlant.Y + direction.Y < map[0].Length)
                    {
                        var nextItem = map[currentPlant.X + direction.X][currentPlant.Y + direction.Y];

                        if (nextItem == map[currentPlant.X][currentPlant.Y])
                        {
                            stack.Push((currentPlant.X + direction.X, currentPlant.Y + direction.Y));
                        }
                    }
                }
            }
        }

        alreadyCalculatedSpots.AddRange(visited);

        return visited.Count * corners;
    }
}