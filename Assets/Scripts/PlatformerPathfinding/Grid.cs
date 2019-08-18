namespace PlatformerPathFinding {
    
    public class Grid {
        readonly Node[,] _grid;
        readonly int _sizeX;
        readonly int _sizeY;

        // Detects if node x and y is correct and it is walkable.
        public Grid(Node[,] grid, int sizeX, int sizeY) {
            _grid = grid;
            _sizeX = sizeX;
            _sizeY = sizeY;
        }

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