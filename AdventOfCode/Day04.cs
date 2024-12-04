namespace AdventOfCode;

public sealed class Day04 : BaseDay
{
    private readonly string _input;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;
        var map = _input.Split("\n").Select(line => line.ToList()).ToList();

        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] != 'X')
                    continue;

                result += Traverse1(map, x, y);
            }
        }

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0;
        var map = _input.Split("\n").Select(line => line.ToList()).ToList();

        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] != 'A')
                    continue;

                result += Traverse2(map, x, y);
            }
        }

        return new ValueTask<string>($"{result}");
    }

    private int Traverse1(List<List<char>> map, int x, int y)
    {
        var count = 0;
        if (IsInBounds(map, x - 3, y) && map[y][x] == 'X' && map[y][x - 1] == 'M' && map[y][x - 2] == 'A' && map[y][x - 3] == 'S') count++;
        if (IsInBounds(map, x + 3, y) && map[y][x] == 'X' && map[y][x + 1] == 'M' && map[y][x + 2] == 'A' && map[y][x + 3] == 'S') count++;
        if (IsInBounds(map, x, y - 3) && map[y][x] == 'X' && map[y - 1][x] == 'M' && map[y - 2][x] == 'A' && map[y - 3][x] == 'S') count++;
        if (IsInBounds(map, x, y + 3) && map[y][x] == 'X' && map[y + 1][x] == 'M' && map[y + 2][x] == 'A' && map[y + 3][x] == 'S') count++;
        if (IsInBounds(map, x - 3, y - 3) && map[y][x] == 'X' && map[y - 1][x - 1] == 'M' && map[y - 2][x - 2] == 'A' && map[y - 3][x - 3] == 'S') count++;
        if (IsInBounds(map, x + 3, y - 3) && map[y][x] == 'X' && map[y - 1][x + 1] == 'M' && map[y - 2][x + 2] == 'A' && map[y - 3][x + 3] == 'S') count++;
        if (IsInBounds(map, x - 3, y + 3) && map[y][x] == 'X' && map[y + 1][x - 1] == 'M' && map[y + 2][x - 2] == 'A' && map[y + 3][x - 3] == 'S') count++;
        if (IsInBounds(map, x + 3, y + 3) && map[y][x] == 'X' && map[y + 1][x + 1] == 'M' && map[y + 2][x + 2] == 'A' && map[y + 3][x + 3] == 'S') count++;
        return count;
    }

    private int Traverse2(List<List<char>> map, int x, int y)
    {
        if (!IsInBounds(map, x - 1, x - 1) || !IsInBounds(map, x + 1, y + 1) || !IsInBounds(map, x - 1, y + 1) || !IsInBounds(map, x + 1, y - 1))
            return 0;
        if ((map[y - 1][x - 1] != 'M' || map[y + 1][x + 1] != 'S') && (map[y + 1][x + 1] != 'M' || map[y - 1][x - 1] != 'S')) return 0;
        if ((map[y - 1][x + 1] == 'M' && map[y + 1][x - 1] == 'S') || (map[y + 1][x - 1] == 'M' && map[y - 1][x + 1] == 'S')) return 1;
        return 0;
    }


    private bool IsInBounds(List<List<char>> map, int x, int y) => x >= 0 && x < map[0].Count && y >= 0 && y < map.Count;
}