namespace AdventOfCode;

public sealed class Day06 : BaseDay
{
    private readonly List<string> _lines;

    public Day06()
    {
        _lines = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>($"{Solve1(ParseMap())}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>($"{GetPossibleLoops(ParseMap())}");
    }

    private int Solve1(Map map)
    {
        var guardPos = map.SelectMany(x => x).First(x => x.Type == MapPosType.Guard).Pos;
        var direction = Direction.Top;

        while (!OutOfBounds(map, guardPos))
        {
            (guardPos, direction) = SimulateMove(map, guardPos, direction);
        }

        return map.SelectMany(x => x).Count(x => x.IsVisited);
    }

    private int GetPossibleLoops(Map mapPrototype)
    {
        var emptyPositions = mapPrototype
            .SelectMany(x => x)
            .Where(x => x.Type == MapPosType.Empty)
            .Select(x => x.Pos)
            .ToList();

        var guardPos = mapPrototype.SelectMany(x => x).First(x => x.Type == MapPosType.Guard).Pos;

        return emptyPositions
            .Select(emptyPos => new Map(mapPrototype) { [emptyPos.X, emptyPos.Y] = { Type = MapPosType.Obstacle } })
            .Select(map => IsLooped(guardPos, map, Direction.Top) ? 1 : 0)
            .Sum();
    }

    private (Pos, Direction) SimulateMove(Map map, Pos guardPos, Direction direction)
    {
        if (IsObstacle(map, guardPos, direction))
        {
            direction = (Direction)(((int)direction + 1) % 4);
            return (guardPos, direction);
        }

        map[guardPos.X, guardPos.Y].Visit();
        guardPos = Move(guardPos, direction);
        return (guardPos, direction);
    }


    private bool IsLooped(Pos guardPos, Map map, Direction direction)
    {
        for (var i = 0;; i++)
        {
            if (i % 1000 == 0 && map.Any(x => x.Any(y => y.VisitedCount > 20)))
                return true;

            if (OutOfBounds(map, guardPos))
                return false;

            (guardPos, direction) = SimulateMove(map, guardPos, direction);
        }
    }

    private Pos Move(Pos pos, Direction dir)
    {
        return dir switch
        {
            Direction.Top => pos with { Y = pos.Y - 1 },
            Direction.Right => pos with { X = pos.X + 1 },
            Direction.Bottom => pos with { Y = pos.Y + 1 },
            Direction.Left => pos with { X = pos.X - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
        };
    }

    private bool IsObstacle(Map map, Pos pos, Direction direction)
    {
        var newPos = Move(pos, direction);
        if (OutOfBounds(map, newPos))
            return false;
        return map[newPos.X, newPos.Y].Type == MapPosType.Obstacle;
    }

    private Map ParseMap()
    {
        var map = new Map();
        for (var y = 0; y < _lines.Count; y++)
        {
            var line = _lines[y];
            map.Add([]);
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                map[y].Add(new MapPos
                {
                    Type = c switch
                    {
                        '.' => MapPosType.Empty,
                        '^' => MapPosType.Guard,
                        _ => MapPosType.Obstacle,
                    },
                    VisitedCount = c == '^' ? 1 : 0,
                    Pos = new Pos(x, y),
                });
            }
        }

        return map;
    }

    private enum Direction
    {
        Top,
        Right,
        Bottom,
        Left,
    }

    public class Map : List<List<MapPos>>
    {
        public Map()
        {
        }

        public Map(Map map)
        {
            foreach (var mapPos in map.SelectMany(x => x))
            {
                if (Count <= mapPos.Pos.Y)
                    Add([]);
                this[mapPos.Pos.Y].Add(new MapPos
                {
                    Pos = mapPos.Pos,
                    Type = mapPos.Type,
                    VisitedCount = mapPos.VisitedCount,
                });
            }
        }

        public MapPos this[int x, int y]
        {
            get => this[y][x];
            set => this[y][x] = value;
        }
    }

    public record Pos(int X, int Y);

    private bool OutOfBounds(Map map, Pos pos) => pos.X < 0 || pos.Y < 0 || pos.X >= map[0].Count || pos.Y >= map.Count;


    public class MapPos
    {
        public Pos Pos { get; set; }

        public int VisitedCount { get; set; }

        public bool IsVisited => VisitedCount > 0;

        public void Visit() => VisitedCount++;

        public MapPosType Type { get; set; }
    }

    public enum MapPosType
    {
        Empty,
        Obstacle,
        Guard,
    }
}