using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PlatformerRules : IPathFindingRules {

        readonly int _jumpHeight, _jumpHorizontal;

        public PlatformerRules(int jumpHeight, int jumpHorizontal) {
            _jumpHeight = jumpHeight;
            _jumpHorizontal = jumpHorizontal;
        }

        public int GetHeuristic(Node node, Node goal) {
            return Mathf.Abs(node.X - goal.X) + Mathf.Abs(node.Y - goal.Y);
        }

        public int GetDistance(Node fromNode, Node toNode) {
            return 1;
        }

        static bool IsGroundedNode(Grid grid, Node node) {
            return !grid.IsEmptyNode(node.Y - 1, node.X);
        }
        
        public IEnumerable<Node> GetNeighbours(Grid grid, Node node) {
            var neighbours = new List<Node>();

            bool isGrounded = IsGroundedNode(grid, node);
            if (isGrounded) {
                // Left
                if (grid.IsEmptyNode(node.Y, node.X - 1))
                    neighbours.Add(grid.GetNode(node.Y, node.X - 1));
                // Right
                if (grid.IsEmptyNode(node.Y, node.X + 1))
                    neighbours.Add(grid.GetNode(node.Y, node.X + 1));
                
                // Jump.
                var isValidJump = true;
                int curY = node.Y;
                int curX = node.X;
                for (int y = 0; y < _jumpHeight; ++y) {
                    ++curY;
                    if (!grid.IsEmptyNode(curY, curX)) {
                        isValidJump = false;
                        break;
                    }
                }

                if (isValidJump) {
                    // Right
                    for (var x = 0; x < _jumpHorizontal; ++x) {
                        ++curX;
                        if (!grid.IsEmptyNode(curY, curX)) {
                            isValidJump = false;
                            break;
                        }
                    }

                    if (isValidJump) {
                        while (!IsGroundedNode(grid, grid.GetNode(curY, curX)))
                            --curY;

                        neighbours.Add(grid.GetNode(curY, curX));
                    }
                }
            }
            // Falling Down. This should only happen if spawned above the ground.
            else {
                neighbours.Add(grid.GetNode(node.Y - 1, node.X));
            }

            return neighbours;
        }
    }
}