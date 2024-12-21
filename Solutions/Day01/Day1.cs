namespace Solutions.Day01;

internal class Day1 : IPart1Challenge, IPart2Challenge
{
    public DateTime Day => new(2024, 12, 1);
    public string Name => "Historian Hysteria";
    public bool IsActive => false;
    public string Part1Result => "1341714";
    public string Part2Result => "27384707";

    public async Task ExecuteAsync()
    {
        var input = await File.ReadAllLinesAsync("./Day01/input.txt");

        var firstList = new List<int>(input.Length);
        var secondList = new List<int>(input.Length);

        foreach (var line in input)
        {
            var numbers = line.Split("   ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            firstList.Add(numbers[0]);
            secondList.Add(numbers[1]);
        }

        firstList = firstList.OrderBy(x => x).ToList();
        secondList = secondList.OrderBy(x => x).ToList();

        var difference = firstList.Select((t, i) => Math.Abs(t - secondList[i])).Sum();

        Console.WriteLine($"Difference: {difference}");

        var rightListCounts = new Dictionary<int, int>();

        foreach (var rightNumber in secondList)
        {
            if (!rightListCounts.TryAdd(rightNumber, 1))
            {
                rightListCounts[rightNumber]++;
            }
        }

        var total = 0;
        foreach (var leftNumber in firstList)
        {
            var rightCount = rightListCounts.TryGetValue(leftNumber, out var value);
            total += leftNumber * (rightCount ? value : 0);
        }

        Console.WriteLine($"Similarity: {total}");
    }
}