using AoC2024.Models;

namespace AoC2024.Challenges;

public class Day6(string? inputPath = null) : BaseDay(inputPath)
{
    public override int Day => 6;

    private record Position(int Row, int Column)
    {
        public Position Move(Direction direction) =>
            new(Row + direction.Row, Column + direction.Col);

        public bool IsInBounds(int totalRows, int totalColumns) =>
            Row >= 0 && Row < totalRows && Column >= 0 && Column < totalColumns;
    }

    private enum CellType
    {
        Empty,
        Obstacle,
        GuardLeft,
        GuardRight,
        GuardUp,
        GuardDown
    }

    private record Guard(Position Position, CellType Facing)
    {
        public Guard TurnRight() =>
            this with { Facing = TurningPattern[Facing] };

        public Guard Move(Direction direction) =>
            this with { Position = Position.Move(direction) };
    }

    private class LabMap(CellType[,] grid)
    {
        public readonly int Rows = grid.GetLength(0);
        public readonly int Columns = grid.GetLength(1);

        public Guard? FindInitialGuard()
        {
            var guardTypes = new[]
            {
                CellType.GuardLeft,
                CellType.GuardRight,
                CellType.GuardUp,
                CellType.GuardDown
            };
            
            for (var i = 0; i < Rows; i++)
            for (var j = 0; j < Columns; j++)
            {
                var cell = grid[i, j];
                if (guardTypes.Contains(cell))
                    return new Guard(new Position(i, j), cell);
            }

            return null;
        }

        public bool IsValidMove(Position position) =>
            !position.IsInBounds(Rows, Columns) ||
            grid[position.Row, position.Column] != CellType.Obstacle;

        public void UpdatePosition(Position oldPos, Position newPos, CellType facing)
        {
            if (oldPos.IsInBounds(Rows, Columns))
            {
                grid[oldPos.Row, oldPos.Column] = CellType.Empty;
            }

            if (newPos.IsInBounds(Rows, Columns))
            {
                grid[newPos.Row, newPos.Column] = facing;
            }
        }
    }

    private class GuardMovementSimulator
    {
        private const int MaxRotations = 4;
        private readonly LabMap _map;
        private readonly HashSet<Position> _visitedPositions = [];
        private Guard? _currentGuard;
        private int _visitCount;

        public GuardMovementSimulator(LabMap map)
        {
            _map = map;
            _currentGuard = map.FindInitialGuard();
            if (_currentGuard == null)
            {
                return;
            }

            TrackPosition(_currentGuard.Position);
        }

        public int SimulateMovement()
        {
            if (_currentGuard == null)
            {
                return 0;
            }

            while (_currentGuard.Position.IsInBounds(_map.Rows, _map.Columns))
            {
                MoveGuard();
            }

            return _visitCount;
        }

        private void MoveGuard()
        {
            var direction = GetNextValidDirection();
            var oldPosition = _currentGuard!.Position;
            _currentGuard = _currentGuard.Move(direction);

            if (!_currentGuard.Position.IsInBounds(_map.Rows, _map.Columns))
            {
                return;
            }
    
            _map.UpdatePosition(oldPosition, _currentGuard.Position, _currentGuard.Facing);
            TrackPosition(_currentGuard.Position);
        }
        
        private Direction GetNextValidDirection()
        {
            var direction = DirectionMappings[_currentGuard!.Facing];
            var nextPosition = _currentGuard.Position.Move(direction);

            if (!nextPosition.IsInBounds(_map.Rows, _map.Columns) || _map.IsValidMove(nextPosition))
            {
                return direction;
            }

            return RotateUntilValidMove();
        }

        private void TrackPosition(Position position)
        {
            if (_visitedPositions.Add(position))
            {
                _visitCount++;
            }
        }

        private Direction RotateUntilValidMove()
        {
            var rotationCount = 0;

            while (rotationCount < MaxRotations)
            {
                _currentGuard = _currentGuard!.TurnRight();
                var direction = DirectionMappings[_currentGuard.Facing];
                var nextPosition = _currentGuard.Position.Move(direction);
                rotationCount++;

                if (!nextPosition.IsInBounds(_map.Rows, _map.Columns) || _map.IsValidMove(nextPosition))
                {
                    return direction;
                }
            }

            throw new InvalidOperationException("Guard is stuck - completed full rotation without finding valid move");
        }

        /// <summary>
        ///
        /// While The Historians begin working around the guard's patrol route, you borrow their fancy
        /// device and step outside the lab. From the safety of a supply closet, you time travel through the
        /// last few months and record the nightly status of the lab's guard post on the walls of the closet.
        /// 
        /// Returning after what seems like only a few seconds to The Historians, they explain that the guard's
        /// patrol area is simply too large for them to safely search the lab without getting caught.
        /// 
        /// Fortunately, they are pretty sure that adding a single new obstruction won't cause a time paradox.
        /// They'd like to place the new obstruction in such a way that the guard will get stuck in a loop,
        /// making the rest of the lab safe to search.
        /// 
        /// To have the lowest chance of creating a time paradox, The Historians would like to know
        /// all of the possible positions for such an obstruction. The new obstruction can't be placed at the guard's
        /// starting position - the guard is there right now and would notice.
        /// 
        /// In the above example, there are only 6 different positions where a new obstruction would cause the guard 
        /// to get stuck in a loop. The diagrams of these six situations use O to mark the new obstruction,
        /// | to show a position where the guard moves up/down, - to show a position where the guard moves left/right,
        /// and + to show a position where the guard moves both up/down and left/right.
        /// 
        /// Option one, put a printing press next to the guard's starting position:
        /// 
        /// ....#.....
        /// ....+---+#
        /// ....|...|.
        /// ..#.|...|.
        /// ....|..#|.
        /// ....|...|.
        /// .#.O^---+.
        /// ........#.
        /// #.........
        /// ......#...
        /// 
        /// Option two, put a stack of failed suit prototypes in the bottom right quadrant of the mapped area:
        /// 
        /// ....#.....
        /// ....+---+#
        /// ....|...|.
        /// ..#.|...|.
        /// ..+-+-+#|.
        /// ..|.|.|.|.
        /// .#+-^-+-+.
        /// ......O.#.
        /// #.........
        /// ......#...
        /// 
        /// Option three, put a crate of chimney-squeeze prototype fabric next to the standing 
        /// desk in the bottom right quadrant:
        /// 
        /// ....#.....
        /// ....+---+#
        /// ....|...|.
        /// ..#.|...|.
        /// ..+-+-+#|.
        /// ..|.|.|.|.
        /// .#+-^-+-+.
        /// .+----+O#.
        /// #+----+...
        /// ......#...
        /// 
        /// Option four, put an alchemical retroencabulator near the bottom left corner:
        /// 
        /// ....#.....
        /// ....+---+#
        /// ....|...|.
        /// ..#.|...|.
        /// ..+-+-+#|.
        /// ..|.|.|.|.
        /// .#+-^-+-+.
        /// ..|...|.#.
        /// #O+---+...
        /// ......#...
        /// 
        /// Option five, put the alchemical retroencabulator a bit to the right instead:
        /// 
        /// ....#.....
        /// ....+---+#
        /// ....|...|.
        /// ..#.|...|.
        /// ..+-+-+#|.
        /// ..|.|.|.|.
        /// .#+-^-+-+.
        /// ....|.|.#.
        /// #..O+-+...
        /// ......#...
        /// 
        /// Option six, put a tank of sovereign glue right next to the tank of universal solvent:
        /// 
        /// ....#.....
        /// ....+---+#
        /// ....|...|.
        /// ..#.|...|.
        /// ..+-+-+#|.
        /// ..|.|.|.|.
        /// .#+-^-+-+.
        /// .+----++#.
        /// #+----++..
        /// ......#O..
        /// 
        /// It doesn't really matter what you choose to use as an obstacle so long as you and The Historians can put 
        /// it into position without the guard noticing. 
        /// The important thing is having enough options that you can find one that minimizes time paradoxes, 
        /// and in this example, there are 6 different positions you could choose.
        /// 
        /// You need to get the guard stuck in a loop by adding a single new obstruction. 
        /// How many different positions could you choose for this obstruction?
        /// </summary>
        /// <returns></returns>
        public int CalculateAllPossibleObstaclePositionsToCreateALoop()
        {
            var sum = 0;
            
            return sum;
        }
    }

    private static readonly Dictionary<char, CellType> InputMap = new()
    {
        ['<'] = CellType.GuardLeft,
        ['>'] = CellType.GuardRight,
        ['^'] = CellType.GuardUp,
        ['v'] = CellType.GuardDown,
        ['.'] = CellType.Empty,
        ['#'] = CellType.Obstacle
    };

    private static readonly Direction[] Directions =
    [
        new(0, -1), // Left
        new(0, 1), // Right
        new(-1, 0), // Up
        new(1, 0), // Down
    ];

    private static readonly Dictionary<CellType, Direction> DirectionMappings = new()
    {
        [CellType.GuardLeft] = Directions[0],
        [CellType.GuardRight] = Directions[1],
        [CellType.GuardUp] = Directions[2],
        [CellType.GuardDown] = Directions[3]
    };

    private static readonly Dictionary<CellType, CellType> TurningPattern = new()
    {
        [CellType.GuardUp] = CellType.GuardRight,
        [CellType.GuardRight] = CellType.GuardDown,
        [CellType.GuardDown] = CellType.GuardLeft,
        [CellType.GuardLeft] = CellType.GuardUp
    };

    public override (int part1, int part2) Solve()
    {
        var map = ParseInput();
        var simulator = new GuardMovementSimulator(map);
        var distinctPositions = simulator.SimulateMovement();

        var possibleLoopPositions = simulator.CalculateAllPossibleObstaclePositionsToCreateALoop();

        return (distinctPositions, possibleLoopPositions);
    }

    private LabMap ParseInput()
    {
        var input = GetInput();
        var grid = new CellType[input.Length, input[0].Length];

        for (var i = 0; i < input.Length; i++)
        for (var j = 0; j < input[i].Length; j++)
        {
            if (!InputMap.TryGetValue(input[i][j], out var cellType))
                throw new FormatException($"Invalid character in input at position ({i}, {j}): {input[i][j]}");

            grid[i, j] = cellType;
        }

        return new LabMap(grid);
    }
}