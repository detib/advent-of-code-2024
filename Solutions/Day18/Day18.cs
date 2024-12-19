using Solutions.Day16;
using static Solutions.Day18.Helper;

namespace Solutions.Day18;

internal class Day18Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 18);
    public bool IsActive => false;
    public string Name => "RAM Run";
    public string Part1Result => "286";
    public async Task ExecuteAsync()
    {
        var coordinates = await File.ReadAllLinesAsync("./Day18/input.txt");

        var map = new char[71][];

        for (var index = 0; index < map.Length; index++)
        {
            map[index] = new char[71];
            for (var j = 0; j < map[index].Length; j++)
            {
                map[index][j] = '.';
            }
        }

        foreach (var coordinate in coordinates[..1024])
        {
            var coordinateSplit = coordinate.Split(',');
            var coordinateJ = int.Parse(coordinateSplit[0]);
            var coordinateI = int.Parse(coordinateSplit[1]);

            map[coordinateI][coordinateJ] = '#';
        }

        (int i, int j) initialPosition = (0, 0);

        var stack = new Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)>();

        stack.Push((initialPosition.i, initialPosition.j, Direction.Ri, answer: 0, []));
        stack.Push((initialPosition.i, initialPosition.j, Direction.Do, answer: 0, []));

        var answer1 = FindPossibleRoutes(map, stack);

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

    private static IEnumerable<(int, HashSet<(int, int)>)> FindPossibleRoutes(char[][] map, Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)> stack)
    {
        var possibleAnswers = new List<(int, HashSet<(int, int)>)>();

        (int i, int j) finalPosition = (70, 70);
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

            //Console.SetCursorPosition(0, 0);

            //var stringBuilder = new StringBuilder();
            //for (var i = 0; i < map.Length; i++)
            //{
            //    var x = map[i];
            //    for (var j = 0; j < x.Length; j++)
            //    {
            //        var y = x[j];
            //        //if (y == '.')
            //        //    y = ' ';
            //        //if (y == '#')
            //        //    y = '.';

            //        stringBuilder.Append(item.seen.Contains((i, j)) ? 'O' : y);
            //    }
            //    if (i != map.Length - 1)
            //        stringBuilder.AppendLine();
            //}

            //Console.Write(stringBuilder);
            //Console.SetCursorPosition(0, map.Length + 1);



            if (item.i == finalPosition.i && item.j == finalPosition.j)
            {
                if (item.answer <= globalMin)
                {
                    globalMin = item.answer;
                    possibleAnswers.Add((item.answer, item.seen.ToHashSet()));

                }
                continue;
            }
            var moves = new List<(int i, int j, Direction newDir, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var (possibleDirection, direction) in Directions)
            {
                if (item.i + direction.i > 70
                    || item.i + direction.i < 0
                    || item.j + direction.j < 0
                    || item.j + direction.j > 70)
                    continue;

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
                    item.answer + 1,
                    newSeen
                ));
            }

            moves = moves
                .OrderByDescending(move => Math.Abs(move.i - finalPosition.i) + Math.Abs(move.j - finalPosition.j))
                .ToList();

            foreach (var move in moves.Where(move => distances[move.i][move.j] > move.answer))
            {
                distances[move.i][move.j] = move.answer;
                stack.Push((move.i, move.j, move.newDir, move.answer, move.newSeen));
            }
        }

        return possibleAnswers.Where(x => x.Item1 == globalMin);
    }

}

internal class Day18Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 18);
    public bool IsActive => false;
    public string Name => "RAM Run";
    public string Part2Result => "20,64";
    public async Task ExecuteAsync()
    {
        var coordinates = await File.ReadAllLinesAsync("./Day18/input.txt");

        var map = new char[71][];

        for (var index = 0; index < map.Length; index++)
        {
            map[index] = new char[71];
            for (var j = 0; j < map[index].Length; j++)
            {
                map[index][j] = '.';
            }
        }

        for (var index = 0; index < 1024; index++)
        {
            var coordinate = coordinates[index];
            var coordinateSplit = coordinate.Split(',');
            var coordinateJ = int.Parse(coordinateSplit[0]);
            var coordinateI = int.Parse(coordinateSplit[1]);

            map[coordinateI][coordinateJ] = '#';
        }

        for (var index = 1024; index < coordinates.Length; index++)
        {
            var coordinate = coordinates[index];
            var coordinateSplit = coordinate.Split(',');
            var coordinateJ = int.Parse(coordinateSplit[0]);
            var coordinateI = int.Parse(coordinateSplit[1]);

            map[coordinateI][coordinateJ] = '#';


            (int i, int j) initialPosition = (0, 0);

            var stack = new Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)>();

            stack.Push((initialPosition.i, initialPosition.j, Direction.Ri, answer: 0, []));
            stack.Push((initialPosition.i, initialPosition.j, Direction.Do, answer: 0, []));

            var possibleRoutes = RouteExists(map, stack);

            if (!possibleRoutes)
            {
                Console.WriteLine($"Coordinate ({coordinate}) {index}");
                break;
            }
        }
    }

    private static bool RouteExists(char[][] map, Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)> stack)
    {
        (int i, int j) finalPosition = (70, 70);
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

            if (item.i == finalPosition.i && item.j == finalPosition.j)
            {
                return true;
            }

            var moves = new List<(int i, int j, Direction newDir, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var (possibleDirection, direction) in Directions)
            {
                if (item.i + direction.i > 70
                    || item.i + direction.i < 0
                    || item.j + direction.j < 0
                    || item.j + direction.j > 70)
                    continue;

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
                    item.answer + 1,
                    newSeen
                ));
            }

            moves = moves
                .OrderByDescending(move => Math.Abs(move.i - finalPosition.i) + Math.Abs(move.j - finalPosition.j))
                .ToList();

            foreach (var move in moves.Where(move => distances[move.i][move.j] > move.answer))
            {
                distances[move.i][move.j] = move.answer;
                stack.Push((move.i, move.j, move.newDir, move.answer, move.newSeen));
            }
        }

        return false;
    }

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
}