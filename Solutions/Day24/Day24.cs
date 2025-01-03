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

internal class Day24Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 24);
    public string Name => "Crossed Wires";
    public bool IsActive => false;
    public string Part2Result => "ckj,dbp,fdv,kdf,rpp,z15,z23,z39";

    // https://github.com/robhabraken/advent-of-code-2024/blob/main/solutions/24/part-2/Program.cs
    public async Task ExecuteAsync()
    {
        var input = (await File.ReadAllTextAsync("./Day24/input.txt")).Split("\r\n\r\n");

        var secondPart = input[1].Split("\r\n");

        var wires = new SortedDictionary<string, Wire>();
        var gates = new List<Gate>();

        // read all wires and gates
        foreach (var line in secondPart)
        {
            if (line.Contains("->"))
            {
                var elements = line.Split(' ');
                AddWire(elements[0]);
                AddWire(elements[2]);
                AddWire(elements[4]);

                gates.Add(new Gate(wires[elements[0]], wires[elements[2]], wires[elements[4]], elements[1]));
            }
        }

        var suspiciousGates = new List<Gate>();
        var outputWires = wires.Values.Select(w => w).Where(w => w.Name.StartsWith('z')).ToList();
        foreach (var gate in gates)
        {
            // starting gates should be followed by OR if AND, and by AND if XOR, except for the first one
            if ((gate.Inputs[0].Name.StartsWith('x') || gate.Inputs[1].Name.StartsWith('x')) &&
                (gate.Inputs[0].Name.StartsWith('y') || gate.Inputs[1].Name.StartsWith('y')) &&
                (!gate.Inputs[0].Name.Contains("00") && !gate.Inputs[1].Name.Contains("00")))
                foreach (var secondGate in gates)
                    if (gate.Output == secondGate.Inputs[0] || gate.Output == secondGate.Inputs[1])
                        if ((gate.Op.Equals("AND") && secondGate.Op.Equals("AND")) ||
                            (gate.Op.Equals("XOR") && secondGate.Op.Equals("OR")))
                            suspiciousGates.Add(gate);

            // gates in the middle should not have XOR operators
            if (!gate.Inputs[0].Name.StartsWith('x') && !gate.Inputs[1].Name.StartsWith('x') &&
                !gate.Inputs[0].Name.StartsWith('y') && !gate.Inputs[1].Name.StartsWith('y') &&
                !gate.Output.Name.StartsWith('z') && gate.Op.Equals("XOR"))
                suspiciousGates.Add(gate);

            // gates at the end should always have XOR operators, except for the last one
            if (outputWires.Contains(gate.Output) && !gate.Output.Name.Equals($"z{outputWires.Count - 1}") && !gate.Op.Equals("XOR"))
                suspiciousGates.Add(gate);
        }

        var answer = string.Empty;
        foreach (var sGate in suspiciousGates.OrderBy(x => x.Output.Name))
            answer += $"{sGate.Output.Name},";

        Console.WriteLine(answer[..^1]);

        return;
        
        void AddWire(string wireName)
        {
            if (!wires.ContainsKey(wireName))
                wires.Add(wireName, new Wire(wireName));
        }
    }
}

internal class Gate(Wire in1, Wire in2, Wire output, string op)
{
    public Wire[] Inputs = [in1, in2];
    public Wire Output = output;
    public string Op = op;
}

internal class Wire(string name)
{
    public string Name = name;
}