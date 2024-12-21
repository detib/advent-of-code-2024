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
        var map = await ReadCharMap("./Day16/input.txt");

        var initialReindeerPosition = GetItemPosition(map, 'S');

        var stack = new Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)>();

        var initialSeen = new HashSet<(int, int)>
        {
            (initialReindeerPosition.i, initialReindeerPosition.j)
        };

        stack.Push((initialReindeerPosition.i, initialReindeerPosition.j, Direction.R, answer: 0, initialSeen.ToHashSet()));

        var answer1 = MoveReindeer(map, stack, bestPathOnly: true);

        foreach (var answer in answer1)
        {
            Console.WriteLine(answer.Item1);

            for (var i = 0; i < map.Length; i++)
            {
                var x = map[i];
                for (var j = 0; j < x.Length; j++)
                {
                    var y = x[j];
                    if (y == '.')
                        y = ' ';
                    if (y == '#')
                        y = '.';

                    Console.Write(answer.Item2.Contains((i, j)) ? 'P' : y);
                }
                Console.WriteLine();
            }
            Console.WriteLine(string.Join("", Enumerable.Repeat("-", 100)));
        }
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
        var map = await ReadCharMap("./Day16/input.txt");

        var initialReindeerPosition = GetItemPosition(map, 'S');

        var stack = new Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)>();

        var initialSeen = new HashSet<(int, int)>
        {
            (initialReindeerPosition.i, initialReindeerPosition.j)
        };

        stack.Push((initialReindeerPosition.i, initialReindeerPosition.j, Direction.R, answer: 0, initialSeen.ToHashSet()));

        var reindeerPaths = MoveReindeer(map, stack, bestPathOnly: false).ToList();

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
}

internal class Helper
{
    internal static readonly Dictionary<Direction, List<Direction>> PossibleMoves = new()
    {
        { Direction.R, [Direction.U, Direction.D, Direction.R] },
        { Direction.D, [Direction.R, Direction.L, Direction.D] },
        { Direction.L, [Direction.U, Direction.D, Direction.L] },
        { Direction.U, [Direction.R, Direction.L, Direction.U] },
    };

    internal static IEnumerable<(int, HashSet<(int, int)>)> MoveReindeer(char[][] map,
        Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)> stack, bool bestPathOnly)
    {
        var possibleAnswers = new List<(int, HashSet<(int, int)>)>();

        var finalPosition = GetItemPosition(map, 'E');

        var globalMin = int.MaxValue; // we can start this with 98416 since we know it is the result of the best path from the first part

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

                if (!bestPathOnly)
                {
                    distances = new int[map.Length][];

                    for (var i = 0; i < distances.Length; i++)
                    {
                        distances[i] = new int[map[i].Length];
                        for (var index = 0; index < distances[i].Length; index++)
                        {
                            distances[i][index] = int.MaxValue;
                        }
                    }
                }
                continue;
            }

            var moves = new List<(int i, int j, Direction newDir, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var possibleDirection in PossibleMoves[item.dir])
            {
                var direction = Sides[possibleDirection];
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