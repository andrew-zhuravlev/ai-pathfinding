using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PathFindingController : MonoBehaviour {
        
        [SerializeField] int _gridSizeX;
        [SerializeField] int _gridSizeY;
        [SerializeField] float _cellSize;
        [SerializeField] LayerMask _collisionLayerMask;

        Grid _grid;
        IPathFindingRules _pathFindingRules;

        Node _start, _goal;
        List<Node> _path;
        
        void Start() {
            var gridNodes = new Node[_gridSizeY, _gridSizeX];

            Vector2 bottomLeftCell = GetBottomLeftCellCenter();
            for (var y = 0; y < _gridSizeY; ++y) {
                for (var x = 0; x < _gridSizeX; ++x) {
                    Vector2 cellCenter = bottomLeftCell + new Vector2(x * _cellSize, y * _cellSize);
                    bool isOccupiedCell = IsOccupiedCell(cellCenter);
                    gridNodes[y, x] = new Node(!isOccupiedCell, cellCenter, x, y);
                }
            }
            
            // TODO:
            _grid = new Grid(gridNodes, _gridSizeX, _gridSizeY);
        }
                
        void Update() {
            _start = WorldPositionToNearestNode(GameObject.Find("Begin").transform.position);
            _goal = WorldPositionToNearestNode(GameObject.Find("End").transform.position);

            _pathFindingRules = new PlatformerRules(3, 3, _gridSizeY, _gridSizeX);
            
            _path = _grid.Search(_start, _goal, _pathFindingRules);
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
        
        Vector2 GetBottomLeftCellCenter() {
            var gridCenter = (Vector2) transform.position;
            return new Vector2(gridCenter.x - (_gridSizeX / 2f - .5f) * _cellSize,
                gridCenter.y - (_gridSizeY / 2f - .5f) * _cellSize);
        }
        
        bool IsOccupiedCell(Vector2 worldPos) {
            return Physics2D.OverlapBox(worldPos, new Vector2(_cellSize, _cellSize) - Vector2.one * 0.05f, 0,
                _collisionLayerMask);
        }
        
        Node WorldPositionToNearestNode(Vector2 worldPos) {
            Vector2 bottomLeftCell = GetBottomLeftCellCenter();
            int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x - bottomLeftCell.x) / _cellSize), 0, _gridSizeX - 1),
                y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - bottomLeftCell.y) / _cellSize), 0, _gridSizeY - 1);

            return _grid.GetNode(y, x);
        }
    }
}