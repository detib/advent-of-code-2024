using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Solutions.Day17;

internal class Day17Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 17);
    public bool IsActive => false;
    public string Name => "Chronospatial Computer";
    public string Part1Result => "1,0,2,0,5,7,2,1,3";
    public async Task ExecuteAsync()
    {
        var lines = (await File.ReadAllTextAsync("./Day17/input.txt")).Split("\r\n\r\n");
        var registers = lines[0].Split("\r\n");
        var registerA = int.Parse(registers[0].Split("A: ")[1]);
        var registerB = int.Parse(registers[1].Split("B: ")[1]);
        var registerC = int.Parse(registers[2].Split("C: ")[1]);

        var program = lines[1].Split("Program: ")[1].Split(',').Select(int.Parse).ToArray();

        var answer = new List<int>();
        for (var instructionPointer = 1; instructionPointer < program.Length; instructionPointer++)
        {
            var opcode = program[instructionPointer - 1];
            var operand = program[instructionPointer];

            if (opcode == 0)
            {
                var numerator = registerA;
                var exponent = GetComboOperand(operand);

                var denominator = Math.Pow(2, exponent);

                registerA = (int)(numerator / denominator);
            }

            if (opcode == 1)
            {
                registerB ^= operand;
            }

            if (opcode == 2)
            {
                registerB = GetComboOperand(operand) % 8;
            }

            if (opcode == 3 && registerA != 0)
            {
                instructionPointer = operand;
                continue;
            }

            if (opcode == 4)
            {
                registerB ^= registerC;
            }

            if (opcode == 5)
            {
                var outCommand = GetComboOperand(operand) % 8;
                answer.Add(outCommand);
            }

            if (opcode == 6)
            {
                var numerator = registerA;
                var exponent = GetComboOperand(operand);

                var denominator = Math.Pow(2, exponent);

                registerB = (int)(numerator / denominator);
            }

            if (opcode == 7)
            {
                var numerator = registerA;
                var exponent = GetComboOperand(operand);

                var denominator = Math.Pow(2, exponent);

                registerC = (int)(numerator / denominator);
            }



            instructionPointer++;
        }

        Console.WriteLine(string.Join(',', answer));

        return;

        int GetComboOperand(int operand)
        {
            return operand switch
            {
                4 => registerA,
                5 => registerB,
                6 => registerC,
                _ => operand
            };
        }
    }
}


internal class Day17Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 17);
    public bool IsActive => false;
    public string Name => "Chronospatial Computer";
    public string Part2Result => "265652340990875";

    public async Task ExecuteAsync()
    {
        var lines = (await File.ReadAllTextAsync("./Day17/input.txt")).Split("\r\n\r\n");
        var registers = lines[0].Split("\r\n");
        var registerA = int.Parse(registers[0].Split("A: ")[1]);
        var registerB = int.Parse(registers[1].Split("B: ")[1]);
        var registerC = int.Parse(registers[2].Split("C: ")[1]);

        var program = lines[1].Split("Program: ")[1].Split(',').Select(int.Parse).ToArray();
        List<BigInteger> candidates = [0];
        for (BigInteger i = 0; i < program.Length; i++)
        {
            var nextCandidates = new List<BigInteger>();
            foreach (var candidate in candidates.ToList())
            {
                for (var j = 0; j < 8; j++)
                {
                    var target = candidate * 8 + j;
                    var programOutput = GetAnswerForProgram(program, target, registerB, registerC);

                    var a = program[^(((int)i) + 1)..];
                    if (a.SequenceEqual(programOutput))
                    {
                        nextCandidates.Add(target);
                    }
                }
            }

            candidates = nextCandidates.ToList();
        }

        var answer = candidates.Min();

        Console.WriteLine(answer);
    }

    public static List<int> GetAnswerForProgram(int[] program, BigInteger registerA, BigInteger registerB, BigInteger registerC)
    {
        var programOutput = new List<int>();
        for (int instructionPointer = 1; instructionPointer < program.Length; instructionPointer++)
        {
            var opcode = program[instructionPointer - 1];
            var operand = program[instructionPointer];

            if (opcode == 0)
            {
                var numerator = registerA;
                var exponent = GetComboOperand(operand);

                var denominator = BigInteger.Pow(2, (int)exponent);

                registerA = numerator / denominator;
            }

            if (opcode == 1)
            {
                registerB ^= operand;
            }

            if (opcode == 2)
            {
                registerB = GetComboOperand(operand) % 8;
            }

            if (opcode == 3 && registerA != 0)
            {
                instructionPointer = operand;
                continue;
            }

            if (opcode == 4)
            {
                registerB ^= registerC;
            }

            if (opcode == 5)
            {
                var outCommand = GetComboOperand(operand) % 8;
                programOutput.Add((int)outCommand);
            }

            if (opcode == 6)
            {
                var numerator = registerA;
                var exponent = GetComboOperand(operand);

                var denominator = BigInteger.Pow(2, (int)exponent);

                registerB = numerator / denominator;
            }

            if (opcode == 7)
            {
                var numerator = registerA;
                var exponent = GetComboOperand(operand);

                var denominator = BigInteger.Pow(2, (int)exponent);

                registerC = numerator / denominator;
            }



            instructionPointer++;
        }

        return programOutput;

        BigInteger GetComboOperand(long operand)
        {
            return operand switch
            {
                4 => registerA,
                5 => registerB,
                6 => registerC,
                _ => operand
            };
        }
    }
}