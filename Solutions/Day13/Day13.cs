using System.Text.RegularExpressions;
using static Solutions.Day13.Helper;

namespace Solutions.Day13;

internal class Day13Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 13);
    public bool IsActive => false;
    public string Name => "Claw Contraption";
    public string Part1Result => "32067";
    public async Task ExecuteAsync()
    {
        var lines = (await File.ReadAllTextAsync("./Day13/input.txt")).Split("\r\n\r\n");

        var clawMachines = new List<ClawMachine>();
        foreach (var clawMachine in lines)
        {
            var clawMachineLines = clawMachine.Split("\r\n");

            var buttonAValues = GetPositionValue(clawMachineLines[0]);
            var buttonA = new Button
            {
                XMove = buttonAValues.X,
                YMove = buttonAValues.Y
            };

            var buttonBValues = GetPositionValue(clawMachineLines[1]);
            var buttonB = new Button
            {
                XMove = buttonBValues.X,
                YMove = buttonBValues.Y
            };

            var clawMachinePrizeValues = GetPositionValue(clawMachineLines[2]);

            var newClawMachine = new ClawMachine
            {
                A = buttonA,
                B = buttonB,
                Prize = new Prize
                {
                    XPosition = clawMachinePrizeValues.X,
                    YPosition = clawMachinePrizeValues.Y
                }
            };
            clawMachines.Add(newClawMachine);
        }

        long answer = 0;
        foreach (var clawMachine in clawMachines)
        {
            var (xMoves, yMoves) = clawMachine.GetMoves();
            var isX = xMoves * clawMachine.A.XMove + yMoves * clawMachine.B.XMove;
            var isY = xMoves * clawMachine.A.YMove + yMoves * clawMachine.B.YMove;
            if (isX == clawMachine.Prize.XPosition &&
                isY == clawMachine.Prize.YPosition)
                answer += 3 * xMoves + yMoves;
        }

        Console.WriteLine(answer);
    }
}

internal class Day13Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 13);
    public bool IsActive => false;
    public string Name => "Claw Contraption";
    public string Part2Result => "92871736253789";

    public async Task ExecuteAsync()
    {
        var lines = (await File.ReadAllTextAsync("./Day13/input.txt")).Split("\r\n\r\n");

        var clawMachines = new List<ClawMachine>();
        foreach (var clawMachine in lines)
        {
            var clawMachineLines = clawMachine.Split("\r\n");

            var buttonAValues = GetPositionValue(clawMachineLines[0]);
            var buttonA = new Button
            {
                XMove = buttonAValues.X,
                YMove = buttonAValues.Y
            };

            var buttonBValues = GetPositionValue(clawMachineLines[1]);
            var buttonB = new Button
            {
                XMove = buttonBValues.X,
                YMove = buttonBValues.Y
            };

            var clawMachinePrizeValues = GetPositionValue(clawMachineLines[2]);

            var newClawMachine = new ClawMachine
            {
                A = buttonA,
                B = buttonB,
                Prize = new Prize
                {
                    XPosition = clawMachinePrizeValues.X + 10000000000000,
                    YPosition = clawMachinePrizeValues.Y + 10000000000000
                }
            };
            clawMachines.Add(newClawMachine);
        }

        long answer = 0;
        foreach (var clawMachine in clawMachines)
        {
            var (xMoves, yMoves) = clawMachine.GetMoves();
            var isX = xMoves * clawMachine.A.XMove + yMoves * clawMachine.B.XMove;
            var isY = xMoves * clawMachine.A.YMove + yMoves * clawMachine.B.YMove;
            if (isX == clawMachine.Prize.XPosition &&
                isY == clawMachine.Prize.YPosition)
                answer += 3 * xMoves + yMoves;
        }

        Console.WriteLine(answer);
    }
}

internal static class Helper
{
    internal static (long X, long Y) GetPositionValue(string input)
    {
        var matchX = Regex.Match(input, "X[+=](\\d+)");
        var matchY = Regex.Match(input, "Y[+=](\\d+)");

        return (long.Parse(matchX.Groups[1].Value), long.Parse(matchY.Groups[1].Value));
    }
}

internal record Button
{
    public long XMove { get; set; }
    public long YMove { get; set; }
}

internal record Prize
{
    public long XPosition { get; set; }
    public long YPosition { get; set; }
}

internal record ClawMachine
{
    public required Button A { get; set; }
    public required Button B { get; set; }
    public required Prize Prize { get; set; }

    public (long xMoves, long yMoves) GetMoves()
    {
        var d = A.XMove * B.YMove - A.YMove * B.XMove;

        var dx = Prize.XPosition * B.YMove - Prize.YPosition * B.XMove;

        var dy = A.XMove * Prize.YPosition - A.YMove * Prize.XPosition;

        return (dx / d, dy / d);
    }
}