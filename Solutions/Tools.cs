namespace Solutions;

public enum Direction
{
    R,
    D,
    L,
    U,
    Tr,
    Tl,
    Br,
    Bl
}

public static class Tools
{
    public static (int i, int j) GetItemPosition<T>(T[][] map, T itemToFind)
    {
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j]?.Equals(itemToFind) ?? false)
                    return (i, j);
            }
        }

        return (-1, -1);
    }

    public static bool IsWithinBounds<T>(T[][] map, params (int i, int j)[] coordinates) => coordinates.All(coordinate => coordinate.i >= 0 && coordinate.i < map.Length && coordinate.j >= 0 && coordinate.j < map[coordinate.i].Length);

    public static int ManhattanDistance((int i1, int j1) firstItem, (int i2, int j2) secondItem) => Math.Abs(firstItem.i1 - secondItem.i2) + Math.Abs(firstItem.j1 - secondItem.j2);

    public static async Task<char[][]> ReadCharMap(string filePath) => (await File.ReadAllLinesAsync(filePath)).Select(x => x.ToCharArray()).ToArray();

    public static IEnumerable<IEnumerable<T>>
        GetPermutations<T>(List<T> list, int length)
    {
        if (length == 1) return list.Select(t => new[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(_ => list, (t1, t2) => t1.Concat([t2]));
    }

    public static IEnumerable<List<string>> CartesianProduct(List<List<string>> lists)
    {
        IEnumerable<List<string>> product = new List<List<string>> { new() };
        return lists.Aggregate(product, (current, list) => current.SelectMany(acc => list.Select(item => acc.Concat(new List<string> { item }).ToList())));
    }


    public static readonly Dictionary<Direction, (int i, int j)> Sides = new()
    {
        { Direction.R, (0, 1) },
        { Direction.D, (1, 0) },
        { Direction.L, (0, -1) },
        { Direction.U, (-1, 0) },
    };

    public static readonly Dictionary<Direction, (int i, int j)> Diagonals = new()
    {
        { Direction.Tr, (-1, 1) },
        { Direction.Tl, (-1, -1) },
        { Direction.Br, (1, 1) },
        { Direction.Bl, (1, -1) }
    };

    public static readonly Dictionary<Direction, (int i, int j)> SidesAndDiagonals = new()
    {
        { Direction.R, (0, 1) },
        { Direction.D, (1, 0) },
        { Direction.L, (0, -1) },
        { Direction.U, (-1, 0) },
        { Direction.Tr, (-1, 1) },
        { Direction.Tl, (-1, -1) },
        { Direction.Br, (1, 1) },
        { Direction.Bl, (1, -1) }
    };
}