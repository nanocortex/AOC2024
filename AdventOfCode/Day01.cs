namespace AdventOfCode;

public sealed class Day01 : BaseDay
{
    private readonly List<string> _lines;

    public Day01()
    {
        _lines = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var line in _lines)
        {
            var parts = line.Split("   ");
            leftList.Add(int.Parse(parts[0]));
            rightList.Add(int.Parse(parts[1]));
        }

        leftList = leftList.Order().ToList();
        rightList = rightList.Order().ToList();

        var distance = 0;
        foreach (var (index, left) in leftList.Index())
        {
            var d = Math.Abs(left - rightList[index]);
            distance += d;
        }

        return new ValueTask<string>($"{distance}");
    }

    public override ValueTask<string> Solve_2()
    {
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var line in _lines)
        {
            var parts = line.Split("   ");
            leftList.Add(int.Parse(parts[0]));
            rightList.Add(int.Parse(parts[1]));
        }

        var score = 0;
        foreach (var left in leftList)
        {
            var rightListCount = rightList.Count(x => x == left);
            score += left * rightListCount;
        }

        return new ValueTask<string>($"{score}");
    }
}