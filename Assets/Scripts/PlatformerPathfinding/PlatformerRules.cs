using System.Collections.Generic;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PlatformerRules : IPathFindingRules {
        public int GetHeuristic(Node node, Node goal) {
            return Mathf.Abs(node.X - goal.X) + Mathf.Abs(node.Y - goal.Y);
        }

        public int GetDistance(Node start, Node neighbour) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Node> GetNeighbours(Grid grid, Node node) {
            var neighbours = new List<Node>();

            bool isGrounded = !grid.CheckNode(node.Y - 1, node.X) || !grid.GetNode(node.Y - 1, node.X).IsEmpty;

            if (isGrounded) {
                // Left
                if(grid.CheckNode(node.Y, node.X - 1) && grid.GetNode(node.Y, node.X - 1).IsEmpty)
                    neighbours.Add(grid.GetNode(node.Y, node.X - 1));
                // Right
                if(grid.CheckNode(node.Y, node.X + 1) && grid.GetNode(node.Y, node.X + 1).IsEmpty)
                    neighbours.Add(grid.GetNode(node.Y, node.X + 1));
            }
            
            return neighbours;
        }
    }
}