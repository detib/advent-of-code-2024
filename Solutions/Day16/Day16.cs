using static Solutions.Day16.Helper;

namespace Solutions.Day16;

internal class Day16Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 16);
    public bool IsActive => true;
    public string Name => "Reindeer Maze";
    public string Part1Result => "???";
    public async Task ExecuteAsync()
    {
        var map = (await File.ReadAllLinesAsync("./Day16/input.txt")).Select(x => x.ToCharArray()).ToArray();

        var initialReindeerPosition = GetReindeerPosition(map);

        var stack = new Stack<(int i, int j, Direction dir, int answer, List<(int, int)> seen)>();

        var initialSeen = new List<(int, int)>
        {
            (initialReindeerPosition.i, initialReindeerPosition.j)
        };

        stack.Push((initialReindeerPosition.i, initialReindeerPosition.j, Direction.Ri, answer: 0, initialSeen.ToList()));

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

    private static IEnumerable<(int, List<(int, int)>)> MoveReindeer(char[][] map, Stack<(int i, int j, Direction dir, int answer, List<(int, int)> seen)> stack)
    {
        var possibleAnswers = new List<(int, List<(int, int)>)>();
        while (stack.Count > 0)
        {
            var item = stack.Pop();
            var currentSeen = item.seen;

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

                if (nextPosition == 'E')
                {
                    possibleAnswers.Add((item.answer + (item.dir == possibleDirection ? 1 : 1000), item.seen));
                    continue;
                }

                var newSeen = new List<(int, int)>(currentSeen)
                {
                    (item.i + direction.i, item.j + direction.j)
                };


                if (nextPosition == '.')
                {
                    stack.Push((
                        item.i + direction.i,
                        item.j + direction.j,
                        possibleDirection,
                        item.answer + (item.dir == possibleDirection ? 1 : 1000),
                        newSeen
                    ));
                }
            }
        }

        return possibleAnswers.Where(x => x.Item1 == possibleAnswers.Min(x => x.Item1));
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
        { Direction.Ri, [Direction.Ri, Direction.Up, Direction.Do] },
        { Direction.Do, [Direction.Do, Direction.Ri, Direction.Le] },
        { Direction.Le, [Direction.Le, Direction.Up, Direction.Do] },
        { Direction.Up, [Direction.Up, Direction.Ri, Direction.Le] },
    };


    internal static (int i, int j) GetReindeerPosition(char[][] map)
    {
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] is 'S')
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
    }
}