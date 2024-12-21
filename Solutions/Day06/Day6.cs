using System.Collections.Concurrent;
using System.Runtime;
using static Solutions.Day06.Helper;

namespace Solutions.Day06;

internal class Day6Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 6);
    public string Name => "Guard Gallivant";
    public bool IsActive => false;
    public string Part1Result => "5131";

    public async Task ExecuteAsync()
    {
        var map = await File.ReadAllLinesAsync("./Day06/input.txt");
        const char UP = '^';
        const char RI = '>';
        const char DO = 'v';
        const char LE = '<';

        var direction = string.Empty;
        var visited = new List<(int, int)>();
        var (guard, i, j) = GetGuardPosition(map);

        var guardNotOut = true;
        visited.Add((i, j));
        while (guardNotOut)
        {
            if (guard is UP)
            {
                while (i-- > 0)
                {
                    if (i < 0)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && !visited.Contains((i, j)))
                    {
                        visited.Add((i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        i++; // return to previous position
                        guard = RI;
                        break;
                    }
                }
            }

            if (guard is RI)
            {
                while (j++ < map[i].Length)
                {
                    if (j >= map[i].Length)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && !visited.Contains((i, j)))
                    {
                        visited.Add((i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        j--;
                        guard = DO;
                        break;
                    }

                }
            }

            if (guard is DO)
            {
                while (i++ < map.Length)
                {
                    if (i >= map.Length)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && !visited.Contains((i, j)))
                    {
                        visited.Add((i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        i--;
                        guard = LE;
                        break;
                    }

                }
            }

            if (guard is LE)
            {
                while (j-- > 0)
                {
                    if (j < 0)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && !visited.Contains((i, j)))
                    {
                        visited.Add((i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        j++;
                        guard = UP;
                        break;
                    }

                }
            }
        };

        Console.WriteLine(visited.Count); // 5131
    }
}

internal class Day6Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 6);
    public string Name => "Guard Gallivant";
    public bool IsActive => false;
    public string Part2Result => "1784";

    private const char Up = '^';
    private const char Ri = '>';
    private const char Do = 'v';
    private const char Le = '<';

    public async Task ExecuteAsync()
    {
        var map = await File.ReadAllLinesAsync("./Day06/input.txt");
        var visited = new List<(char, int, int)>();
        var (guard, i, j) = GetGuardPosition(map);

        var initialGuardDirection = guard;
        var initialX = i;
        var initialY = j;

        var obstructions = new List<(int, int)>();
        var guardNotOut = true;
        visited.Add((guard, i, j));
        while (guardNotOut)
        {
            Console.Clear();
            Console.WriteLine($"Visited Count: {visited.Count}");
            if (guard is Up)
            {
                while (i-- > 0)
                {
                    if (i < 0)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (visited.Contains((Ri, i, j)) && !obstructions.Contains((i - 1, j)))
                    {
                        obstructions.Add((i - 1, j));
                    }

                    if (map[i][j] != '#' && !visited.Contains((guard, i, j)))
                    {
                        visited.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        i++; // return to previous position
                        guard = Ri;
                        break;
                    }
                }
            }

            if (guard is Ri)
            {
                while (j++ < map[i].Length)
                {
                    if (j >= map[i].Length)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && !visited.Contains((guard, i, j)))
                    {
                        visited.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        j--;
                        guard = Do;
                        break;
                    }

                }
            }

            if (guard is Do)
            {
                while (i++ < map.Length)
                {
                    if (i >= map.Length)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && !visited.Contains((guard, i, j)))
                    {
                        visited.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        i--;
                        guard = Le;
                        break;
                    }

                }
            }

            if (guard is Le)
            {
                while (j-- > 0)
                {
                    if (j < 0)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && !visited.Contains((guard, i, j)))
                    {
                        visited.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        j++;
                        guard = Up;
                        break;
                    }

                }
            }
        };

        var v = visited.Select(z => (z.Item2, z.Item3)).Distinct();
        var vc = v.Count();
        var loops = new ConcurrentBag<(int x, int y)>();
        ushort aa = 0;

        GCSettings.LatencyMode = GCLatencyMode.LowLatency;

        if (GC.TryStartNoGCRegion(1024 * 1024 * 1000))
        {
            Parallel.ForEach(visited.Select(z => (z.Item2, z.Item3)).Distinct(), (coords, _, _) =>
            {
                var (x, y) = coords;
                var mapCopyWithNewObstruction = (string[])map.Clone();

                // Modify only the specific row
                var rowWithNewObstruction = mapCopyWithNewObstruction[x].ToCharArray();
                rowWithNewObstruction[y] = '#';
                mapCopyWithNewObstruction[x] = new string(rowWithNewObstruction);

                var isLoop = FindLoopInPossibleGuardPath(
                    mapCopyWithNewObstruction,
                    initialGuardDirection,
                    initialX,
                    initialY,
                    []
                );

                if (isLoop)
                {
                    loops.Add((x, y));
                }

                aa++;
                Console.Clear();
                Console.WriteLine($"Tried loops {aa} / {vc}");
            });

            GC.Collect();
        }

        Console.WriteLine(loops.Count); // 1784
    }

    private static bool FindLoopInPossibleGuardPath(string[] map, char guard, int i, int j,
        List<(char, int, int)> previouslyVisitedSpots)
    {
        var guardNotOut = true;
        while (guardNotOut)
        {
            if (guard is Up)
            {
                while (i-- >= 0)
                {
                    if (i < 0)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        return true;
                    }

                    if (map[i][j] != '#' && !previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        previouslyVisitedSpots.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        i++; // return to previous position
                        guard = Ri;
                        break;
                    }
                }
            }

            if (guard is Ri)
            {
                while (j++ < map[i].Length)
                {
                    if (j >= map[i].Length)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        return true;
                    }

                    if (map[i][j] != '#' && !previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        previouslyVisitedSpots.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        j--;
                        guard = Do;
                        break;
                    }

                }
            }

            if (guard is Do)
            {
                while (i++ < map.Length)
                {
                    if (i >= map.Length)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        return true;
                    }

                    if (map[i][j] != '#' && !previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        previouslyVisitedSpots.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        i--;
                        guard = Le;
                        break;
                    }

                }
            }

            if (guard is Le)
            {
                while (j-- >= 0)
                {
                    if (j < 0)
                    {
                        guardNotOut = false;
                        break;
                    }

                    if (map[i][j] != '#' && previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        return true;
                    }


                    if (map[i][j] != '#' && !previouslyVisitedSpots.Contains((guard, i, j)))
                    {
                        previouslyVisitedSpots.Add((guard, i, j));
                    }
                    else if (map[i][j] == '#')
                    {
                        j++;
                        guard = Up;
                        break;
                    }
                }
            }
        };

        return false;
    }
}

internal static class Helper
{
    internal static (char guard, int i, int j) GetGuardPosition(string[] map)
    {
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] is '^' or '>' or 'v' or '<')
                {
                    var guard = map[i][j];
                    return (guard, i, j);
                }
            }
        }

        return ('X', -1, -1);
    }
}