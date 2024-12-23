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

internal class Day22Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 22);
    public string Name => "Monkey Market";
    public bool IsActive => false;
    public string Part2Result => "1444";

    public async Task ExecuteAsync()
    {
        var input = (await File.ReadAllLinesAsync("./Day22/input.txt")).Select(int.Parse).ToList();

        var allnumbers = input.Select(x => CalculateSecretNumber(x)).ToList();

        var dictionary = new Dictionary<string, BigInteger>();
        foreach (var number in allnumbers)
        {
            foreach (var x in number)
            {
                if (dictionary.ContainsKey(x.Key))
                {
                    dictionary[x.Key] += x.Value;
                }
                else
                {
                    dictionary[x.Key] = x.Value;
                }
            }
        }

        var answer = dictionary.Values.Max();

        Console.WriteLine(answer);
    }

    private static Dictionary<string, int> CalculateSecretNumber(BigInteger secretNumber)
    {
        var prices = new List<(BigInteger secretNumber, BigInteger price, BigInteger changeFromPrevious)>
        {
            (secretNumber, secretNumber % 10, 0)
        };

        var sequencesWithPrices = new Dictionary<string, int>();
        for (var i = 0; i < 1999; i++)
        {
            secretNumber = ((secretNumber * 64) ^ secretNumber) % 16777216;
            secretNumber = ((secretNumber / 32) ^ secretNumber) % 16777216;
            secretNumber = ((secretNumber * 2048) ^ secretNumber) % 16777216;

            prices.Add((secretNumber, secretNumber % 10, secretNumber % 10 - prices[i].price));
            if (i > 3)
            {
                BigInteger[] sequence = [
                    prices[i - 3].changeFromPrevious,
                    prices[i - 2].changeFromPrevious,
                    prices[i - 1].changeFromPrevious,
                    prices[i].changeFromPrevious
                ];

                var seq = string.Join(',', sequence);
                sequencesWithPrices.TryAdd(seq, (int)prices[i].price);
            }
        }

        return sequencesWithPrices;
    }
}