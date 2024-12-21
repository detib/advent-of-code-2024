global using Solutions;
global using static Solutions.Tools;

using System.Diagnostics;
using System.Reflection;
using Solutions;

var assembly = Assembly.GetExecutingAssembly();

List<IChallenge> challenges = assembly.GetTypes()
    .Where(t => typeof(IChallenge).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
    .Select(t => Activator.CreateInstance(t) as IChallenge).OrderBy(x => x!.Day).ToList()!;

if (challenges.All(x => !x.IsActive))
{
    var maxNameLength = challenges.Max(x => x.Name.Length);
    var maxPart1Length = challenges.OfType<IPart1Challenge>().Max(x => x.Part1Result.ToString().Length);
    var maxPart2Length = challenges.OfType<IPart2Challenge>().Max(x => x.Part2Result.ToString().Length);

    foreach (var group in challenges.GroupBy(x => x.Name))
    {
        var part1Challenge = group.OfType<IPart1Challenge>().FirstOrDefault();
        var part2Challenge = group.OfType<IPart2Challenge>().FirstOrDefault();

        var result = $"Part 1: {(part1Challenge?.Part1Result ?? "???").PadRight(maxPart1Length)} Part 2: {(part2Challenge?.Part2Result ?? "???").PadRight(maxPart2Length)}";

        Console.WriteLine($"{part1Challenge?.Day.Day.ToString() ?? part2Challenge?.Day.Day.ToString() ?? "???",2} - " +
                          $"{(part1Challenge?.Name ?? part2Challenge?.Name ?? "???").PadRight(maxNameLength)} : {result}");
    }
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
    Console.WriteLine($"\n{string.Concat(Enumerable.Repeat('-', 100))}");
}