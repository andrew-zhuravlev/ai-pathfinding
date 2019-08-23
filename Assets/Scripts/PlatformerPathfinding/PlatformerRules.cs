using System;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PlatformerRules : IPathFindingRules {
        static readonly Func<Node, bool> IsGround = n => n == null || !n.IsEmpty;
        static readonly Func<Node, bool> IsAir = n => n != null && n.IsEmpty;

        public PlatformerRules(PathFindingAgent agent) {
            int neighboursMaxSize = 2 + 2 * (agent.JumpStrength - 1);
            _neighbours = new Node[neighboursMaxSize];
        }

        readonly Node[] _neighbours;

        public int GetHeuristic(Node node, Node goal, PathFindingAgent agent) {
            return Mathf.Abs(node.X - goal.X) + Mathf.Abs(node.Y - goal.Y);
        }

        public int GetCost(Node fromNode, Node toNode, PathFindingAgent agent) {
            int deltaY = toNode.Y - fromNode.Y,
                deltaX = toNode.X - fromNode.X;
            // Step to the side.
            if (Node.IsWalkTransition(fromNode, toNode))
                return 1;
            // Performed jump.
            return agent.JumpStrength + Mathf.Abs(deltaX) + agent.JumpStrength - deltaY;
        }

        public Node[] GetNeighbours(PathFindingGrid grid, Node node, PathFindingAgent agent, out int count) {
            
            count = 0;

            bool isGrounded = AnyNode(grid, node.Y - 1, node.X, 1, agent.Width, IsGround);
            if (isGrounded) {

                Node neighbour;
                
                if (AllNodes(grid, node.Y, node.X - 1, agent.Height, 1, IsAir)) {
                    neighbour = grid.GetNode(node.Y, node.X - 1);

                    _neighbours[count++] = neighbour;
                }

                if (AllNodes(grid, node.Y, node.X + agent.Width, agent.Height, 1, IsAir)) {
                    neighbour = grid.GetNode(node.Y, node.X + 1);

                    _neighbours[count++] = neighbour;
                }


                // Jump to the Right
                for (var x = 2; x <= agent.JumpStrength; ++x) {
                    neighbour = GetFallOnGroundNode(grid, node.Y + agent.JumpStrength, node.X + x, agent.Width);
                    if (neighbour == null)
                        continue;

                    if (CheckTrajectory(grid, agent, node, neighbour, x))
                        _neighbours[count++] = neighbour;
                }

                // Jump to the Left
                for (var x = -2; x >= -agent.JumpStrength; --x) {
                    neighbour = GetFallOnGroundNode(grid, node.Y + agent.JumpStrength, node.X + x, agent.Width);
                    if (neighbour == null)
                        continue;

                    if (CheckTrajectory(grid, agent, node, neighbour, x))
                        _neighbours[count++] = neighbour;
                }
            }
            // Falling Down. This should only happen if spawned above the ground.
            else
                _neighbours[count++] = grid.GetNode(node.Y - 1, node.X);

            return _neighbours;
        }

        /// <summary>
        /// Checks if all nodes in the square satisfy condition.
        /// </summary>
        /// <param name="grid">Grid</param>
        /// <param name="yStart"></param>
        /// <param name="xStart"></param>
        /// <param name="yCount">Should be greater than zero.</param>
        /// <param name="xCount">Should be greater than zero.</param>
        /// <param name="checkFunc">Function should return true, if satisfies.</param>
        /// <returns>True if all nodes satisfy condition.</returns>
        static bool AllNodes(PathFindingGrid grid, int yStart, int xStart, int yCount, int xCount,
            Func<Node, bool> checkFunc) {
            for (int y = 0; y < yCount; y++) {
                for (int x = 0; x < xCount; x++)
                    if (!checkFunc(grid.GetNode(yStart + y, xStart + x)))
                        return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if any node satisfies condition.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="yStart"></param>
        /// <param name="xStart"></param>
        /// <param name="yCount"></param>
        /// <param name="xCount"></param>
        /// <param name="checkFunc"></param>
        /// <returns>True if any node satisfies condition.</returns>
        static bool AnyNode(PathFindingGrid grid, int yStart, int xStart, int yCount, int xCount,
            Func<Node, bool> checkFunc) {
            for (int y = 0; y < yCount; y++) {
                for (int x = 0; x < xCount; x++)
                    if (checkFunc(grid.GetNode(yStart + y, xStart + x)))
                        return true;
            }

            return false;
        }

        static Node GetFallOnGroundNode(PathFindingGrid grid, int yStart, int xStart, int width) {
            // Make sure that it's not out of grid.
            for (int x = 0; x < width; x++) {
                if (grid.GetNode(yStart, xStart + x) == null)
                    return null;
            }

            while (!AnyNode(grid, yStart - 1, xStart, 1, width, IsGround)) {
                --yStart;
            }

            return grid.GetNode(yStart, xStart);
        }

        static bool CheckTrajectory(PathFindingGrid grid, PathFindingAgent agent, Node jumpStart, Node jumpLanding,
            int xDelta, int precision = 12) {
            
            float cellSize = grid.CellSize;

            Vector2 a = jumpStart.WorldPosition + new Vector2(-.5f, agent.Height - .5f) * cellSize;
            Vector2 b = a + Vector2.up * (agent.JumpStrength * cellSize);
            Vector2 c = b + Vector2.right * (xDelta * cellSize);
            Vector2 d = jumpLanding.WorldPosition + new Vector2(-.5f, agent.Height - .5f) * cellSize;

            var bezierCurve = new BezierCurve(a, b, c, d);

            bool isRight = xDelta > 0;

            Node lastNode = null;
            float offset = 1f / precision;
            float t = 0;
            for (int i = 0; i < precision - 1; i++) {
                t += offset;

                var curveValue = bezierCurve.GetValue(t);
                var node = grid.WorldPositionToNode(curveValue);

                if (node == lastNode) {
                    continue;
                }
                
                if (!AllNodes(grid, node.Y, node.X, 1, agent.Width + 1, IsAir))
                    return false;
                    
                if (isRight) {
                    if (!AllNodes(grid, node.Y - (agent.Height - 1), node.X + agent.Width,
                        agent.Height + 1, 1, IsAir)) {
                        return false;
                    }
                }
                else {
                    if (!AllNodes(grid, node.Y - (agent.Height - 1), node.X - 1,
                        agent.Height + 1, 1, IsAir)) {
                        return false;
                    }
                }

                lastNode = node;
            }

            return true;
        }
    }
}