using System.Collections.Generic;

namespace PlatformerPathFinding {
    public class FourSideNeighbours : INeighboursProvider {
        public IEnumerable<Node> GetNeighbours(Grid grid, Node node) {
            var neighbours = new List<Node>();
            // Left
            if(grid.CheckNode(node.Y, node.X - 1))
                neighbours.Add(grid.GetNode(node.Y, node.X - 1));
            // Right
            if(grid.CheckNode(node.Y, node.X + 1))
                neighbours.Add(grid.GetNode(node.Y, node.X + 1));
            // Top
            if(grid.CheckNode(node.Y + 1, node.X))
                neighbours.Add(grid.GetNode(node.Y + 1, node.X));
            // Bottom
            if(grid.CheckNode(node.Y - 1, node.X))
                neighbours.Add(grid.GetNode(node.Y - 1, node.X));

            return neighbours;
        }
    }
}