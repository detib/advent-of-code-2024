using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using static Solutions.Day14.Helper;

namespace Solutions.Day14;

internal class Day14Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 14);

    public bool IsActive => false;

    public string Name => "Restroom Redoubt";

    public string Part1Result => "230686500";

    public async Task ExecuteAsync()
    {
        var (initialGridX, initialGridY) = (103, 101);

        var inputLines = await File.ReadAllLinesAsync("./Day14/input.txt");

        var robots = new List<Robot>();
        foreach (var inputLine in inputLines)
        {
            var inputSplitted = inputLine.Split(" ");
            var initialPosition = GetXAndY(inputSplitted[0]);
            var velocity = GetXAndY(inputSplitted[1]);
            var robot = new Robot
            {
                XPosition = initialPosition.X,
                YPosition = initialPosition.Y,
                XVelocity = velocity.X,
                YVelocity = velocity.Y
            };

            robots.Add(robot);
        }

        const int seconds = 100;

        for (var i = 1; i <= seconds; i++)
        {
            foreach (var robot in robots)
            {
                robot.XPosition += robot.XVelocity;
                robot.YPosition += robot.YVelocity;

                if (robot.XPosition >= initialGridY)
                {
                    robot.XPosition -= initialGridY;
                }

                if (robot.YPosition >= initialGridX)
                {
                    robot.YPosition -= initialGridX;
                }

                if (robot.XPosition < 0)
                {
                    robot.XPosition = initialGridY + robot.XPosition;
                }

                if (robot.YPosition < 0)
                {
                    robot.YPosition = initialGridX + robot.YPosition;
                }
            }
        }

        var quadrant1 = robots.Count(x => x is { XPosition: < 50, YPosition: < 51 });
        var quadrant2 = robots.Count(x => x is { XPosition: > 50, YPosition: < 51 });
        var quadrant3 = robots.Count(x => x is { XPosition: < 50, YPosition: > 51 });
        var quadrant4 = robots.Count(x => x is { XPosition: > 50, YPosition: > 51 });

        var answer = quadrant1 * quadrant2 * quadrant3 * quadrant4;

        Console.WriteLine(answer);
    }
}


internal class Day14Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 14);

    public bool IsActive => false;

    public string Name => "Restroom Redoubt";

    public string Part2Result => "7672";

    public async Task ExecuteAsync()
    {
        var (initialGridX, initialGridY) = (103, 101);

        var inputLines = await File.ReadAllLinesAsync("./Day14/input.txt");

        var robots = new List<Robot>();
        foreach (var inputLine in inputLines)
        {
            var inputSplitted = inputLine.Split(" ");
            var initialPosition = GetXAndY(inputSplitted[0]);
            var velocity = GetXAndY(inputSplitted[1]);
            var robot = new Robot
            {
                XPosition = initialPosition.X,
                YPosition = initialPosition.Y,
                XVelocity = velocity.X,
                YVelocity = velocity.Y
            };

            robots.Add(robot);
        }

        const int seconds = 8000;
        for (var i = 1; i <= seconds; i++)
        {
            foreach (var robot in robots)
            {
                robot.XPosition += robot.XVelocity;
                robot.YPosition += robot.YVelocity;

                if (robot.XPosition >= initialGridY)
                {
                    robot.XPosition -= initialGridY;
                }

                if (robot.YPosition >= initialGridX)
                {
                    robot.YPosition -= initialGridX;
                }

                if (robot.XPosition < 0)
                {
                    robot.XPosition = initialGridY + robot.XPosition;
                }

                if (robot.YPosition < 0)
                {
                    robot.YPosition = initialGridX + robot.YPosition;
                }
            }

            if (false)
            {
                var map = CreateMatrix(initialGridX, initialGridY);
                foreach (var robot1 in robots)
                {
                    var value = map[robot1.YPosition][robot1.XPosition];

                    if (value == ".")
                    {
                        map[robot1.YPosition][robot1.XPosition] = "1";
                        continue;
                    }

                    var intValue = int.Parse(value);

                    map[robot1.YPosition][robot1.XPosition] = (++intValue).ToString();
                }
                if (i is > 7500 and < 7700) // so we do not save too many images
                    SaveMatrixAsImage(map, i);
            }
        }
        Console.WriteLine("Image is found on frame 7672");
    }
}


internal class Robot
{
    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public int XVelocity { get; set; }
    public int YVelocity { get; set; }
}

internal class Helper
{
    internal static (int X, int Y) GetXAndY(string input)
    {
        // p=58,57 v=-51,-38
        var match = Regex.Match(input, "[pv]=(-?\\d+),(-?\\d+)");

        return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    }

    internal static string[][] CreateMatrix(int xLength, int yLength)
    {
        var matrix = new string[xLength][];
        for (var i = 0; i < xLength; i++)
        {
            matrix[i] = new string[yLength];
            for (var j = 0; j < yLength; j++)
            {
                matrix[i][j] = ".";
            }
        }
        return matrix;
    }

    internal static void SaveMatrixAsImage(string[][] matrix, int fileName)
    {
        var height = matrix.Length;
        var width = matrix[0].Length;

        using var bitmap = new Bitmap(width, height);
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var color = matrix[y][x] == "." ? Color.Black : Color.White;
                bitmap.SetPixel(x, y, color);
            }
        }

        var filePath = $"{AppContext.BaseDirectory}..\\..\\..\\Day14\\images\\{fileName}.png";
        bitmap.Save(filePath, ImageFormat.Png);
    }
}