using System.Numerics;

namespace Solutions.Day22;

internal class Day22Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 22);
    public string Name => "Monkey Market";
    public bool IsActive => false;
    public string Part1Result => "12664695565";

    public async Task ExecuteAsync()
    {
        var input = (await File.ReadAllLinesAsync("./Day22/input.txt")).Select(int.Parse).ToList();

        BigInteger answer = 0;
        foreach (var secretNumber in input)
        {
            answer += CalculateSecretNumber(secretNumber);
        }
        Console.WriteLine(answer);
    }

    private static BigInteger CalculateSecretNumber(BigInteger secretNumber)
    {
        for (var i = 0; i < 2000; i++)
        {
            secretNumber = ((secretNumber * 64) ^ secretNumber) % 16777216;
            secretNumber = ((secretNumber / 32) ^ secretNumber) % 16777216;
            secretNumber = ((secretNumber * 2048) ^ secretNumber) % 16777216;
        }

        return secretNumber;
    }
}
