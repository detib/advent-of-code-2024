using System.Text.RegularExpressions;

namespace Solutions.Day3;

internal class Day3Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 3);
    public string Name => "Mull It Over";
    public bool IsActive => false;
    public string Part1Result => "163931492";

    public async Task ExecuteAsync()
    {
        var sections = await File.ReadAllLinesAsync("./Day3/input.txt");

        long total = 0;
        foreach (var section in sections)
        {
            var results = Regex.Matches(section, @"mul\((\d+),(\d+)\)");

            foreach (Match result in results)
            {
                Console.WriteLine($"{result.Groups[0]} : {result.Groups[1]}, {result.Groups[2]}");
                total += int.Parse(result.Groups[1].Value) * int.Parse(result.Groups[2].Value);
            }
        }
        Console.WriteLine(total);
    }
}

internal class Day3Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 3);
    public string Name => "Mull It Over";
    public bool IsActive => false;
    public string Part2Result => "76911921";

    public async Task ExecuteAsync()
    {
        var sections = await File.ReadAllLinesAsync("./Day3/input.txt");
        var section = "do()" + string.Join("", sections); // add do() to "enable" the start of the program
        long total = 0;

        // not needed after we joined the string, a don't() operation at the end of the first line disables the program for the next line
        //foreach (var section in sections)
        //{
        //var results = Regex.Matches(section, @"(do\(\))(!(don't\(\)))?.*?mul\((\d+),(\d+)\)");
        var results = Regex.Matches(section, @"do\(\)(.*?)(don't\(\)|$)");

        foreach (Match result in results)
        {
            var betweenDoAndDont = Regex.Matches(result.Groups[1].Value, @"mul\((\d+),(\d+)\)");
            foreach (Match match in betweenDoAndDont)
            {
                Console.WriteLine($"{match.Groups[0]} : {match.Groups[1]}, {match.Groups[2]}");
                total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
        }

        // enable the first row by manually adding a do() operation so this is not needed
        //var beforeAnyDoOrDont = Regex.Match(section, @"^(.*?)(do\(\)|don't\(\))");

        //var firstPartMatches = Regex.Matches(beforeAnyDoOrDont.Groups[1].Value, @"mul\((\d+),(\d+)\)");
        //foreach (Match result in firstPartMatches)
        //{
        //    Console.WriteLine($"{result.Groups[0]} : {result.Groups[1]}, {result.Groups[2]}");
        //    total += int.Parse(result.Groups[1].Value) * int.Parse(result.Groups[2].Value);
        //}
        //}
        Console.WriteLine(total);
    }
}