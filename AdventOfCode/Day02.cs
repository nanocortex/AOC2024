namespace AdventOfCode;

public sealed class Day02 : BaseDay
{
    private readonly List<string> _lines;

    public Day02()
    {
        _lines = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var safeCount = _lines.Select(line => IsReportSafe(line.Split(' ').Select(int.Parse).ToList())).Count(safe => safe);
        return new ValueTask<string>($"{safeCount}");
    }

    public override ValueTask<string> Solve_2()
    {
        var safeCount = 0;

        foreach (var line in _lines)
        {
            var levels = line.Split(' ').Select(int.Parse).ToList();

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

        return new ValueTask<string>($"{safeCount}");
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

            if (prevDiffType != type || diff is < 1 or > 3)
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