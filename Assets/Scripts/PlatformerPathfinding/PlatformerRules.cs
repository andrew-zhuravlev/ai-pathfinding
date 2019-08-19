using System.Collections.Generic;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PlatformerRules : IPathFindingRules {
        public int GetHeuristic(Node node, Node goal, PathFindingAgent agent) {
            return Mathf.Abs(node.X - goal.X) + Mathf.Abs(node.Y - goal.Y);
        }

        public int GetCost(Node fromNode, Node toNode, PathFindingAgent agent) {
            int deltaY = toNode.Y - fromNode.Y,
                deltaX = toNode.X - fromNode.X;
            // Step to the side.
            if (deltaY == 0 && (deltaX == -1 || deltaX == 1))
                return 1;
            // Performed jump.
            return agent.JumpHeight + Mathf.Abs(deltaX) + agent.JumpHeight - deltaY;
        }

        static bool IsGroundedNode(Grid grid, Node node) {
            return !grid.IsEmptyNode(node.Y - 1, node.X);
        }

        static Node GetFallOnGroundNode(Grid grid, int y, int x) {
            while (!IsGroundedNode(grid, grid.GetNode(y, x)))
                --y;
            return grid.GetNode(y, x);
        }
        
        public IEnumerable<Node> GetNeighbours(Grid grid, Node node, PathFindingAgent agent) {
            var neighbours = new List<Node>();

            var jumpHeight = agent.JumpHeight;
            var jumpHorizontal = agent.JumpHorizontal;

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
                for (int y = 1; y <= jumpHeight; ++y) {
                    if (!grid.IsEmptyNode(node.Y + y, node.X)) {
                        isValidJump = false;
                        break;
                    }
                }

                if (isValidJump) {
                    // Right
                    for (var x = 1; x <= jumpHorizontal; ++x) {
                        if (!grid.IsEmptyNode(node.Y + jumpHeight, node.X + x))
                            break;
                        if (x > 1)
                            neighbours.Add(GetFallOnGroundNode(grid, node.Y + jumpHeight, node.X + x));
                    }
                    // Left
                    for (var x = -1; x >= -jumpHorizontal; --x) {
                        if (!grid.IsEmptyNode(node.Y + jumpHeight, node.X + x))
                            break;
                        if (x < -1)
                            neighbours.Add(GetFallOnGroundNode(grid, node.Y + jumpHeight, node.X + x));
                    }
                }
            }
            // Falling Down. This should only happen if spawned above the ground.
            else
                neighbours.Add(grid.GetNode(node.Y - 1, node.X));

            return neighbours;
        }
    }
}