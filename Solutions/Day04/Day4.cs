namespace Solutions.Day4;

internal class Day4Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 4);
    public string Name => "Ceres Search";
    public bool IsActive => false;
    public string Part1Result => "2644";

    public async Task ExecuteAsync()
    {
        var wordSearch = await File.ReadAllLinesAsync("./Day4/input.txt");

        int result = 0;
        for (var i = 0; i < wordSearch.Length; i++)
        {
            var word = wordSearch[i];

            for (var j = 0; j < word.Length; j++)
            {
                // flliqt
                try
                {
                    var wordRight = wordSearch[i][j..(j + 4)];
                    if (wordRight is "XMAS" or "SAMX") result++;
                }
                catch
                {
                }

                try
                {
                    var wordBottom = wordSearch[i..(i + 4)].ToList();
                    var t = string.Join("", wordBottom.Select(x => x[j]));
                    if (t is "XMAS" or "SAMX") result++;
                }
                catch
                {
                }

                try
                {
                    var wordDiagonalDown = "";
                    for (var k = 0; k < 4; k++)
                    {
                        wordDiagonalDown += wordSearch[i + k][j + k];
                    }
                    if (wordDiagonalDown is "XMAS" or "SAMX") result++;
                }
                catch
                {

                }

                try
                {
                    var wordDiagonalUp = "";
                    for (var k = 0; k < 4; k++)
                    {
                        wordDiagonalUp += wordSearch[i - k][j + k];
                    }
                    if (wordDiagonalUp is "XMAS" or "SAMX") result++;
                }
                catch
                {

                }
            }
        }

        Console.WriteLine(result); // 2644
    }
}

internal class Day4Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 4);
    public string Name => "Ceres Search";
    public bool IsActive => false;
    public string Part2Result => "1952";

    public async Task ExecuteAsync()
    {
        var wordSearch = await File.ReadAllLinesAsync("./Day4/input.txt");

        int result = 0;
        for (var i = 0; i < wordSearch.Length; i++)
        {
            var word = wordSearch[i];

            for (var j = 0; j < word.Length; j++)
            {
                if (word[j] == 'A')
                {
                    try
                    {
                        var topLeft = wordSearch[i - 1][j - 1];
                        var topRight = wordSearch[i - 1][j + 1];
                        var bottomLeft = wordSearch[i + 1][j - 1];
                        var bottomRight = wordSearch[i + 1][j + 1];

                        var firstDiagonal = $"{topLeft}A{bottomRight}";
                        var secondDiagonal = $"{bottomLeft}A{topRight}";

                        if (firstDiagonal is "MAS" or "SAM" && secondDiagonal is "MAS" or "SAM")
                            result++;
                    } catch {} // for out of bounds
                }
            }
        }

        Console.WriteLine(result); // 1952
    }
}