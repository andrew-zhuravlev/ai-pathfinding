using System.Collections.Generic;

namespace PlatformerPathFinding {
//    class PlatformerNeighbourProvider :  {
//
//        NodeJumpData[,] _jumpData;
//        
//        public IEnumerable<Node> GetNeighbours(Grid grid, Node node) {
//            var neighbours = new List<Node>();
//
//            bool isGrounded = !grid.CheckNode(node.Y - 1, node.X) || !grid.GetNode(node.Y - 1, node.X).IsEmpty;
//
//            if (isGrounded) {
//                // Left
//                if(grid.CheckNode(node.Y, node.X - 1) && grid.GetNode(node.Y, node.X - 1).IsEmpty)
//                    neighbours.Add(grid.GetNode(node.Y, node.X - 1));
//                // Right
//                if(grid.CheckNode(node.Y, node.X + 1) && grid.GetNode(node.Y, node.X + 1).IsEmpty)
//                    neighbours.Add(grid.GetNode(node.Y, node.X + 1));
//            }
//            
//            return neighbours;
//        }
//    }

//    public class NodeJumpData {
//        public int UpNodesLeft;
//        public bool IsJump;
//    }
}