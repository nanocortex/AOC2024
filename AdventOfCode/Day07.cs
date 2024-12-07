namespace AdventOfCode;

public sealed class Day07 : BaseDay
{
    private readonly List<string> _lines;

    public Day07()
    {
        _lines = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var rules = Parse();
        var result = rules.Sum(rule => Solve1(rule.Numbers, rule.Expected));
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var rules = Parse();
        var result = rules.Sum(rule => Solve2(rule.Numbers, rule.Expected));
        return new ValueTask<string>($"{result}");
    }


    private long Solve1(List<long> numbers, long expected)
    {
        var operators = Enumerable.Repeat("+", numbers.Count - 1).ToList();

        for (var i = 0; i < Math.Pow(2, numbers.Count - 1); i++)
        {
            var binary = Convert.ToString(i, 2).PadLeft(numbers.Count - 1, '0');
            var @operator = new List<string>(operators);
            for (var j = 0; j < binary.Length; j++)
            {
                @operator[j] = binary[j] == '0' ? "+" : "*";
            }

            var result = Calculate(numbers, @operator);
            if (result == expected)
                return result;
        }

        return 0;
    }

    private long Solve2(List<long> numbers, long expected)
    {
        var operators = Enumerable.Repeat("+", numbers.Count - 1).ToList();

        for (var i = 0; i < Math.Pow(3, numbers.Count - 1); i++)
        {
            var ternary = ConvertToBase3(i).PadLeft(numbers.Count - 1, '0');
            var @operator = new List<string>(operators);
            for (var j = 0; j < ternary.Length; j++)
            {
                @operator[j] = ternary[j] == '0' ? "+" : ternary[j] == '1' ? "*" : "||";
            }

            var result = Calculate(numbers, @operator);
            if (result == expected)
                return result;
        }

        return 0;
    }

    private long Calculate(List<long> numbers, List<string> @operators)
    {
        var result = numbers[0];
        for (var i = 1; i < numbers.Count; i++)
        {
            var number = numbers[i];
            var @operator = @operators[i - 1];

            switch (@operator)
            {
                case "+":
                    result += number;
                    break;
                case "*":
                    result *= number;
                    break;
                case "||":
                    result = long.Parse(result.ToString() + number);
                    break;
            }
        }

        return result;
    }

    private List<(long Expected, List<long> Numbers)> Parse()
    {
        var rules = new List<(long, List<long>)>();
        foreach (var line in _lines)
        {
            var parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
            var expected = long.Parse(parts[0]);
            var numbers = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            rules.Add((expected, numbers));
        }

        return rules;
    }

    private static string ConvertToBase3(int number)
    {
        if (number == 0) return "0";

        var result = "";
        while (number > 0)
        {
            var remainder = number % 3;
            result = remainder + result;
            number /= 3;
        }

        return result;
    }
}