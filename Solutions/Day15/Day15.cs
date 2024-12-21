using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using static Solutions.Day15.Helper;

namespace Solutions.Day15;

internal class Day15Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 15);
    public bool IsActive => false;
    public string Name => "Warehouse Woes";
    public string Part1Result => "1505963";

    public async Task ExecuteAsync()
    {
        var input = await File.ReadAllTextAsync("./Day15/input.txt");

        var map = input.Split("\r\n\r\n")[0].Split("\r\n").Select(x => x.ToCharArray()).ToArray();
        var moves = input.Split("\r\n\r\n")[1].Replace("\r\n", string.Empty);

        var allFrames = new List<char[][]>();
        foreach (var nextMove in moves)
        {
            allFrames.Add(map.Select(x => x.ToArray()).ToArray());
            var robotPosition = GetItemPosition(map, '@');
            var direction = Directions[nextMove];

            //var outputBuffer = new StringBuilder();

            //foreach (var row in map)
            //{
            //    foreach (var cell in row)
            //    {
            //        outputBuffer.Append(cell);
            //    }
            //    outputBuffer.AppendLine();
            //}

            //Console.Clear();
            //Console.Write(outputBuffer.ToString());
            //Thread.Sleep(20);


            var nextTile = map[robotPosition.i + direction.i][robotPosition.j + direction.j];

            if (nextTile == '.')
            {
                map[robotPosition.i + direction.i][robotPosition.j + direction.j] = '@';
                map[robotPosition.i][robotPosition.j] = '.';
                continue;
            }

            if (nextTile == 'O')
            {
                var nextPos = (robotPosition.i + direction.i, robotPosition.j + direction.j);
                var firstO = (nextPos.Item1, nextPos.Item2);
                var iterations = 1;
                while (map[nextPos.Item1][nextPos.Item2] == 'O')
                {
                    iterations++;
                    nextPos = (robotPosition.i + direction.i * iterations, robotPosition.j + direction.j * iterations);
                }

                if (map[nextPos.Item1][nextPos.Item2] == '#')
                    continue;

                if (map[nextPos.Item1][nextPos.Item2] == '.')
                {
                    map[robotPosition.i][robotPosition.j] = '.';
                    map[firstO.Item1][firstO.Item2] = '@';
                    map[nextPos.Item1][nextPos.Item2] = 'O';
                }
            }
        }

        allFrames.Add(map);

        var boxCoordinates = new List<(int i, int j)>();
        for (var i = 0; i < map.Length; i++)
        {
            var x = map[i];
            for (var j = 0; j < x.Length; j++)
            {
                var y = x[j];
                if (y == 'O')
                {
                    boxCoordinates.Add((i, j));
                }
            }
        }

        var answer = boxCoordinates.Sum(x => x.i * 100 + x.j);

        Console.WriteLine(answer);

        if (false)
            CreateVideo(RemoveDuplicateFrames(allFrames), "frames1", "video2");
    }
}

internal class Day15Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 15);
    public bool IsActive => false;
    public string Name => "Warehouse Woes";
    public string Part2Result => "1543141";

    public async Task ExecuteAsync()
    {
        var input = await File.ReadAllTextAsync("./Day15/input.txt");

        var oldMap = input.Split("\r\n\r\n")[0].Split("\r\n").Select(x => x.ToCharArray()).ToArray();
        var moves = input.Split("\r\n\r\n")[1].Replace("\r\n", string.Empty);

        var listMap = new List<List<char>>();

        foreach (var x in oldMap)
        {
            var newlistRow = new List<char>();
            foreach (var y in x)
            {
                if (y == 'O')
                {
                    newlistRow.Add('[');
                    newlistRow.Add(']');
                }
                else if (y == '@')
                {
                    newlistRow.Add('@');
                    newlistRow.Add('.');
                }
                else
                {
                    newlistRow.Add(y);
                    newlistRow.Add(y);
                }
            }

            listMap.Add(newlistRow);
        }

        var map = listMap.Select(x => x.ToArray()).ToArray();
        var allFrames = new List<char[][]>();
        foreach (var nextMove in moves)
        {
            allFrames.Add(map.Select(x => x.ToArray()).ToArray());

            var robotPosition = GetItemPosition(map, '@');

            var direction = Directions[nextMove];

            var nextTile = map[robotPosition.i + direction.i][robotPosition.j + direction.j];

            if (nextTile == '.')
            {
                map[robotPosition.i + direction.i][robotPosition.j + direction.j] = '@';
                map[robotPosition.i][robotPosition.j] = '.';
                continue;
            }

            if (nextTile is '[' or ']')
            {
                if (nextMove is '<' or '>')
                {
                    var nextPos = (robotPosition.i + direction.i, robotPosition.j + direction.j);
                    var firstBoxPart = (nextPos.Item1, nextPos.Item2);
                    var iterations = 1;
                    var stack = new Stack<(int i, int j, char part)>();
                    while (map[nextPos.Item1][nextPos.Item2] is '[' or ']')
                    {
                        stack.Push((nextPos.Item1, nextPos.Item2, map[nextPos.Item1][nextPos.Item2]));
                        iterations++;
                        nextPos = (robotPosition.i + direction.i * iterations, robotPosition.j + direction.j * iterations);
                    }

                    if (map[nextPos.Item1][nextPos.Item2] == '#')
                        continue;

                    if (map[nextPos.Item1][nextPos.Item2] == '.')
                    {
                        map[firstBoxPart.Item1][firstBoxPart.Item2] = '@';
                        map[robotPosition.i][robotPosition.j] = '.';
                        while (stack.Count > 0)
                        {
                            var (i1, j, part) = stack.Pop();
                            map[i1 + direction.i][j + direction.j] = part;
                        }
                    }
                }

                if (nextMove is 'v' or '^')
                {
                    (int i, int j) nextPos = (robotPosition.i + direction.i, robotPosition.j + direction.j);
                    var firstItemLocation = (robotPosition.i + direction.i, robotPosition.j + direction.j);

                    if (map[firstItemLocation.Item1][firstItemLocation.Item2] == '.')
                    {
                        map[firstItemLocation.Item1][firstItemLocation.Item2] = '@';
                        map[robotPosition.i][robotPosition.j] = '.';
                        continue;
                    }

                    var itemsToCheck = new Stack<(int i, int j)>();
                    itemsToCheck.Push(nextPos);
                    var seen = new Stack<(int i, int j, char part)>();
                    while (itemsToCheck.Count > 0)
                    {
                        var nextItem = itemsToCheck.Pop();
                        var nextPosItem = map[nextItem.i][nextItem.j];
                        if (seen.Contains((nextItem.i, nextItem.j, nextPosItem)))
                            continue;


                        if (nextPosItem == '[')
                        {
                            seen.Push((nextItem.i, nextItem.j, nextPosItem));
                            itemsToCheck.Push((nextItem.i, nextItem.j + 1));
                        }
                        else if (nextPosItem == ']')
                        {
                            seen.Push((nextItem.i, nextItem.j, nextPosItem));
                            itemsToCheck.Push((nextItem.i, nextItem.j - 1));
                        }
                        else if (nextPosItem == '#')
                        {
                            itemsToCheck.Clear();
                            seen.Clear();
                            continue;
                        }
                        else if (nextPosItem == '.')
                        {
                            continue;
                        }

                        itemsToCheck.Push((nextItem.i + direction.i, nextItem.j + direction.j));
                    }

                    var seenCount = seen.Count;

                    var itemsToMove = seen.ToList();
                    if (nextMove == '^')
                        itemsToMove = itemsToMove.OrderBy(x => x.i).ToList();
                    else
                        itemsToMove = itemsToMove.OrderByDescending(x => x.i).ToList();
                    foreach (var nextItem in itemsToMove)
                    {
                        map[nextItem.i][nextItem.j] = '.';
                        map[nextItem.i + direction.i][nextItem.j] = nextItem.part;
                    }

                    if (seenCount > 0)
                    {
                        map[firstItemLocation.Item1][firstItemLocation.Item2] = '@';
                        map[robotPosition.i][robotPosition.j] = '.';
                    }
                }
            }
        }

        var boxCoordinates = new List<(int i, int j)>();
        for (var i = 0; i < map.Length; i++)
        {
            var x = map[i];
            for (var j = 0; j < x.Length; j++)
            {
                var y = x[j];
                if (y == '[')
                {
                    boxCoordinates.Add((i, j));
                }
            }
        }

        var answer = boxCoordinates.Sum(x => x.i * 100 + x.j);

        Console.WriteLine(answer);

        allFrames.Add(map);
        // 1.16 gb frames, 7mb video
        if (false)
            CreateVideo(RemoveDuplicateFrames(allFrames));
    }
}



internal static class Helper
{
    internal static readonly Dictionary<char, (int i, int j)> Directions = new()
    {
        { '>', (0, 1) },
        { 'v', (1, 0) },
        { '<', (0, -1) },
        { '^', (-1, 0) }
    };

    internal static List<char[][]> RemoveDuplicateFrames(List<char[][]> frames)
    {
        var distinctFrames = new List<char[][]>();

        for (var i = 0; i < frames.Count; i++)
        {
            if (i == 0 || !AreFramesEqual(frames[i], frames[i - 1]))
            {
                distinctFrames.Add(frames[i]);
            }
        }

        return distinctFrames;
    }

    private static bool AreFramesEqual(char[][] frame1, char[][] frame2)
    {
        if (frame1.Length != frame2.Length)
            return false;

        for (int i = 0; i < frame1.Length; i++)
        {
            if (!frame1[i].SequenceEqual(frame2[i]))
                return false;
        }

        return true;
    }

    internal static void CreateVideo(List<char[][]> frames, string? folderName = "frames", string? videoName = "video")
    {
        var frameDir = $@"{AppContext.BaseDirectory}..\..\..\Day15\{folderName}\";

        if (!Directory.Exists(frameDir))
            Directory.CreateDirectory(frameDir);

        if (Directory.GetFiles(frameDir).Length == 0)
        {
            var totalFrames = frames.Count;
            var lockObject = new object();
            var processedFrames = 0;

            Parallel.ForEach(frames.Select((frame, index) => new { Frame = frame, Index = index }),
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                frameInfo =>
                {
                    // Create an image for each frame
                    using var bitmap = RenderFrameToImage(frameInfo.Frame);
                    var filePath = Path.Combine(frameDir, $"frame_{frameInfo.Index:D8}.png");
                    bitmap.Save(filePath, ImageFormat.Png);

                    lock (lockObject)
                        processedFrames++;
                    
                    Console.WriteLine($"Processed frame {processedFrames} of {totalFrames}");
                    Console.Clear();
                });
        }

        Console.Clear();

        var outputVideoDir = $@"{AppContext.BaseDirectory}..\..\..\Day15\{videoName}";
        CreateVideoFromImages(frameDir, outputVideoDir);
        CreateVideoFromImages(frameDir, outputVideoDir, frameRate: 60);
        CreateVideoFromImages(frameDir, outputVideoDir, frameRate: 120);

        Console.WriteLine("Video created");
    }

    private static void CreateVideoFromImages(string imagesFolderPath, string outputVideoPath, int frameRate = 30)
    {
        outputVideoPath += $"-{frameRate}.mp4";
        if (!Directory.Exists(imagesFolderPath))
        {
            throw new DirectoryNotFoundException($"The folder '{imagesFolderPath}' does not exist.");
        }

        if (File.Exists(outputVideoPath))
        {
            File.Delete(outputVideoPath);
        }

        const string ffmpegPath = @"C:\Users\pc\Desktop\ffmpeg\ffmpeg-2024-12-11-git-a518b5540d-full_build\bin\ffmpeg.exe";

        if (!File.Exists(ffmpegPath))
            throw new Exception("FFMPEG executable not found!");

        string arguments = $"-y -framerate {frameRate} -i \"{Path.Combine(imagesFolderPath, "frame_%08d.png")}\" " +
                           $"-c:v libx264 -pix_fmt yuv420p \"{outputVideoPath}\"";

        var processInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process())
        {
            process.StartInfo = processInfo;
            process.OutputDataReceived += (_, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (_, e) => Console.WriteLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        Console.WriteLine("Video creation completed!");
    }

    private static Bitmap RenderFrameToImage(char[][] frame)
    {
        var cellSize = 20; // Size of each cell in pixels
        var width = frame[0].Length * cellSize;
        var height = frame.Length * cellSize;

        var bitmap = new Bitmap(width, height);
        using var g = Graphics.FromImage(bitmap);
        g.Clear(Color.Black); // Background color

        for (var y = 0; y < frame.Length; y++)
        {
            for (var x = 0; x < frame[y].Length; x++)
            {
                var cell = frame[y][x];
                var brush = cell switch
                {
                    '#' => Brushes.DarkSlateGray,
                    '.' => Brushes.LightGray,
                    '[' => Brushes.SandyBrown,
                    ']' => Brushes.SandyBrown,
                    'O' => Brushes.SandyBrown,
                    '@' => Brushes.Firebrick,
                    _ => Brushes.Black
                };

                if (cell == '[')
                {
                    g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                    g.DrawRectangle(Pens.SlateGray, x * cellSize, y * cellSize, cellSize, cellSize);
                    // Draw border on the right side
                    g.DrawLine(Pens.SandyBrown, (x + 1) * cellSize, y * cellSize, (x + 1) * cellSize, (y + 1) * cellSize);
                }
                else if (cell == ']')
                {
                    g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                    g.DrawRectangle(Pens.SlateGray, x * cellSize, y * cellSize, cellSize, cellSize);
                    // Draw border on the left side
                    g.DrawLine(Pens.SandyBrown, x * cellSize, y * cellSize, x * cellSize, (y + 1) * cellSize);
                }
                else if (cell == 'O')
                {
                    g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                    g.DrawRectangle(Pens.SlateGray, x * cellSize, y * cellSize, cellSize, cellSize);
                }
                else
                {
                    g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                }
            }
        }

        return bitmap;
    }
}