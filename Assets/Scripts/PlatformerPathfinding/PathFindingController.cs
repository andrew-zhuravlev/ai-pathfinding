using System.Collections.Generic;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PathFindingController : MonoBehaviour {
        
        [SerializeField] int _gridSizeX;
        [SerializeField] int _gridSizeY;
        [SerializeField] float _cellSize;
        [SerializeField] LayerMask _collisionLayerMask;
        [SerializeField] bool _drawGrid = true;

        Grid _grid;
        IPathFindingRules _pathFindingRules;

        Node _start, _goal;
        
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
            
            _pathFindingRules = new PlatformerRules();
            _grid = new Grid(gridNodes, _gridSizeX, _gridSizeY);
        }

        public List<Vector2> FindPath(PathFindingAgent agent, Transform goalObject) {
            Node start = WorldPositionToNearestNode(agent.transform.position);
            Node goal = WorldPositionToNearestNode(goalObject.position);

            var path = _grid.Search(start, goal, _pathFindingRules, agent);
            if (path == null)
                return null;

            var pointsPath = new List<Vector2>(path.Count);
            foreach (Node node in path)
                pointsPath.Add(node.WorldPositionCenter);

            return pointsPath;
        }

        void OnDrawGizmos() {
            if (!_drawGrid)
                return;
            
            Vector2 bottomLeftCell = GetBottomLeftCellCenter();

            Vector2 size = Vector2.one * _cellSize;
            for (var y = 0; y < _gridSizeY; ++y) {
                for (var x = 0; x < _gridSizeX; ++x) {
                    Vector2 cellCenter = bottomLeftCell + new Vector2(x * _cellSize, y * _cellSize);

                    Gizmos.color = IsOccupiedCell(cellCenter) ? Color.red : Color.green;

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