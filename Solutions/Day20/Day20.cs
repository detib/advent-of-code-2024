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

        var initialPosition = GetItemPosition(map, 'S');

        var stack = new Stack<(int i, int j, int answer, HashSet<(int, int)> seen)>();

        stack.Push((initialPosition.i, initialPosition.j, answer: 0, []));

        var initialRouteNoCheats = FindPossibleRoutes(map, stack);

        var distances = initialRouteNoCheats.distances;
        var answer = 0;
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == '#') continue;
                var currentDistance = distances[i][j];
                
                foreach (var value in ((int i, int j)[])[.. Diagonals.Select(x => x.Value).ToArray(), .. Sides.Select(x => (x.Value.i * 2, x.Value.j * 2)).ToArray()])
                {
                    if (!IsWithinBounds(map, (i + value.i, j + value.j)))
                        continue;

                    var nextDistance = distances[i + value.i][j + value.j];

                    if (nextDistance == int.MaxValue)
                        continue;

                    if (currentDistance < nextDistance && nextDistance - currentDistance >= 102)
                        answer++;
                }
            }
        }

        Console.WriteLine(answer);
    }

    private static (int answer, HashSet<(int, int)>, int[][] distances) FindPossibleRoutes(char[][] map,
        Stack<(int i, int j, int answer, HashSet<(int, int)> seen)> stack)
    {
        var globalMin = int.MaxValue;
        var finalPosition = GetItemPosition(map, 'E');

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

            var moves = new List<(int i, int j, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var (_, direction) in Sides)
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

internal class Day20Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 20);
    public bool IsActive => false;
    public string Name => "Race Condition";
    public string Part2Result => "975376";

    public async Task ExecuteAsync()
    {
        var map = await ReadCharMap("./Day20/input.txt");

        var initialPosition = GetItemPosition(map, 'S');

        var stack = new Stack<(int i, int j, int answer, HashSet<(int, int)> seen)>();

        stack.Push((initialPosition.i, initialPosition.j, answer: 0, [initialPosition]));

        var distances = FindPossibleRoutes(map, stack);

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

                        if (nextPointi < 0 || nextPointi >= map.Length || nextPointj < 0 ||
                            nextPointj >= map[nextPointi].Length) continue;

                        var distance = Math.Abs(di) + Math.Abs(dj);
                        
                        if (distance is <= 20 and >= 2)
                        {
                            var nextDistance = distances[nextPointi][nextPointj];

                            if (currentDistance - nextDistance >= 100 + distance)
                                answer++;
                        }
                    }
                }
            }
        }

        Console.WriteLine(answer);
    }

    private static int[][] FindPossibleRoutes(char[][] map,
        Stack<(int i, int j, int answer, HashSet<(int, int)> seen)> stack)
    {
        var globalMin = int.MaxValue;
        var finalPosition = GetItemPosition(map, 'E');

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
                return distances;
            }

            var moves = new List<(int i, int j, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var (_, direction) in Sides)
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
                .OrderByDescending(move => ManhattanDistance((move.i, move.j), (finalPosition.i, finalPosition.j)))
                .ToList();

            foreach (var move in moves.Where(move => distances[move.i][move.j] > move.answer))
            {
                distances[move.i][move.j] = move.answer;
                stack.Push((move.i, move.j, move.answer, move.newSeen));
            }
        }

        return [];
    }
}