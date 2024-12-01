namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var line in lines)
        {
            var parts = line.Split("   ", StringSplitOptions.RemoveEmptyEntries);
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

        return new ValueTask<string>($"Solution to {ClassPrefix} {CalculateIndex()}, part 1: {distance}");
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var leftList = new List<int>();
        var rightList = new List<int>();
        foreach (var line in lines)
        {
            var parts = line.Split("   ", StringSplitOptions.RemoveEmptyEntries);
            leftList.Add(int.Parse(parts[0]));
            rightList.Add(int.Parse(parts[1]));
        }


        var score = 0;
        foreach (var (index, left) in leftList.Index())
        {
            var rightListCount = rightList.Count(x => x == left);
            
            score += left * rightListCount;
        }
        
        
        return new ValueTask<string>($"Solution to {ClassPrefix} {CalculateIndex()}, part 2: {score}");
    }
}