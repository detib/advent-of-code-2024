using System.Numerics;
using System.Text.RegularExpressions;

namespace Solutions.Day24;

internal class Day24Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 24);
    public string Name => "Crossed Wires";
    public bool IsActive => false;
    public string Part1Result => "57632654722854";

    public async Task ExecuteAsync()
    {
        var input = (await File.ReadAllTextAsync("./Day24/input.txt")).Split("\r\n\r\n");

        var dictionary = new Dictionary<string, bool>();

        var firstPart = input[0].Split("\r\n");

        foreach (var wire in firstPart.Select(x => x.Split(": ")))
        {
            dictionary.TryAdd(wire[0], wire[1] == "1");
        }

        var operationsQueue = new Queue<(string leftOperand, string gate, string rightOperand, string output)>();
        var secondPart = input[1].Split("\r\n");

        foreach (var gate in secondPart)
        {
            var regex = Regex.Match(gate, @"(...)\s(.{2,3})\s(...)\s->\s(...)");
            operationsQueue.Enqueue((regex.Groups[1].Value, regex.Groups[2].Value, regex.Groups[3].Value, regex.Groups[4].Value));
        }

        while (operationsQueue.Count > 0)
        {
            var nextOperation = operationsQueue.Dequeue();

            if (!dictionary.TryGetValue(nextOperation.leftOperand, out var leftOperandValue))
            {
                operationsQueue.Enqueue(nextOperation);
                continue;
            }

            if (!dictionary.TryGetValue(nextOperation.rightOperand, out var rightOperandValue))
            {
                operationsQueue.Enqueue(nextOperation);
                continue;
            }

            if (nextOperation.gate == "AND")
            {
                var result = leftOperandValue && rightOperandValue;
                dictionary[nextOperation.output] = result;
                continue;
            }

            if (nextOperation.gate == "XOR")
            {
                var result = leftOperandValue != rightOperandValue;
                dictionary[nextOperation.output] = result;
                continue;
            }

            if (nextOperation.gate == "OR")
            {
                var result = leftOperandValue || rightOperandValue;
                dictionary[nextOperation.output] = result;
            }
        }

        var zWires = dictionary.Keys.Where(x => x.StartsWith('z')).OrderByDescending(x => x).Select(x => dictionary[x]).ToList();

        var answer = ConvertToDecimal(zWires);

        Console.WriteLine(answer);
    }

    private static BigInteger ConvertToDecimal(List<bool> binaryBools)
    {
        BigInteger decimalValue = 0;
        var power = binaryBools.Count - 1;

        foreach (var bit in binaryBools)
        {
            if (bit)
            {
                decimalValue += BigInteger.Pow(2, power);
            }
            power--;
        }

        return decimalValue;
    }

}
