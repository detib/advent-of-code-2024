using Solutions;
using System.Diagnostics;
using System.Reflection;

var assembly = Assembly.GetExecutingAssembly();

List<IChallenge> challenges = assembly.GetTypes()
    .Where(t => typeof(IChallenge).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
    .Select(t => Activator.CreateInstance(t) as IChallenge).OrderBy(x => x!.Day).ToList()!;

if (challenges.All(x => !x.IsActive))
{
    Console.WriteLine(string.Join("\n", challenges.Select(x =>
    {
        var result = x switch
        {
            IPart1Challenge part1 and IPart2Challenge part2 => $"Part1: {part1.Part1Result}, Part2: {part2.Part2Result}",
            IPart1Challenge part1Only => $"Part1: {part1Only.Part1Result}",
            IPart2Challenge part2Only => $"Part2: {part2Only.Part2Result}",
            _ => "Unknown Challenge Type"
        };
        return $"{x.Day.Day} - {x.Name} : {result}";
    })));
}

foreach (var challenge in challenges.Where(challenge => challenge.IsActive))
{
    Console.WriteLine($"Starting challenge: {challenge.Name ?? challenge.GetType().Name}");

    var stopwatch = Stopwatch.StartNew();
    await challenge.ExecuteAsync();
    stopwatch.Stop();

    var elapsedTime = stopwatch.Elapsed;
    var friendlyTime = $"{(elapsedTime.Hours > 0 ? $"{elapsedTime.Hours}h " : "")}" +
                       $"{(elapsedTime.Minutes > 0 ? $"{elapsedTime.Minutes}m " : "")}" +
                       $"{(elapsedTime.Seconds > 0 ? $"{elapsedTime.Seconds}s " : "")}" +
                       $"{(elapsedTime.Milliseconds > 0 ? $"{elapsedTime.Milliseconds}ms" : "")}";

    Console.WriteLine($"Finished challenge: {challenge.Name ?? challenge.GetType().Name} in {friendlyTime.Trim()}");
}