namespace Solutions;

internal interface IChallenge
{
    DateTime Day { get; }
    bool IsActive { get; }
    string? Name => null;
    Task ExecuteAsync();
}

internal interface IPart1Challenge : IChallenge
{
    string Part1Result { get; }
}

internal interface IPart2Challenge : IChallenge
{
    string Part2Result { get; }
}