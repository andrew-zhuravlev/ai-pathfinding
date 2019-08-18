using System.Collections.Generic;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PlatformerRules : IPathFindingRules {

        struct JumpState {
            public int Up;
            public int Horizontal;

            public JumpState(int up, int horizontal) {
                Up = up;
                Horizontal = horizontal;
            }
        }
        
        readonly JumpState[,] _jumpGrid;
        readonly int _jumpHeight, _maxJumpHorizontal;

        public PlatformerRules(int jumpHeight, int maxJumpHorizontal, int gridSizeY, int gridSizeX) {
            _jumpHeight = jumpHeight;
            _maxJumpHorizontal = maxJumpHorizontal;
            
            _jumpGrid = new JumpState[gridSizeY, gridSizeX];
            
            for (var y = 0; y < gridSizeY; ++y)
                for (var x = 0; x < gridSizeX; ++x)
                    _jumpGrid[y, x] = new JumpState();
        }

        public int GetHeuristic(Node node, Node goal) {
            return Mathf.Abs(node.X - goal.X) + Mathf.Abs(node.Y - goal.Y);
        }

        public int GetDistance(Node node, Node neighbour) {
            return 1;
        }

        public IEnumerable<Node> GetNeighbours(Grid grid, Node node) {
            var neighbours = new List<Node>();
            
            JumpState curJump = _jumpGrid[node.Y, node.X];
            // Still going up ?
            if (curJump.Up > 0 && curJump.Up < _jumpHeight) {
                if (grid.IsEmptyNode(node.Y + 1, node.X)) {
                    neighbours.Add(grid.GetNode(node.Y + 1, node.X));
                    _jumpGrid[node.Y + 1, node.X].Up = curJump.Up + 1;
                    
                }
                _jumpGrid[node.Y, node.X] = new JumpState();
                return neighbours;
            }
            
            // Highest point of jump reached.
            if (curJump.Up == _jumpHeight) {
                //Left
                if (grid.IsEmptyNode(node.Y, node.X - 1)) {
                    neighbours.Add(grid.GetNode(node.Y, node.X - 1));
                    _jumpGrid[node.Y, node.X - 1].Horizontal = -1;
                }
                //Right
                if (grid.IsEmptyNode(node.Y, node.X + 1)) {
                    neighbours.Add(grid.GetNode(node.Y, node.X + 1));
                    _jumpGrid[node.Y, node.X + 1].Horizontal = 1;
                }
                _jumpGrid[node.Y, node.X] = new JumpState();

                return neighbours;
            }
            
            if (curJump.Horizontal > 0 && curJump.Horizontal < _maxJumpHorizontal) {
                if (grid.IsEmptyNode(node.Y, node.X + 1)) {
                    neighbours.Add(grid.GetNode(node.Y, node.X + 1));
                    _jumpGrid[node.Y, node.X + 1].Horizontal = curJump.Horizontal + 1;
                }
                _jumpGrid[node.Y, node.X] = new JumpState();
                return neighbours;
            }
            
            if (curJump.Horizontal < 0 && curJump.Horizontal > -_maxJumpHorizontal) {
                if (grid.IsEmptyNode(node.Y, node.X - 1)) {
                    neighbours.Add(grid.GetNode(node.Y, node.X - 1));
                    _jumpGrid[node.Y, node.X - 1].Horizontal = curJump.Horizontal - 1;
                }
                _jumpGrid[node.Y, node.X] = new JumpState();
                return neighbours;
            }
            
            bool isGrounded = !grid.IsEmptyNode(node.Y - 1, node.X);

            if (isGrounded) {
                // Left
                if (grid.IsEmptyNode(node.Y, node.X - 1))
                    neighbours.Add(grid.GetNode(node.Y, node.X - 1));
                // Right
                if (grid.IsEmptyNode(node.Y, node.X + 1))
                    neighbours.Add(grid.GetNode(node.Y, node.X + 1));
                
                // Jump
                if (grid.IsEmptyNode(node.Y + 1, node.X)) {
                    neighbours.Add(grid.GetNode(node.Y + 1, node.X));
                    _jumpGrid[node.Y + 1, node.X].Up = 1;
                }
            }

            // Falling Down
            else {
                neighbours.Add(grid.GetNode(node.Y - 1, node.X));
            }

            _jumpGrid[node.Y, node.X] = new JumpState();
            
            return neighbours;
        }
    }
}