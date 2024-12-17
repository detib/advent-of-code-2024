using static Solutions.Day16.Helper;

namespace Solutions.Day16;

internal class Day16Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 16);
    public bool IsActive => false;
    public string Name => "Reindeer Maze";
    public string Part1Result => "98416";
    public async Task ExecuteAsync()
    {
        var map = (await File.ReadAllLinesAsync("./Day16/input.txt")).Select(x => x.ToCharArray()).ToArray();

        var initialReindeerPosition = GetReindeerPosition(map);

        var stack = new Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)>();

        var initialSeen = new HashSet<(int, int)>
        {
            (initialReindeerPosition.i, initialReindeerPosition.j)
        };

        stack.Push((initialReindeerPosition.i, initialReindeerPosition.j, Direction.Ri, answer: 0, initialSeen.ToHashSet()));

        var answer1 = MoveReindeer(map, stack);

        foreach (var answer in answer1)
        {
            Console.WriteLine(answer.Item1);

            for (var i = 0; i < map.Length; i++)
            {
                var x = map[i];
                for (var j = 0; j < x.Length; j++)
                {
                    var y = x[j];
                    Console.Write(answer.Item2.Contains((i, j)) ? 'P' : y);
                }
                Console.WriteLine();
            }
            Console.WriteLine(string.Join("", Enumerable.Repeat("-", 100)));
        }
    }

    private static IEnumerable<(int, HashSet<(int, int)>)> MoveReindeer(char[][] map, Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)> stack)
    {
        var possibleAnswers = new List<(int, HashSet<(int, int)>)>();

        var finalPosition = GetReindeerPosition(map, 'E');

        var globalMin = int.MaxValue;

        var distances = new int[map.Length][];

        for (var i = 0; i < distances.Length; i++)
        {
            distances[i] = new int[map[i].Length];
            for (var index = 0; index < distances[i].Length; index++)
            {
                distances[i][index] = int.MaxValue;
            }
        }

        while (stack.Count > 0)
        {
            var item = stack.Pop();
            var currentSeen = item.seen;

            if (item.answer >= globalMin)
                continue;

            if (map[item.i][item.j] == 'E')
            {
                if (item.answer <= globalMin)
                {
                    globalMin = item.answer;
                    possibleAnswers.Add((item.answer, item.seen.ToHashSet()));
                }
                continue;
            }
            var moves = new List<(int i, int j, Direction newDir, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var possibleDirection in PossibleMoves[item.dir])
            {
                var direction = Directions[possibleDirection];
                if (currentSeen.Contains((item.i + direction.i, item.j + direction.j)))
                    continue;

                var nextPosition = map[item.i + direction.i][item.j + direction.j];
                if (nextPosition == '#')
                {
                    continue;
                }

                var newSeen = new HashSet<(int, int)>(currentSeen)
                {
                    (item.i + direction.i, item.j + direction.j)
                };

                moves.Add((
                    item.i + direction.i,
                    item.j + direction.j,
                    possibleDirection,
                    item.answer + (item.dir == possibleDirection ? 1 : 1001),
                    newSeen
                ));
            }

            moves = moves
                .OrderByDescending(move => Math.Abs(move.i - finalPosition.i) + Math.Abs(move.j - finalPosition.j))
                .ToList();

            foreach (var move in moves)
            {
                if (distances[move.i][move.j] > move.answer)
                {
                    distances[move.i][move.j] = move.answer;
                    stack.Push((move.i, move.j, move.newDir, move.answer, move.newSeen));
                }
            }
        }

        return possibleAnswers.Where(x => x.Item1 == globalMin);
    }
}

internal class Day16Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 16);
    public bool IsActive => false;
    public string Name => "Reindeer Maze";
    public string Part2Result => "471";
    public async Task ExecuteAsync()
    {
        var map = (await File.ReadAllLinesAsync("./Day16/input.txt")).Select(x => x.ToCharArray()).ToArray();

        var initialReindeerPosition = GetReindeerPosition(map);

        var stack = new Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)>();

        var initialSeen = new HashSet<(int, int)>
        {
            (initialReindeerPosition.i, initialReindeerPosition.j)
        };

        stack.Push((initialReindeerPosition.i, initialReindeerPosition.j, Direction.Ri, answer: 0, initialSeen.ToHashSet()));

        var reindeerPaths = MoveReindeer(map, stack).ToList();

        var uniqueSpots = reindeerPaths.SelectMany(x => x.Item2).Distinct().ToList();

        var answer = uniqueSpots.Count;

        for (var i = 0; i < map.Length; i++)
        {
            var x = map[i];
            for (var j = 0; j < x.Length; j++)
            {
                var y = x[j];
                Console.Write(uniqueSpots.Contains((i, j)) ? 'P' : y);
            }
            Console.WriteLine();
        }

        Console.WriteLine(string.Join("", Enumerable.Repeat("-", map.Length)));

        Console.WriteLine(answer);
    }

    private static IEnumerable<(int, HashSet<(int, int)>)> MoveReindeer(char[][] map, Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)> stack)
    {
        var possibleAnswers = new List<(int, HashSet<(int, int)>)>();

        var finalPosition = GetReindeerPosition(map, 'E');

        var globalMin = 98416; // we can start this with int.MaxValue but since we know the result of the best path from the first part we use that

        var distances = new int[map.Length][];

        for (var i = 0; i < distances.Length; i++)
        {
            distances[i] = new int[map[i].Length];
            for (var index = 0; index < distances[i].Length; index++)
            {
                distances[i][index] = int.MaxValue;
            }
        }


        while (stack.Count > 0)
        {
            var item = stack.Pop();
            var currentSeen = item.seen;

            if (item.answer > globalMin)
                continue;

            if (map[item.i][item.j] == 'E')
            {
                if (item.answer < globalMin)
                    globalMin = item.answer;
                possibleAnswers.Add((item.answer, item.seen.ToHashSet()));

                distances = new int[map.Length][];

                for (var i = 0; i < distances.Length; i++)
                {
                    distances[i] = new int[map[i].Length];
                    for (var index = 0; index < distances[i].Length; index++)
                    {
                        distances[i][index] = int.MaxValue;
                    }
                }

                continue;
            }

            var moves = new List<(int i, int j, Direction newDir, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var possibleDirection in PossibleMoves[item.dir])
            {
                var direction = Directions[possibleDirection];
                var nextPosition = map[item.i + direction.i][item.j + direction.j];
                if (nextPosition == '#')
                {
                    continue;
                }

                if (currentSeen.Contains((item.i + direction.i, item.j + direction.j)))
                    continue;

                var newSeen = new HashSet<(int, int)>(currentSeen)
                {
                    (item.i + direction.i, item.j + direction.j)
                };

                moves.Add((
                    item.i + direction.i,
                    item.j + direction.j,
                    possibleDirection,
                    item.answer + (item.dir == possibleDirection ? 1 : 1001),
                    newSeen
                ));
            }

            moves = moves
                .OrderByDescending(move => Math.Abs(move.i - finalPosition.i) + Math.Abs(move.j - finalPosition.j))
                .ToList();

            foreach (var move in moves)
            {
                if (distances[move.i][move.j] > move.answer)
                {
                    distances[move.i][move.j] = move.answer;
                    stack.Push((move.i, move.j, move.newDir, move.answer, move.newSeen));
                }
            }
        }

        return possibleAnswers.Where(x => x.Item1 == globalMin);
    }
}

internal enum Direction
{
    Ri,
    Do,
    Le,
    Up
}

internal class Helper
{
    internal static readonly Dictionary<Direction, (int i, int j)> Directions = new()
    {
        { Direction.Ri, (0, 1) },
        { Direction.Do, (1, 0) },
        { Direction.Le, (0, -1) },
        { Direction.Up, (-1, 0) },
    };

    internal static readonly Dictionary<Direction, List<Direction>> PossibleMoves = new()
    {
        { Direction.Ri, [Direction.Up, Direction.Do, Direction.Ri] },
        { Direction.Do, [Direction.Ri, Direction.Le, Direction.Do] },
        { Direction.Le, [Direction.Up, Direction.Do, Direction.Le] },
        { Direction.Up, [Direction.Ri, Direction.Le, Direction.Up] },
    };

    internal static (int i, int j) GetReindeerPosition(char[][] map, char target = 'S')
    {
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == target)
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
    }
}