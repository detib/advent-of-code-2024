using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using Solutions.Day16;
using static Solutions.Day20.Helper;

namespace Solutions.Day20;

internal class Day20Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 20);
    public bool IsActive => false;
    public string Name => "Race Condition";
    public string Part1Result => "1338";

    public async Task ExecuteAsync()
    {
        var map = (await File.ReadAllLinesAsync("./Day20/input.txt")).Select(x => x.ToCharArray()).ToArray();

        var initialPosition = GetCharacterPosition(map, 'S');

        var stack = new Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)>();

        stack.Push((initialPosition.i, initialPosition.j, Direction.Ri, answer: 0, []));
        stack.Push((initialPosition.i, initialPosition.j, Direction.Do, answer: 0, []));

        var initialRouteNoCheats = FindPossibleRoutes(map, stack);
        var distances = initialRouteNoCheats.distances;
        var answer = 0;
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] != '#')
                {
                    var currentDistance = distances[i][j];
                    foreach (var value in ((int i, int j)[])[.. Diagonals.Select(x => x.Value).ToArray(), .. Directions.Select(x => (x.Value.i * 2, x.Value.j * 2)).ToArray()])
                    {
                        if (i + value.i < 0 || j + value.j < 0 || i + value.i >= map.Length || j + value.j >= map.Length)
                            continue;

                        var nextDistance = distances[i + value.i][j + value.j];

                        if (nextDistance == int.MaxValue)
                            continue;

                        if (currentDistance < nextDistance && nextDistance - currentDistance >= 102)
                            answer++;
                    }
                }
            }
        }

        Console.WriteLine(answer);
    }

    private static (int answer, HashSet<(int, int)>, int[][] distances) FindPossibleRoutes(char[][] map,
        Stack<(int i, int j, Direction dir, int answer, HashSet<(int, int)> seen)> stack)
    {
        var globalMin = int.MaxValue;
        var finalPosition = GetCharacterPosition(map, 'E');

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

                return (item.answer, item.seen.ToHashSet(), distances);
            }

            var moves = new List<(int i, int j, Direction newDir, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var (possibleDirection, direction) in Directions)
            {
                if (currentSeen.Contains((item.i + direction.i, item.j + direction.j)))
                    continue;

                if (item.i + direction.i < 0 || item.i + direction.i >= map.Length || item.j + direction.j < 0 ||
                    item.j + direction.j >= map[0].Length)
                    continue;

                var nextPosition = map[item.i + direction.i][item.j + direction.j];
                if (nextPosition == '#')
                    continue;

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

        return (-1, [], []);
    }
}

internal class Day20Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 20);
    public bool IsActive => false;
    public string Name => "Race Condition";
    public string Part2Result => "975376";

    public async Task ExecuteAsync()
    {
        var map = (await File.ReadAllLinesAsync("./Day20/input.txt")).Select(x => x.ToCharArray()).ToArray();

        var initialPosition = GetCharacterPosition(map, 'S');

        var stack = new Stack<(int i, int j, int answer, HashSet<(int, int)> seen)>();

        stack.Push((initialPosition.i, initialPosition.j, answer: 0, [initialPosition]));

        var (_, _, distances) = FindPossibleRoutes(map, stack);

        distances[initialPosition.i][initialPosition.j] = 0;

        var answer = 0;

        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == '#') continue;

                var currentDistance = distances[i][j];

                for (var di = -20; di <= 20; di++)
                {
                    for (var dj = -20; dj <= 20; dj++)
                    {
                        var nextPointi = i + di;
                        var nextPointj = j + dj;

                        if (nextPointi >= 0 && nextPointi < map.Length && nextPointj >= 0 && nextPointj < map[nextPointi].Length)
                        {
                            var manhattanDistance = Math.Abs(di) + Math.Abs(dj);
                            if (manhattanDistance is <= 20 and >= 2)
                            {
                                var nextDistance = distances[nextPointi][nextPointj];

                                if (currentDistance - nextDistance >= 100 + manhattanDistance)
                                    answer++;
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine(answer);
    }

    private static (int answer, HashSet<(int, int)>, int[][] distances) FindPossibleRoutes(char[][] map,
        Stack<(int i, int j, int answer, HashSet<(int, int)> seen)> stack)
    {
        var globalMin = int.MaxValue;
        var finalPosition = GetCharacterPosition(map, 'E');

        var distances = new int[map.Length][];

        for (var i = 0; i < distances.Length; i++)
        {
            distances[i] = new int[map[i].Length];
            for (var j = 0; j < distances[i].Length; j++)
            {
                distances[i][j] = int.MaxValue;
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
                return (item.answer, item.seen.ToHashSet(), distances);
            }

            var moves = new List<(int i, int j, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var (_, direction) in Directions)
            {
                if (currentSeen.Contains((item.i + direction.i, item.j + direction.j)))
                    continue;

                if (item.i + direction.i < 0 || item.i + direction.i >= map.Length || item.j + direction.j < 0 ||
                    item.j + direction.j >= map[0].Length)
                    continue;

                var nextPosition = map[item.i + direction.i][item.j + direction.j];
                if (nextPosition == '#')
                    continue;

                var newSeen = new HashSet<(int, int)>(currentSeen)
                {
                    (item.i + direction.i, item.j + direction.j)
                };

                moves.Add((
                    item.i + direction.i,
                    item.j + direction.j,
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
                stack.Push((move.i, move.j, move.answer, move.newSeen));
            }
        }

        return (-1, [], []);
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

    internal static readonly Dictionary<Direction, (int i, int j)> Diagonals = new()
    {
        { Direction.ToRi, (-1, 1) },
        { Direction.ToLe, (-1, -1) },
        { Direction.BoRi, (1, -1) },
        { Direction.BoLe, (1, 1) },
    };

    internal static (int i, int j) GetCharacterPosition(char[][] map, char target)
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