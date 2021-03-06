﻿using System.Collections.Generic;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PathFindingGrid : MonoBehaviour {

        [SerializeField] PathFindingAgent _agent;
        
        [SerializeField] int _gridSizeX;
        [SerializeField] int _gridSizeY;
        [SerializeField] float _cellSize;
        [SerializeField] LayerMask _collisionLayerMask;
        [SerializeField] bool _drawGrid = true;

        IPathFindingRules _pathFindingRules;
        AStarSearch _search;

        Node[,] _grid;
        Node _start, _goal;

        public float CellSize => _cellSize;

        public int MaxSize => _gridSizeX * _gridSizeY;

        // TODO:
        Vector2 _position;

        bool IsInsideGrid(int y, int x) {
            return x >= 0 && y >= 0 && x < _gridSizeX && y < _gridSizeY;
        }

        public Node GetNode(int y, int x) {
            return IsInsideGrid(y, x) ? _grid[y, x] : null;
        }


        void Start() {
            _grid = new Node[_gridSizeY, _gridSizeX];

            _position = transform.position;
            
            Vector2 bottomLeftCell = GetBottomLeftCellCenter();
            for (var y = 0; y < _gridSizeY; ++y) {
                for (var x = 0; x < _gridSizeX; ++x) {
                    Vector2 cellCenter = bottomLeftCell + new Vector2(x * _cellSize, y * _cellSize);
                    bool isOccupiedCell = IsOccupiedCell(cellCenter);
                    _grid[y, x] = new Node(!isOccupiedCell, cellCenter, x, y);
                }
            }

            _pathFindingRules = new PlatformerRules(_agent);
            _search = new AStarSearch(this);
        }

        public List<Node> FindPath(PathFindingAgent agent, Transform goalObject) {
            Node start = WorldPositionToNode(agent.transform.position);
            Node goal = WorldPositionToNode(goalObject.position);

            return _search.Search(start, goal, _pathFindingRules, agent);
        }

        Vector2 GetBottomLeftCellViaTransform() {
            var pos = transform.position;
            return new Vector2(pos.x - (_gridSizeX / 2f - .5f) * _cellSize,
                pos.y - (_gridSizeY / 2f - .5f) * _cellSize);    
        }

        void OnDrawGizmos() {
            if (!_drawGrid)
                return;

            Vector2 bottomLeftCell = GetBottomLeftCellViaTransform();

            Vector2 size = Vector2.one * _cellSize;
            for (var y = 0; y < _gridSizeY; ++y) {
                for (var x = 0; x < _gridSizeX; ++x) {
                    Vector2 cellCenter = bottomLeftCell + new Vector2(x * _cellSize, y * _cellSize);

                    Gizmos.color = IsOccupiedCell(cellCenter) ? Color.red : Color.green;

                    Gizmos.DrawWireCube(cellCenter, size * 0.95f);
                }
            }
        }

        bool IsOccupiedCell(Vector2 worldPos) {
            return Physics2D.OverlapBox(worldPos, new Vector2(_cellSize, _cellSize) - Vector2.one * 0.05f, 0,
                _collisionLayerMask);
        }

        public Node WorldPositionToNode(Vector2 worldPos) {
            Vector2 bottomLeftCell = GetBottomLeftCellCenter();
            int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x - bottomLeftCell.x) / _cellSize), 0, _gridSizeX - 1),
                y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - bottomLeftCell.y) / _cellSize), 0, _gridSizeY - 1);

            return GetNode(y, x);
        }

        Vector2 GetBottomLeftCellCenter() {
            return new Vector2(_position.x - (_gridSizeX / 2f - .5f) * _cellSize,
                _position.y - (_gridSizeY / 2f - .5f) * _cellSize);
        }
    }
}