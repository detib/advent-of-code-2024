using System.Numerics;

namespace Solutions.Day07;


internal class Day7Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 7);
    public string Name => "Bridge Repair";
    public bool IsActive => false;
    public string Part1Result => "7579994664753";

    public async Task ExecuteAsync()
    {
        var lines = await File.ReadAllLinesAsync("./Day07/input.txt");

        var possibleLines = new List<Line>();
        foreach (var line in lines)
        {
            var lineSplit = line.Split(": ");
            possibleLines.Add(new Line
            {
                ExpectedResult = BigInteger.Parse(lineSplit[0]),
                Numbers = lineSplit[1].Split(' ').Select(int.Parse).ToList()
            });
        }

        foreach (var possibleLine in possibleLines)
        {
            var totalOperations = possibleLine.Numbers.Count - 1;
            var permutations = GetPermutations(["+", "*"], totalOperations);

            foreach (var permutation in permutations)
            {
                possibleLine.OperationPossible = CheckIfLineIsPossible(possibleLine, permutation.ToList());
                if (possibleLine.OperationPossible)
                    break;
            }
        }

        BigInteger finalValue = 0;
        foreach (var possibleLine in possibleLines)
        {
            if (possibleLine.OperationPossible)
            {
                finalValue += possibleLine.ExpectedResult;
            }
        }

        Console.WriteLine(finalValue); // 7579994664753
    }

    private static bool CheckIfLineIsPossible(Line possibleLine, List<string> operations, int deep = 0)
    {
        var total = CalculateLine(possibleLine, operations);

        return total == possibleLine.ExpectedResult;
    }

    private static BigInteger CalculateLine(Line possibleLine, List<string> operations)
    {
        BigInteger total = possibleLine.Numbers[0];
        for (var index = 0; index < operations.Count; index++)
        {
            int? nextNumber = index == possibleLine.Numbers.Count - 1 ? null : possibleLine.Numbers[index + 1];

            var operation = operations[index];
            if (operation == "+" && nextNumber is not null)
            {
                total += nextNumber.Value;
            }

            if (operation == "*" && nextNumber is not null)
            {
                total *= nextNumber.Value;
            }
        }

        return total;
    }
}

internal class Day7Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 7);
    public string Name => "Bridge Repair";
    public bool IsActive => false;
    public string Part2Result => "438027111276610";

    public async Task ExecuteAsync()
    {
        var lines = await File.ReadAllLinesAsync("./Day07/input.txt");

        var possibleLines = new List<Line>();
        foreach (var line in lines)
        {
            var lineSplit = line.Split(": ");
            possibleLines.Add(new Line
            {
                ExpectedResult = BigInteger.Parse(lineSplit[0]),
                Numbers = lineSplit[1].Split(' ').Select(int.Parse).ToList()
            });
        }

        foreach (var possibleLine in possibleLines)
        {
            var totalOperations = possibleLine.Numbers.Count - 1;
            var permutations = GetPermutations(["+", "*", "||"], totalOperations);

            foreach (var permutation in permutations)
            {
                possibleLine.OperationPossible = CheckIfLineIsPossible(possibleLine, permutation.ToList());
                if (possibleLine.OperationPossible)
                    break;
            }
        }

        BigInteger finalValue = 0;
        foreach (var possibleLine in possibleLines)
        {
            if (possibleLine.OperationPossible)
            {
                finalValue += possibleLine.ExpectedResult;
            }
        }

        Console.WriteLine(finalValue); // 438027111276610
    }

    private static bool CheckIfLineIsPossible(Line possibleLine, List<string> operations)
    {
        var total = CalculateLine(possibleLine, operations);

        return total == possibleLine.ExpectedResult;
    }

    private static BigInteger CalculateLine(Line possibleLine, List<string> operations)
    {
        BigInteger total = possibleLine.Numbers[0];
        for (var index = 0; index < operations.Count; index++)
        {
            int? nextNumber = index == possibleLine.Numbers.Count - 1 ? null : possibleLine.Numbers[index + 1];

            var operation = operations[index];
            if (operation == "||" && nextNumber is not null)
            {
                total = BigInteger.Parse($"{total}{nextNumber}");
            }

            if (operation == "+" && nextNumber is not null)
            {
                total += nextNumber.Value;
            }

            if (operation == "*" && nextNumber is not null)
            {
                total *= nextNumber.Value;
            }
        }

        return total;
    }
}

internal class Line
{
    public BigInteger ExpectedResult { get; set; }

    public bool OperationPossible { get; set; } = false;
    public required List<int> Numbers { get; set; }
}