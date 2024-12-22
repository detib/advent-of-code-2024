using System.Numerics;
using static Solutions.Day21.Day21;

namespace Solutions.Day21;


// this solution was based on this video
// https://www.youtube.com/watch?v=dqzAaj589cM

internal class Day21Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 21);
    public bool IsActive => false;
    public string Name => "Keypad Conundrum";
    public string Part1Result => "197560";

    public async Task ExecuteAsync()
    {
        var lines = await File.ReadAllLinesAsync("./Day21/input.txt");

        var answer = 0;

        foreach (var line in lines)
        {
            var options = Solve(line, NumberKeypad);

            for (var i = 0; i < 2; i++)
            {
                var possibleNextOptions = new List<string>();
                foreach (var x in options)
                {
                    possibleNextOptions.AddRange(Solve(x, DirectionalKeypad));
                }

                var minimum = possibleNextOptions.Select(y => y.Length).Min();
                options = possibleNextOptions.Where(x => x.Length == minimum)
                    .ToList();
            }

            answer += options.MinBy(x => x.Length)!.Length * int.Parse(line[..3]);
        }

        Console.WriteLine(answer);
    }

    public static List<string> Solve(string line, char[][] keypad)
    {
        var keypadItemPositions = new Dictionary<char, (int, int)>();

        for (var i = 0; i < keypad.Length; i++)
        {
            for (var j = 0; j < keypad[i].Length; j++)
            {
                if (keypad[i][j] != 'X')
                    keypadItemPositions.Add(keypad[i][j], (i, j));
            }
        }

        var sequencesFromOneKeyToAnother = new Dictionary<(char, char), List<string>>();
        foreach (var x in keypadItemPositions.Keys)
        {
            foreach (var y in keypadItemPositions.Keys)
            {
                if (x == y)
                {
                    sequencesFromOneKeyToAnother.Add((x, y), ["A"]);
                    continue;
                }

                var routes = FindPossibleRoutes(keypad, x, y);

                sequencesFromOneKeyToAnother.Add((x, y), routes);
            }
        }

        line = "A" + line;

        var moves = new List<List<string>>();
        for (var i = 1; i < line.Length; i++)
        {
            var lastChar = line[i - 1];
            var currentChar = line[i];

            var options = sequencesFromOneKeyToAnother[(lastChar, currentChar)];

            moves.Add(options);
        }

        var product = CartesianProduct(moves);

        return product.Select(x => string.Concat(x)).ToList();
    }
}

internal class Day21Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 21);
    public bool IsActive => false;
    public string Name => "Keypad Conundrum";
    public string Part2Result => "242337182910752";

    public async Task ExecuteAsync()
    {
        var lines = await File.ReadAllLinesAsync("./Day21/input.txt");

        BigInteger answer = 0;

        DirectionalPadSequences();
        NumPadSequences();

        foreach (var line in lines)
        {
            var options = Solve(line, 1);
            BigInteger optimalLength = long.MaxValue;
            foreach (var t in options)
            {
                var length = GetLength(t);

                optimalLength = BigInteger.Min(optimalLength, length);
            }

            answer += optimalLength * int.Parse(line[..3]);
        }

        Console.WriteLine(answer);
    }

    public BigInteger GetLength(string sequence, int depth = 25)
    {
        if (LengthsCache.TryGetValue((sequence, depth), out var cachedresult))
            return cachedresult;

        if (depth == 1)
        {
            var line1 = "A" + sequence;
            BigInteger length1 = 0;
            for (var i = 1; i < line1.Length; i++)
            {
                var nextLastChar = line1[i - 1];
                var nextCurrentChar = line1[i];

                length1 += DirectionalPadSequences()[(nextLastChar, nextCurrentChar)].MinBy(x => x.Length)!.Length;
            }

            return LengthsCache[(sequence, depth)] = length1;
        }

        var line = "A" + sequence;
        BigInteger length = 0;
        for (var i = 1; i < line.Length; i++)
        {
            var nextLastChar = line[i - 1];
            var nextCurrentChar = line[i];

            BigInteger optimal = ulong.MaxValue;
            foreach (var subseq in DirectionalPadSequences()[(nextLastChar, nextCurrentChar)])
            {
                var res = GetLength(subseq, depth - 1);
                optimal = BigInteger.Min(optimal, res);
            }

            length += optimal;
        }

        LengthsCache[(sequence, depth)] = length;

        return length;
    }

    public List<string> Solve(string line, int keypad)
    {
        line = "A" + line;

        var moves = new List<List<string>>();
        for (var i = 1; i < line.Length; i++)
        {
            var lastChar = line[i - 1];
            var currentChar = line[i];

            var options = SequencesCache[keypad][(lastChar, currentChar)];

            moves.Add(options);
        }

        var product = CartesianProduct(moves);

        return product.Select(x => string.Concat(x)).ToList();
    }

    public Dictionary<(string sequence, int depth), BigInteger> LengthsCache = new();

    public Dictionary<int, Dictionary<(char, char), List<string>>> SequencesCache = new();

    public Dictionary<(char, char), List<string>> DirectionalPadSequences()
    {
        if (SequencesCache.TryGetValue(2, out var sequences))
            return sequences;

        var keypad = DirectionalKeypad;

        var keypadItemPositions = new Dictionary<char, (int, int)>();

        for (var i = 0; i < keypad.Length; i++)
        {
            for (var j = 0; j < keypad[i].Length; j++)
            {
                if (keypad[i][j] != 'X')
                    keypadItemPositions.Add(keypad[i][j], (i, j));
            }
        }

        var sequencesFromOneKeyToAnother = new Dictionary<(char, char), List<string>>();
        foreach (var x in keypadItemPositions.Keys)
        {
            foreach (var y in keypadItemPositions.Keys)
            {
                if (x == y)
                {
                    sequencesFromOneKeyToAnother.Add((x, y), ["A"]);
                    continue;
                }

                var routes = FindPossibleRoutes(keypad, x, y);

                sequencesFromOneKeyToAnother.Add((x, y), routes);
            }
        }

        return SequencesCache[2] = sequencesFromOneKeyToAnother;
    }

    public Dictionary<(char, char), List<string>> NumPadSequences()
    {
        if (SequencesCache.TryGetValue(1, out var sequences))
            return sequences;

        var keypad = NumberKeypad;

        var keypadItemPositions = new Dictionary<char, (int, int)>();

        for (var i = 0; i < keypad.Length; i++)
        {
            for (var j = 0; j < keypad[i].Length; j++)
            {
                if (keypad[i][j] != 'X')
                    keypadItemPositions.Add(keypad[i][j], (i, j));
            }
        }

        var sequencesFromOneKeyToAnother = new Dictionary<(char, char), List<string>>();
        foreach (var x in keypadItemPositions.Keys)
        {
            foreach (var y in keypadItemPositions.Keys)
            {
                if (x == y)
                {
                    sequencesFromOneKeyToAnother.Add((x, y), ["A"]);
                    continue;
                }

                var routes = FindPossibleRoutes(keypad, x, y);

                sequencesFromOneKeyToAnother.Add((x, y), routes);
            }
        }

        return SequencesCache[1] = sequencesFromOneKeyToAnother;
    }
}

internal static class Day21
{
    internal static List<string> FindPossibleRoutes(char[][] map, char start, char end)
    {
        var possibleAnswers = new List<(int, string path)>();

        var initialPosition = GetItemPosition(map, start);
        var finalPosition = GetItemPosition(map, end);

        var globalMin =
            int.MaxValue;

        var stack = new Stack<(int i, int j, int answer, string path, HashSet<(int, int)> seen)>();

        stack.Push((initialPosition.i, initialPosition.j, 0, string.Empty, []));

        while (stack.Count > 0)
        {
            var item = stack.Pop();
            var currentSeen = item.seen;

            if (item.answer > globalMin)
                continue;

            if (map[item.i][item.j] == end)
            {
                if (item.answer < globalMin)
                    globalMin = item.answer;
                possibleAnswers.Add((item.answer, item.path + "A"));
                continue;
            }

            var moves = new List<(int i, int j, Direction newDir, int answer, HashSet<(int, int)> newSeen)>();

            foreach (var possibleDirection in Sides.Keys)
            {
                var direction = Sides[possibleDirection];
                if (!IsWithinBounds(map, (item.i + direction.i, item.j + direction.j)))
                    continue;

                var nextPosition = map[item.i + direction.i][item.j + direction.j];
                if (nextPosition == 'X')
                    continue;

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
                    item.answer + 1,
                    newSeen
                ));
            }

            moves = moves
                .OrderByDescending(move => Math.Abs(move.i - finalPosition.i) + Math.Abs(move.j - finalPosition.j))
                .ToList();

            foreach (var move in moves)
            {
                stack.Push((move.i, move.j, move.answer, item.path + DirectionToPath[move.newDir], move.newSeen));
            }
        }

        return possibleAnswers.Where(x => x.Item1 == globalMin).Select(x => x.path).Distinct().ToList();
    }

    internal static Dictionary<Direction, string> DirectionToPath = new()
    {
        { Direction.D, "v" },
        { Direction.U, "^" },
        { Direction.L, "<" },
        { Direction.R, ">" },
    };

    internal static char[][] NumberKeypad =
    [
        ['7' , '8', '9'],
        ['4' , '5', '6'],
        ['1' , '2', '3'],
        ['X' , '0', 'A']
    ];

    internal static char[][] DirectionalKeypad =
    [
        ['X' , '^', 'A'],
        ['<' , 'v', '>']
    ];
}