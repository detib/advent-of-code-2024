namespace Solutions.Day5;

internal class Day5Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 5);
    public string Name => "Print Queue";
    public bool IsActive => false;
    public string Part1Result => "5268";

    public async Task ExecuteAsync()
    {
        var allInput = await File.ReadAllTextAsync("./Day5/input.txt");

        var input = allInput.Split("\r\n\r\n");

        var nextRules = input[0].Split("\r\n").Select(x => x.Split("|")).GroupBy(x => x[0], y => y[1]).ToDictionary(x => x.Key, x => x.ToList());
        var backRules = input[0].Split("\r\n").Select(x => x.Split("|")).GroupBy(x => x[1], y => y[0]).ToDictionary(x => x.Key, x => x.ToList());
        var updates = input[1].Split("\r\n");

        var answer = 0;
        foreach (var update in updates)
        {
            var updateNumbers = update.Split(',').ToList();
            var isRightOrder = true;
            for (var index = 0; index < updateNumbers.Count; index++)
            {
                var number = updateNumbers[index];

                nextRules.TryGetValue(number, out var nextRulesForNumber);
                backRules.TryGetValue(number, out var backRulesForNumber);

                if (index > 0 && nextRulesForNumber is not null)
                {
                    var previousUpdates = updateNumbers[..(index - 1)];

                    var intersection = previousUpdates.Intersect(nextRulesForNumber);
                    if (intersection.Any())
                    {
                        isRightOrder = false;
                        break;
                    }
                }

                if (index < updateNumbers.Count && backRulesForNumber is not null)
                {
                    var nextUpdates = updateNumbers[(index + 1)..];
                    var intersection = nextUpdates.Intersect(backRulesForNumber);
                    if (intersection.Any())
                    {
                        isRightOrder = false;
                        break;
                    }
                }
            }

            if (isRightOrder)
            {
                answer += int.Parse(updateNumbers[(int)Math.Floor((decimal)updateNumbers.Count / 2)]);
            }
        }

        Console.WriteLine(answer); // 5268
    }
}

internal class Day5Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 5);
    public string Name => "Print Queue";
    public bool IsActive => false;
    public string Part2Result => "5799";

    public async Task ExecuteAsync()
    {
        var allInput = await File.ReadAllTextAsync("./Day5/input.txt");

        var input = allInput.Split("\r\n\r\n");

        var allRules = input[0].Split("\r\n");
        var nextRules = input[0].Split("\r\n").Select(x => x.Split("|")).GroupBy(x => x[0], y => y[1]).ToDictionary(x => x.Key, x => x.Distinct().ToList());
        var backRules = input[0].Split("\r\n").Select(x => x.Split("|")).GroupBy(x => x[1], y => y[0]).ToDictionary(x => x.Key, x => x.Distinct().ToList());
        var updates = input[1].Split("\r\n");

        var incorrectlyOrderedUpdates = new List<string>();
        foreach (var update in updates)
        {
            var updateNumbers = update.Split(',').ToList();
            var isRightOrder = true;
            for (var index = 0; index < updateNumbers.Count; index++)
            {
                var number = updateNumbers[index];

                nextRules.TryGetValue(number, out var nextRulesForNumber);
                backRules.TryGetValue(number, out var backRulesForNumber);

                if (index > 0 && nextRulesForNumber is not null)
                {
                    var previousUpdates = updateNumbers[..(index - 1)];

                    var intersection = previousUpdates.Intersect(nextRulesForNumber);
                    if (intersection.Any())
                    {
                        isRightOrder = false;
                        break;
                    }
                }

                if (index < updateNumbers.Count && backRulesForNumber is not null)
                {
                    var nextUpdates = updateNumbers[(index + 1)..];
                    var intersection = nextUpdates.Intersect(backRulesForNumber);
                    if (intersection.Any())
                    {
                        isRightOrder = false;
                        break;
                    }
                }
            }

            if (!isRightOrder)
            {
                incorrectlyOrderedUpdates.Add(update);
            }
        }

        var answer = 0;
        foreach (var incorrectUpdate in incorrectlyOrderedUpdates)
        {
            var updateNumbers = incorrectUpdate.Split(',').ToList();

            // bubble sort based on the all rules
            // if the combination of the {nextNumber}|{currentNumber} exists in the allrules
            // that means that those two numbers need to be swapped
            var length = updateNumbers.Count;
            bool sortAgain;
            do
            {
                sortAgain = false;
                for (var index = 0; index < length - 1; index++)
                {
                    var currentNumber = updateNumbers[index];
                    var nextNumber = updateNumbers[index + 1];

                    if (allRules.Contains($"{nextNumber}|{currentNumber}"))
                    {
                        updateNumbers[index] = nextNumber;
                        updateNumbers[index + 1] = currentNumber;
                        sortAgain = true;
                        index = 0;
                    }
                }

                length--;

            } while (sortAgain);

            //// linq ordering of the bubble sort above
            //updateNumbers = updateNumbers
            //    .OrderBy(number =>
            //        updateNumbers.Count(other => allRules.Contains($"{number}|{other}")))
            //    .ToList();

            answer += int.Parse(updateNumbers[(int)Math.Floor((decimal)updateNumbers.Count / 2)]);
        }

        Console.WriteLine(answer); // 5799
    }
}