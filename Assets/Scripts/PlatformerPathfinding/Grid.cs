using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace PlatformerPathFinding {
    public class Grid : MonoBehaviour {
        [SerializeField] int _gridSizeX;
        [SerializeField] int _gridSizeY;
        [SerializeField] float _cellSize;
        [SerializeField] LayerMask _collisionLayerMask;

        Node[,] _grid;
        Node _start, _goal;
        List<Node> _path;
        
        INeighboursProvider _neighboursProvider;

        void Start() {
            _grid = new Node[_gridSizeY, _gridSizeX];

            Vector2 bottomLeftCell = GetBottomLeftCellCenter();
            for (var y = 0; y < _gridSizeY; ++y) {
                for (var x = 0; x < _gridSizeX; ++x) {
                    Vector2 cellCenter = bottomLeftCell + new Vector2(x * _cellSize, y * _cellSize);
                    bool isOccupiedCell = IsOccupiedCell(cellCenter);
                    _grid[y, x] = new Node(!isOccupiedCell, cellCenter, x, y);
                }
            }
            
            // TODO:
            _neighboursProvider = new FourSideNeighbours();
        }
        
        // Detects if node x and y is correct and it is walkable.
        public bool CheckNode(int y, int x) {
            return (x >= 0 && y >= 0 && x < _gridSizeX && y < _gridSizeY) 
                   && _grid[y, x].IsWalkable;
        }

        public Node GetNode(int y, int x) {
            return _grid[y, x];
        }

        void Update() {
            _start = WorldPositionToNearestNode(GameObject.Find("Begin").transform.position);
            _goal = WorldPositionToNearestNode(GameObject.Find("End").transform.position);

            _path = this.Search(_start, _goal, _neighboursProvider, GetManhattanHeuristics);
        }
        
        static int GetManhattanHeuristics(Node node, Node goal) {
            return Mathf.Abs(node.X - goal.X) + Mathf.Abs(node.Y - goal.Y);
        }

        Node WorldPositionToNearestNode(Vector2 worldPos) {
            Vector2 bottomLeftCell = GetBottomLeftCellCenter();
            int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x - bottomLeftCell.x) / _cellSize), 0, _gridSizeX - 1),
                y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - bottomLeftCell.y) / _cellSize), 0, _gridSizeY - 1);

            return _grid[y, x];
        }

        void OnDrawGizmos() {
            Vector2 bottomLeftCell = GetBottomLeftCellCenter();

            Vector2 size = Vector2.one * _cellSize;
            for (var y = 0; y < _gridSizeY; ++y) {
                for (var x = 0; x < _gridSizeX; ++x) {
                    Vector2 cellCenter = bottomLeftCell + new Vector2(x * _cellSize, y * _cellSize);

                    Gizmos.color = IsOccupiedCell(cellCenter) ? Color.red : Color.green;

                    if (_path != null && _path.Any(node => node.X == x && node.Y == y))
                        Gizmos.color = Color.blue;

                    Gizmos.DrawWireCube(cellCenter, size * 0.95f);
                }
            }
        }

        bool IsOccupiedCell(Vector2 worldPos) {
            return Physics2D.OverlapBox(worldPos, new Vector2(_cellSize, _cellSize) - Vector2.one * 0.05f, 0,
                _collisionLayerMask);
        }

        Vector2 GetBottomLeftCellCenter() {
            var gridCenter = (Vector2) transform.position;
            return new Vector2(gridCenter.x - (_gridSizeX / 2f - .5f) * _cellSize,
                gridCenter.y - (_gridSizeY / 2f - .5f) * _cellSize);
        }
    }
}