namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string _input;

    private const int MinDiff = 1;
    private const int MaxDiff = 3;


    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var safeCount = lines.Select(line => IsReportSafe(line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())).Count(safe => safe);
        return new ValueTask<string>($"Solution to {ClassPrefix} {CalculateIndex()}, part 1: {safeCount}");
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        var safeCount = 0;

        foreach (var line in lines)
        {
            var levels = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            if (IsReportSafe(levels))
            {
                safeCount++;
                continue;
            }

            for (var i = 0; i < levels.Count; i++)
            {
                var subLevels = levels.ToList();
                subLevels.RemoveAt(i);
                if (!IsReportSafe(subLevels)) continue;

                safeCount++;
                break;
            }
        }


        return new ValueTask<string>($"Solution to {ClassPrefix} {CalculateIndex()}, part 2: {safeCount}");
    }

    private bool IsReportSafe(List<int> levels)
    {
        var prev = levels[0];
        var prevDiffType = Type.Increase;

        for (var i = 1; i < levels.Count; i++)
        {
            var current = levels[i];
            var type = GetDiffType(current, prev);
            if (i == 1)
                prevDiffType = type;

            var diff = Math.Abs(current - prev);

            if (prevDiffType != type || diff is < MinDiff or > MaxDiff)
                return false;

            prev = current;
        }

        return true;
    }

    private Type GetDiffType(int current, int prev) => current > prev ? Type.Increase : Type.Decrease;

    private enum Type
    {
        Increase,
        Decrease,
    }
}