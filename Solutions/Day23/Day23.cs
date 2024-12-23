namespace Solutions.Day23;

internal class Day23Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 23);
    public string Name => "LAN Party";
    public bool IsActive => false;
    public string Part1Result => "1043";

    public async Task ExecuteAsync()
    {
        var input = await File.ReadAllLinesAsync("./Day23/input.txt");
        var computersAndLinks = new Dictionary<string, List<string>>();
        foreach (var connection in input.Select(x => x.Split('-')))
        {
            var leftComputer = connection[0];
            var rightComputer = connection[1];

            if (!computersAndLinks.TryGetValue(leftComputer, out var linksLeft))
            {
                computersAndLinks.Add(leftComputer, [rightComputer]);
            }
            else
            {
                linksLeft.Add(rightComputer);
            }

            if (!computersAndLinks.TryGetValue(rightComputer, out var linksRight))
            {
                computersAndLinks.Add(rightComputer, [leftComputer]);
            }
            else
            {
                linksRight.Add(leftComputer);
            }
        }

        var threeGroups = new HashSet<string>();
        foreach (var (computer, links) in computersAndLinks)
        {
            if (!computer.StartsWith('t'))
                continue;

            for (var i = 0; i < links.Count; i++)
            {
                for (var j = i + 1; j < links.Count; j++)
                {
                    if (computersAndLinks[links[j]].Contains(links[i]))
                    {
                        var group = string.Join(',', new List<string> { computer, links[i], links[j] }.OrderBy(x => x));

                        threeGroups.Add(group);
                    }
                }
            }
        }

        Console.WriteLine(threeGroups.Count);
    }
}


internal class Day23Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 23);
    public string Name => "LAN Party";
    public bool IsActive => false;
    public string Part2Result => "ai,bk,dc,dx,fo,gx,hk,kd,os,uz,xn,yk,zs";

    public async Task ExecuteAsync()
    {
        var input = await File.ReadAllLinesAsync("./Day23/input.txt");
        var computersAndLinks = new Dictionary<string, List<string>>();
        foreach (var connection in input.Select(x => x.Split('-')))
        {
            var leftComputer = connection[0];
            var rightComputer = connection[1];

            if (!computersAndLinks.TryGetValue(leftComputer, out var linksLeft))
            {
                computersAndLinks.Add(leftComputer, [rightComputer]);
            }
            else
            {
                linksLeft.Add(rightComputer);
            }

            if (!computersAndLinks.TryGetValue(rightComputer, out var linksRight))
            {
                computersAndLinks.Add(rightComputer, [leftComputer]);
            }
            else
            {
                linksRight.Add(leftComputer);
            }
        }

        BronKerbosch(computersAndLinks, [], computersAndLinks.Keys.ToHashSet(), []);

        var answer = string.Join(',', _largestClique.OrderBy(x => x));

        Console.WriteLine(answer);
    }

    private HashSet<string> _largestClique = [];

    private void BronKerbosch(Dictionary<string, List<string>> graph, HashSet<string> r, HashSet<string> p, HashSet<string> x)
    {
        if (!p.Any() && !x.Any())
        {
            // Check if the current clique is the largest
            if (r.Count > _largestClique.Count)
            {
                _largestClique = [..r];
            }
            return;
        }

        foreach (var vertex in new HashSet<string>(p))
        {
            r.Add(vertex);
            var neighbors = new HashSet<string>(graph[vertex]);
            BronKerbosch(graph, r, [..p.Intersect(neighbors)], [..x.Intersect(neighbors)]);
            r.Remove(vertex);
            p.Remove(vertex);
            x.Add(vertex);
        }
    }
}