namespace AdventOfCode;

// It can be done with regex, but I'm too lazy to write it.
public sealed class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;
        var isInMul = false;
        var isInFirstNumber = false;

        var firstNumberStr = "";
        var secondNumberStr = "";

        var firstNumber = 0;

        for (var i = 3; i < _input.Length; i++)
        {
            var c = _input[i];

            if (_input[i - 3] == 'm' && _input[i - 2] == 'u' && _input[i - 1] == 'l' && _input[i] == '(')
            {
                isInMul = true;
                isInFirstNumber = true;
                continue;
            }

            if (!isInMul)
            {
                firstNumberStr = "";
                secondNumberStr = "";
                continue;
            }

            if (isInFirstNumber)
            {
                if (c == ',')
                {
                    var parsed = int.TryParse(firstNumberStr, out firstNumber);

                    if (!parsed)
                    {
                        isInFirstNumber = false;
                        isInMul = false;
                        firstNumberStr = "";
                        continue;
                    }

                    isInFirstNumber = false;
                    continue;
                }

                if (!char.IsDigit(c))
                {
                    isInFirstNumber = false;
                    isInMul = false;
                    firstNumberStr = "";
                }

                firstNumberStr += c;
            }
            else
            {
                if (c == ')')
                {
                    var parsed = int.TryParse(secondNumberStr, out var secondNumber);

                    if (!parsed)
                    {
                        isInMul = false;
                        secondNumberStr = "";
                        continue;
                    }

                    result += firstNumber * secondNumber;
                    isInMul = false;
                }
                else if (!char.IsDigit(c))
                {
                    isInMul = false;
                    secondNumberStr = "";
                }

                secondNumberStr += c;
            }
        }

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0;
        var isInMul = false;
        var enabled = true;
        var isInFirstNumber = false;

        var firstNumberStr = "";
        var secondNumberStr = "";

        var firstNumber = 0;

        for (var i = 0; i < _input.Length; i++)
        {
            var c = _input[i];

            switch (i)
            {
                case >= 3 when _input[i - 3] == 'd' && _input[i - 2] == 'o' && _input[i - 1] == '(' && _input[i] == ')':
                    enabled = true;
                    continue;
                case >= 6 when _input[i - 6] == 'd' && _input[i - 5] == 'o' && _input[i - 4] == 'n' && _input[i - 3] == '\'' && _input[i - 2] == 't' && _input[i - 1] == '(' &&
                               _input[i] == ')':
                    enabled = false;
                    continue;
                case >= 3 when _input[i - 3] == 'm' && _input[i - 2] == 'u' && _input[i - 1] == 'l' && _input[i] == '(':
                    isInMul = true;
                    isInFirstNumber = true;
                    continue;
            }

            if (!isInMul)
            {
                firstNumberStr = "";
                secondNumberStr = "";
                continue;
            }

            if (isInFirstNumber)
            {
                if (c == ',')
                {
                    var parsed = int.TryParse(firstNumberStr, out firstNumber);

                    if (!parsed)
                    {
                        isInFirstNumber = false;
                        isInMul = false;
                        firstNumberStr = "";
                        continue;
                    }

                    isInFirstNumber = false;
                    continue;
                }

                if (!char.IsDigit(c))
                {
                    isInFirstNumber = false;
                    isInMul = false;
                    firstNumberStr = "";
                }

                firstNumberStr += c;
            }
            else
            {
                if (c == ')')
                {
                    var parsed = int.TryParse(secondNumberStr, out var secondNumber);

                    if (!parsed)
                    {
                        isInMul = false;
                        secondNumberStr = "";
                        continue;
                    }

                    if (enabled)
                        result += firstNumber * secondNumber;

                    isInMul = false;
                }
                else if (!char.IsDigit(c))
                {
                    isInMul = false;
                    secondNumberStr = "";
                }

                secondNumberStr += c;
            }
        }

        return new ValueTask<string>($"{result}");
    }
}