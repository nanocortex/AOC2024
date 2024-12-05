namespace AdventOfCode;

public sealed class Day05 : BaseDay
{
    private readonly List<string> _lines;

    public Day05()
    {
        _lines = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var (rules, pageInstructions) = Parse();
        var result = pageInstructions.Sum(pages => GetMiddleNumber1(rules, pages));
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var (rules, pageInstructions) = Parse();
        var result = pageInstructions.Sum(pages => GetMiddleNumber2(rules, pages));
        return new ValueTask<string>($"{result}");
    }


    private int GetMiddleNumber1(List<Rule> rules, List<int> pages)
    {
        foreach (var page in pages)
        {
            var pageRules = rules.Where(x => x.Before == page || x.After == page).ToList();
            if (!AllRuleApplies(page, pageRules, pages))
                return 0;
        }

        return pages[(int)(Math.Round(pages.Count / 2.0, MidpointRounding.AwayFromZero) - 1)];
    }

    private int GetMiddleNumber2(List<Rule> rules, List<int> pages)
    {
        var b = false;
        foreach (var page in pages)
        {
            var pageRules = rules.Where(x => x.Before == page || x.After == page).ToList();
            if (AllRuleApplies(page, pageRules, pages)) continue;
            b = true;
            break;
        }

        if (!b) return 0;

        pages = ReorderPages(rules, pages);
        return pages[(int)(Math.Round(pages.Count / 2.0, MidpointRounding.AwayFromZero) - 1)];
    }

    private List<int> ReorderPages(List<Rule> rules, List<int> pages)
    {
        var reordered = new List<int>(pages);

        foreach (var page in pages)
        {
            foreach (var rule in rules.Where(x => x.Before == page))
            {
                var pageIndex = reordered.IndexOf(page);
                var afterIndex = reordered.IndexOf(rule.After);

                if (afterIndex == -1)
                {
                    continue;
                }

                if (pageIndex <= afterIndex) continue;
                reordered.Remove(page);
                reordered.Insert(afterIndex, page);
            }

            foreach (var rule in rules.Where(x => x.After == page))
            {
                var pageIndex = reordered.IndexOf(page);
                var beforeIndex = reordered.IndexOf(rule.Before);

                if (beforeIndex == -1)
                {
                    continue;
                }

                if (pageIndex >= beforeIndex) continue;

                reordered.Remove(page);
                reordered.Insert(beforeIndex, page);
            }
        }

        return reordered;
    }

    private bool AllRuleApplies(int page, List<Rule> pageRules, List<int> pages)
    {
        var pageIndex = pages.IndexOf(page);

        for (var i = 0; i < pageRules.Count; i++)
        {
            var pageRule = pageRules[i];
            if (page == pageRule.Before)
            {
                if (pages.Contains(pageRule.After) && pageIndex >= pages.IndexOf(pageRule.After))
                    return false;
            }
            else
            {
                if (pages.Contains(pageRule.Before) && pageIndex <= pages.IndexOf(pageRule.Before))
                    return false;
            }
        }

        return true;
    }


    private (List<Rule>, List<List<int>>) Parse()
    {
        var rules = new List<Rule>();

        var i = 0;
        for (var index = 0; index < _lines.Count; index++)
        {
            var line = _lines[index];
            if (string.IsNullOrWhiteSpace(line))
            {
                i = index + 1;
                break;
            }

            var parts = line.Split("|");
            var before = int.Parse(parts[0].Trim());
            var after = int.Parse(parts[1].Trim());

            rules.Add(new Rule(before, after));
        }

        var pageInstructions = new List<List<int>>();
        for (var index = i; index < _lines.Count; index++)
        {
            var parts = _lines[index].Split(",", StringSplitOptions.RemoveEmptyEntries);

            var pages = parts.Select(int.Parse).ToList();
            pageInstructions.Add(pages);
        }

        return (rules, pageInstructions);
    }

    private record Rule(int Before, int After);
}