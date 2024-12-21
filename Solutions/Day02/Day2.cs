namespace Solutions.Day02;

internal class Day2Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 2);
    public string Name => "Red-Nosed Reports";
    public bool IsActive => false;
    public string Part1Result => "246";

    public async Task ExecuteAsync()
    {
        var reports = await File.ReadAllLinesAsync("./Day02/input.txt");

        var answer = 0;
        foreach (var report in reports)
        {
            var notSafe = Day2.NotSafe(report);

            if (!notSafe)
                answer++;
        }

        Console.WriteLine($"Safe Reports: {answer}");
    }
}

internal class Day2Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 2);
    public string Name => "Red-Nosed Reports";
    public bool IsActive => false;
    public string Part2Result => "318";

    public async Task ExecuteAsync()
    {
        var reports = await File.ReadAllLinesAsync("./Day02/input.txt");

        var answer = 0;
        foreach (var report in reports)
        {
            var notSafe = Day2.NotSafe(report);

            var reportList = report.Split(' ').Select(int.Parse).ToList();
            for (var index = 0; index < reportList.Count; index++)
            {
                if (notSafe)
                {
                    var newList = reportList.ToList();
                    newList.RemoveAt(index);
                    notSafe = Day2.NotSafe(string.Join(' ', newList));
                }
            }

            if (!notSafe)
                answer++;
        }

        Console.WriteLine($"Safe Reports: {answer}");
    }
}

internal static class Day2
{
    internal static bool NotSafe(string report)
    {
        var increasing = false;
        var decreasing = false;
        var levels = report.Split(' ').Select(int.Parse).ToList();

        for (var index = 1; index < levels.Count; index++)
        {
            var lastLevel = levels[index - 1];

            var level = levels[index];

            if (level == lastLevel)
                return true;

            if (lastLevel < level && !increasing && !decreasing)
            {
                increasing = true;
            }

            if (lastLevel > level && !decreasing && !increasing)
            {
                decreasing = true;
            }


            if (increasing)
            {
                if (level < lastLevel)
                    return true;
                if (level - lastLevel is > 3 or < 1)
                {
                    return true;
                }
            }

            if (decreasing)
            {
                if (lastLevel < level)
                    return true;

                if (lastLevel - level is > 3 or < 1)
                {
                    return true;
                }
            }
        }

        return false;
    }
}