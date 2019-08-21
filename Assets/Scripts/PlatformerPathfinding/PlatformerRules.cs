using System;
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

        // TODO: IT uses try catch, RAFACTOR!
        static Node GetFallOnGroundNode(Grid grid, int y, int x) {
            try {
                while (!IsGroundedNode(grid, grid.GetNode(y, x)))
                    --y;
                return grid.GetNode(y, x);
            }
            catch (Exception e) {
                return null;
            }
        }

        public IEnumerable<Node> GetNeighbours(PathFindingController pathFindingController, Node node, 
            PathFindingAgent agent) {
            var neighbours = new List<Node>();

            var jumpHeight = agent.JumpHeight;
            var jumpHorizontal = agent.JumpHorizontal;

            var height = agent.Height;
            var width = agent.Width;

            var grid = pathFindingController.Grid;
            
            bool isGrounded = IsGroundedNode(grid, node);
            if (isGrounded) {
                // Left
                bool isLeft = true;
                for (int y = 0; y < height; y++) {
                    if (!grid.IsEmptyNode(node.Y + y, node.X - 1))
                        isLeft = false;
                }
                if (isLeft)
                    neighbours.Add(grid.GetNode(node.Y, node.X - 1));
                
                
                // Right
                bool isRight = true;
                for (int y = 0; y < height; y++) {
                    if (!grid.IsEmptyNode(node.Y + y, node.X + width))
                        isRight = false;
                }
                if (isRight)
                    neighbours.Add(grid.GetNode(node.Y, node.X + width));

                
                // Jump to the Right
                for (var x = 2; x <= jumpHorizontal; ++x) {
                    Node jumpEnd = GetFallOnGroundNode(grid, node.Y + jumpHeight, node.X + x);
                    
                    //TODO: REFACTOR.
                    if (jumpEnd == null)
                        continue;
                    
                    if(CheckTrajectory(pathFindingController, node, jumpEnd, jumpHeight, height, width, x))
                        neighbours.Add(jumpEnd);
                }

                // Jump to the Left
                for (var x = -2; x >= -jumpHorizontal; --x) {
                    Node jumpEnd = GetFallOnGroundNode(grid, node.Y + jumpHeight, node.X + x);
                    
                    //TODO: REFACTOR.
                    if (jumpEnd == null)
                        continue;
                    
                    if(CheckTrajectory(pathFindingController, node, jumpEnd, jumpHeight, height, width, x))
                    neighbours.Add(jumpEnd);
                }
            }
            // Falling Down. This should only happen if spawned above the ground.
            else
                neighbours.Add(grid.GetNode(node.Y - 1, node.X));

            return neighbours;
        }

        // TODO: Refactor input parameters.
        static bool CheckTrajectory(PathFindingController pathFindingController, Node jumpStart, Node jumpEnd, 
            int jumpHeight, int agentHeight, int agentWidth, int xDelta) {
            
            Vector2 p0 = jumpStart.WorldPositionCenter;
            Vector2 p1 = p0 + Vector2.up * jumpHeight * pathFindingController.Grid.CellSize;
            Vector2 p2 = p1 + Vector2.right * (xDelta / 2f) * pathFindingController.Grid.CellSize;
            Vector2 p3 = p2 + Vector2.right * (xDelta / 2f) * pathFindingController.Grid.CellSize;
            Vector2 p4 = jumpEnd.WorldPositionCenter;

            // TODO: Number of points instead of offset. For now its easier.
            const float offset = 0.1f;
            for (float t = offset; t < 1; t += offset) {
                var curveDot = QuadraticCurve(p0, p1, p2, t);
                // TODO: it should be optimized
                Vector2Int nodeXyWorld = pathFindingController.WorldPositionToNodeXY(curveDot);
                if (!pathFindingController.Grid.IsEmptyNode(nodeXyWorld.y, nodeXyWorld.x))
                    return false;
            }
            
            for (float t = offset; t < 1; t += offset) {
                var curveDot = QuadraticCurve(p2, p3, p4, t);
                // TODO: it should be optimized
                Vector2Int nodeXyWorld = pathFindingController.WorldPositionToNodeXY(curveDot);
                if (!pathFindingController.Grid.IsEmptyNode(nodeXyWorld.y, nodeXyWorld.x))
                    return false;
            }

            return true;
        }

        static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector3 c, float t) {
            Vector2 p0 = Vector2.Lerp(a, b, t);
            Vector2 p1 = Vector2.Lerp(b, c, t);
            return Vector2.Lerp(p0, p1, t);
        }
    }
}