using System.Collections.Generic;

namespace PlatformerPathFinding {
    public interface INeighboursProvider {
        IEnumerable<Node> GetNeighbours(Grid grid, Node node);
    }
}