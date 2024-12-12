using System.Diagnostics;
using System.Reflection;
using Solutions;

var assembly = Assembly.GetExecutingAssembly();

List<IChallenge> challenges = assembly.GetTypes()
    .Where(t => typeof(IChallenge).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
    .Select(t => Activator.CreateInstance(t) as IChallenge).OrderBy(x => x!.Day).ToList()!;

if (challenges.All(x => !x.IsActive))
{
    int maxNameLength = challenges.Max(x => x.Name.Length);
    int maxPart1Length = challenges.OfType<IPart1Challenge>().Max(x => x.Part1Result.ToString().Length);
    int maxPart2Length = challenges.OfType<IPart2Challenge>().Max(x => x.Part2Result.ToString().Length);

    Console.WriteLine(string.Join("\n", challenges.Select(x =>
    {
        var result = x switch
        {
            IPart1Challenge part1 and IPart2Challenge part2 =>
                $"Part1: {part1.Part1Result.PadRight(maxPart1Length)} Part2: {part2.Part2Result.PadRight(maxPart2Length)}",
            IPart1Challenge part1Only =>
                $"Part1: {part1Only.Part1Result.PadRight(maxPart1Length)}",
            IPart2Challenge part2Only =>
                $"Part2: {part2Only.Part2Result.PadRight(maxPart2Length)}",
            _ => "Unknown Challenge Type"
        };
        return $"{x.Day.Day.ToString(), 2} - {x.Name.PadRight(maxNameLength)} : {result}";
    })));
}

foreach (var challenge in challenges.Where(challenge => challenge.IsActive))
{
    var challengeType = challenge switch
    {
        IPart1Challenge and IPart2Challenge => "Part 1 and Part 2",
        IPart1Challenge => "Part 1",
        IPart2Challenge => "Part 2",
        _ => "Unknown Type"
    };

    Console.WriteLine($"Starting challenge: {challenge.Name} ({challengeType})");

    var stopwatch = Stopwatch.StartNew();
    await challenge.ExecuteAsync();
    stopwatch.Stop();

    var elapsedTime = stopwatch.Elapsed;
    var friendlyTime = $"{(elapsedTime.Hours > 0 ? $"{elapsedTime.Hours}h " : "")}" +
                       $"{(elapsedTime.Minutes > 0 ? $"{elapsedTime.Minutes}m " : "")}" +
                       $"{(elapsedTime.Seconds > 0 ? $"{elapsedTime.Seconds}s " : "")}" +
                       $"{(elapsedTime.Milliseconds > 0 ? $"{elapsedTime.Milliseconds}ms" : "")}";

    Console.WriteLine($"Finished challenge: {challenge.Name} ({challengeType}) in {friendlyTime.Trim()}");
}