
namespace PlatformerPathFinding {
    public class Grid {
        readonly Node[,] _grid;
        readonly int _sizeX;
        readonly int _sizeY;

        public float CellSize { get; }

        // Detects if node x and y exists and it's walkable.
        public Grid(Node[,] grid, int sizeX, int sizeY, float cellSize) {
            _grid = grid;
            _sizeX = sizeX;
            _sizeY = sizeY;
            CellSize = cellSize;
        }
        
        public int MaxSize => _sizeX * _sizeY;

        bool CheckNode(int y, int x) {
            return x >= 0 && y >= 0 && x < _sizeX && y < _sizeY;
        }

        public bool IsEmptyNode(int y, int x) {
            return CheckNode(y, x) && GetNode(y, x).IsEmpty;
        }

        public Node GetNode(int y, int x) {
            return _grid[y, x];
        }
    }
}