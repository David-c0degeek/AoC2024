using AoC2024.Models;

namespace AoC2024.Challenges;

public class Day6(string? inputPath = null) : BaseDay(inputPath)
{
    public override int Day => 6;

    public record Position(int Row, int Column)
    {
        public Position Move(Direction direction) =>
            new(Row + direction.Row, Column + direction.Col);

        public bool IsInBounds(int totalRows, int totalColumns) =>
            Row >= 0 && Row < totalRows && Column >= 0 && Column < totalColumns;
    }

    public enum CellType
    {
        Empty,
        Obstacle,
        GuardLeft,
        GuardRight,
        GuardUp,
        GuardDown
    }

    public record Guard(Position Position, CellType Facing)
    {
        public Guard TurnRight() =>
            this with { Facing = TurningPattern[Facing] };

        public Guard Move(Direction direction) =>
            this with { Position = Position.Move(direction) };
    }

    public class LabMap
    {
        public readonly int Rows;
        public readonly int Columns;

        public LabMap(CellType[,] grid)
        {
            Grid = grid;

            Rows = Grid.GetLength(0);
            Columns = Grid.GetLength(1);
        }

        public CellType[,] Grid { get; set; }

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
                var cell = Grid[i, j];
                if (guardTypes.Contains(cell))
                    return new Guard(new Position(i, j), cell);
            }

            return null;
        }

        public bool IsValidMove(Position position) =>
            !position.IsInBounds(Rows, Columns) ||
            Grid[position.Row, position.Column] != CellType.Obstacle;

        public void UpdatePosition(Position oldPos, Position newPos, CellType facing)
        {
            if (oldPos.IsInBounds(Rows, Columns))
            {
                Grid[oldPos.Row, oldPos.Column] = CellType.Empty;
            }

            if (newPos.IsInBounds(Rows, Columns))
            {
                Grid[newPos.Row, newPos.Column] = facing;
            }
        }
    }

    private class GuardMovementSimulator
    {
        private const int MaxRotations = 4;
        private readonly LabMap _map;
        public readonly HashSet<Position> VisitedPositions = [];
        private readonly Dictionary<(Position Position, CellType Facing), int> _stateVisits = [];
        private Guard? _currentGuard;
        private int _visitCount;

        public GuardMovementSimulator(LabMap map)
        {
            _map = new LabMap((CellType[,])map.Grid.Clone());
            _currentGuard = map.FindInitialGuard();
            if (_currentGuard == null)
            {
                return;
            }

            TrackPosition(_currentGuard.Position);
        }

        /// <summary>
        /// Returns -1 if guard is stuck in a loop
        /// </summary>
        /// <returns></returns>
        public int SimulateMovement()
        {
            if (_currentGuard == null)
            {
                return 0;
            }

            // Maximum possible unique states = grid size * 4 directions * 2 visits
            // This accounts for visiting each position in each direction twice
            var maxPossibleStates = _map.Rows * _map.Columns * 4 * 2;
            var moveCount = 0;

            while (_currentGuard.Position.IsInBounds(_map.Rows, _map.Columns))
            {
                if (moveCount++ > maxPossibleStates)
                {
                    return -1; // We must be in a loop if we've exceeded max possible states
                }

                if (!MoveGuard())
                {
                    return -1;
                }
            }

            return _visitCount;
        }

        private bool MoveGuard()
        {
            var newDirectionAttempt = GetNextValidDirection();
            if (!newDirectionAttempt.Success)
            {
                return false;
            }

            var oldPosition = _currentGuard!.Position;
            _currentGuard = _currentGuard.Move(newDirectionAttempt.NewDirection!);

            if (!_currentGuard.Position.IsInBounds(_map.Rows, _map.Columns))
            {
                return true;
            }

            // Track the state (position + facing direction)
            var state = (_currentGuard.Position, _currentGuard.Facing);
            _stateVisits[state] = _stateVisits.GetValueOrDefault(state) + 1;

            // If we've seen this exact state (position + facing) more than twice,
            // we're definitely in a loop
            if (_stateVisits[state] > 2)
            {
                return false;
            }

            _map.UpdatePosition(oldPosition, _currentGuard.Position, _currentGuard.Facing);
            TrackPosition(_currentGuard.Position);

            return true;
        }

        private (bool Success, Direction? NewDirection) GetNextValidDirection()
        {
            var direction = DirectionMappings[_currentGuard!.Facing];
            var nextPosition = _currentGuard.Position.Move(direction);

            if (!nextPosition.IsInBounds(_map.Rows, _map.Columns) || _map.IsValidMove(nextPosition))
            {
                return (true, direction);
            }

            return RotateUntilValidMove();
        }

        private void TrackPosition(Position position)
        {
            if (VisitedPositions.Add(position))
            {
                _visitCount++;
            }
        }

        private bool HasVisitedPosition(Position position) => VisitedPositions.Contains(position);

        private (bool Success, Direction? NewDirection) RotateUntilValidMove()
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
                    return (true, direction);
                }
            }

            return (false, null);
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

        var allPossibleObstaclePositionsToCreateALoop =
            CalculateAllPossibleObstaclePositionsToCreateALoop(simulator.VisitedPositions, map);

        return (distinctPositions, allPossibleObstaclePositionsToCreateALoop);
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
    /// <param name="possibleObstaclePositions"></param>
    /// <param name="initialMap"></param>
    /// <returns></returns>
    private static int CalculateAllPossibleObstaclePositionsToCreateALoop(HashSet<Position> possibleObstaclePositions,
        LabMap initialMap)
    {
        var sum = 0;

        possibleObstaclePositions.Remove(initialMap.FindInitialGuard()!.Position);

        foreach (var position in possibleObstaclePositions)
        {
            var newMap = new LabMap((CellType[,])initialMap.Grid.Clone());
            newMap.UpdatePosition(position, position, CellType.Obstacle);
            var simulator = new GuardMovementSimulator(newMap);

            if (simulator.SimulateMovement() == -1)
            {
                sum++;
            }
        }

        return sum;
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