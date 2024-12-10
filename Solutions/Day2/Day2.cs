namespace Solutions.Day2;

internal class Day2 : IPart1Challenge, IPart2Challenge
{
    public DateTime Day => new(2024, 12, 2);

    public string Name => "Red-Nosed Reports";

    public bool IsActive => false;

    public string Part1Result => "246";
    public string Part2Result => "318";

    public async Task ExecuteAsync()
    {
        var reports = await File.ReadAllLinesAsync("./Day2/input.txt");

        var safeReports = 0;
        foreach (var report in reports)
        {
            var notSafe = NotSafe(report);

            // part 2 extra
            var reportList = report.Split(' ').Select(int.Parse).ToList();
            for (var index = 0; index < reportList.Count; index++)
            {
                if (notSafe)
                {
                    var newList = reportList.ToList();
                    newList.RemoveAt(index);
                    notSafe = NotSafe(string.Join(' ', newList));
                }
            } 
            // part 2 extra end

            if (!notSafe)
                safeReports++;
        }

        Console.WriteLine($"Safe Reports: {safeReports}"); // part 1: 246, part 2: 318
    }

    private static bool NotSafe(string report)
    {
        var increasing = false;
        var decreasing = false;
        var levels = report.Split(' ').Select(int.Parse).ToList();

        var lastNumber = int.MinValue;
        var notSafe = false;

        for (var index = 0; index < levels.Count; index++)
        {
            if (notSafe)
                continue;

            if (index == 0)
            {
                lastNumber = levels[0];
                continue;
            }

            var level = levels[index];

            if (level == lastNumber)
                notSafe = true;

            if (lastNumber < level && !increasing && !decreasing)
            {
                increasing = true;
            }

            if (lastNumber > level && !decreasing && !increasing)
            {
                decreasing = true;
            }


            if (increasing)
            {
                if (level < lastNumber)
                    notSafe = true;
                if (level - lastNumber is > 3 or < 1)
                {
                    notSafe = true;
                }
            }

            if (decreasing)
            {
                if (lastNumber < level)
                    notSafe = true;

                if (lastNumber - level is > 3 or < 1)
                {
                    notSafe = true;
                }
            }

            lastNumber = level;
        }

        return notSafe;
    }
}