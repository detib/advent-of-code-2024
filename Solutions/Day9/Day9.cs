using System.Numerics;

namespace Solutions.Day9;

internal class Day9Part1 : IPart1Challenge
{
    public DateTime Day => new(2024, 12, 9);
    public string Name => "Disk Fragmenter";
    public bool IsActive => false;
    public string Part1Result => "6288599492129";

    public async Task ExecuteAsync()
    {
        var diskMap = await File.ReadAllTextAsync("./Day9/input.txt");

        var itemList = new List<Item>();

        var id = 0;
        for (var index = 0; index < diskMap.Length; index++)
        {
            var diskItem = diskMap[index];

            var itemId = index % 2 == 0 ? (id++).ToString() : ".";
            for (int i = 0; i < int.Parse(diskItem.ToString()); i++)
            {
                var item = new Item
                {
                    Id = itemId,
                    Length = int.Parse(diskItem.ToString()),
                    IsEmptySpace = index % 2 == 0
                };
                itemList.Add(item);
            }
        }

        for (var index = 0; index < itemList.Count; index++)
        {
            var item = itemList[index];
            if (item.Id == ".")
            {
                for (var j = itemList.Count - 1; j >= 0; j--)
                {
                    var rightItem = itemList[j];
                    if (rightItem.Id != "." && index < j)
                    {
                        itemList[index] = rightItem;
                        itemList[j] = item;
                        break;
                    }
                }
            }
        }

        BigInteger checksum = 0;
        for (var index = 0; index < itemList.Count; index++)
        {
            var x = itemList[index];
            checksum += x.Id != "." ? index * int.Parse((string)x.Id) : 0;
        }

        Console.WriteLine(checksum); // 6288599492129
    }
}

internal class Day9Part2 : IPart2Challenge
{
    public DateTime Day => new(2024, 12, 9);
    public string Name => "Disk Fragmenter";
    public bool IsActive => false;
    public string Part2Result => "6321896265143";

    public async Task ExecuteAsync()
    {
        var diskMap = await File.ReadAllTextAsync("./Day9/input.txt");

        var itemList = new List<Item>();

        var id = 0;
        for (var index = 0; index < diskMap.Length; index++)
        {
            var diskItem = diskMap[index];

            var itemId = index % 2 == 0 ? (id++).ToString() : ".";
            for (int i = 0; i < int.Parse(diskItem.ToString()); i++)
            {
                var item = new Item
                {
                    Id = itemId,
                    Length = int.Parse(diskItem.ToString()),
                    IsEmptySpace = index % 2 != 0
                };
                itemList.Add(item);
            }
        }

        for (var i = itemList.Count - 1; i >= 0; i--) 
        {
            if (!itemList[i].IsEmptySpace)
            {
                var rightItem = itemList[i];
                for (var j = 0; j < itemList.Count && j < i; j++)
                {
                    var leftItem = itemList[j];
                    var leftItemLength = itemList[j].Length;
                    if (leftItem.IsEmptySpace && leftItem.Length >= rightItem.Length)
                    {
                        var itemListLengthDifference = leftItem.Length - rightItem.Length;

                        for (int k = 0; k < leftItemLength; k++)
                        {
                            itemList[j + k].Length = itemListLengthDifference;
                            if (k < rightItem.Length)
                            {
                                itemList[j + k] = rightItem;
                                itemList[i - k] = leftItem;
                            }
                        }

                        break;
                    } 
                }
            }
        }


        BigInteger checksum = 0;
        for (var index = 0; index < itemList.Count; index++)
        {
            var x = itemList[index];
            checksum += x.Id != "." ? index * int.Parse(x.Id) : 0;
        }

        Console.WriteLine(checksum); // 6321896265143

    }
}


internal class Item
{
    public required string Id { get; set; }

    public int Length { get; set; }

    public bool IsEmptySpace { get; set; }
}