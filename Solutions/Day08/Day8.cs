namespace Solutions.Day08;

internal class Day8Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 8);
    public string Name => "Resonant Collinearity";
    public bool IsActive => false;
    public string Part1Result => "344";

    public async Task ExecuteAsync()
    {
        var map = await ReadCharMap("./Day08/input.txt");

        var antinodes = new List<(int, int)>();

        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                var currentAntenna = map[i][j];
                if (currentAntenna != '.')
                {
                    for (var k = 0; k < map.Length; k++)
                    {
                        for (var l = 0; l < map[i].Length; l++)
                        {
                            var nextAntenna = map[k][l];
                            if (nextAntenna == currentAntenna && i != k && l != j)
                            {
                                var distanceX = k - i;
                                var distanceY = l - j;

                                var leftAntinodeX = i - distanceX;
                                var leftAntinodeY = j - distanceY;

                                var rightAntinodeX = k + distanceX;
                                var rightAntinodeY = l + distanceY;

                                if (IsWithinBounds(map, (leftAntinodeX, leftAntinodeY)))
                                {
                                    antinodes.Add((leftAntinodeX, leftAntinodeY));
                                }

                                if (IsWithinBounds(map, (rightAntinodeX, rightAntinodeY)))
                                {
                                    antinodes.Add((rightAntinodeX, rightAntinodeY));
                                }
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine(antinodes.Distinct().Count()); // 344
    }
}

internal class Day8Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 8);
    public string Name => "Resonant Collinearity";
    public bool IsActive => false;
    public string Part2Result => "1182";

    public async Task ExecuteAsync()
    {
        var map = await ReadCharMap("./Day08/input.txt");

        var antinodes = new List<(int, int)>();

        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                var currentAntenna = map[i][j];
                if (currentAntenna != '.')
                {
                    for (int k = 0; k < map.Length; k++)
                    {
                        for (int l = 0; l < map[i].Length; l++)
                        {
                            var nextAntenna = map[k][l];
                            if (nextAntenna == currentAntenna && i != k && l != j)
                            {
                                antinodes.Add((i, j));
                                antinodes.Add((k, l));
                                var distanceX = k - i;
                                var distanceY = l - j;

                                // try to find bounds of array based on position and distance
                                // or just loop until 100 and multiply the distance
                                // check if out of bounds before adding to antinodes
                                for (var q = 0; q < 100; q++)
                                {
                                    var leftAntinodeX = i - (distanceX * q);
                                    var leftAntinodeY = j - (distanceY * q);

                                    var rightAntinodeX = k + (distanceX * q);
                                    var rightAntinodeY = l + (distanceY * q);

                                    if (IsWithinBounds(map, (leftAntinodeX, leftAntinodeY)))
                                    {
                                        antinodes.Add((leftAntinodeX, leftAntinodeY));
                                    }

                                    if (IsWithinBounds(map, (rightAntinodeX, rightAntinodeY)))
                                    {
                                        antinodes.Add((rightAntinodeX, rightAntinodeY));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        var ac = antinodes.Distinct().ToHashSet();
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] != '.')
                {
                    Console.Write(map[i][j]);
                    continue;
                }
                Console.Write(ac.Contains((i, j)) ? "#" : ".");
            }
            Console.WriteLine();
        }

        Console.WriteLine(antinodes.Distinct().Count()); // 1182
    }
}