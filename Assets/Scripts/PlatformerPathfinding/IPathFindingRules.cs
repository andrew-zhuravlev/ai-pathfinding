using System.Collections.Generic;

namespace PlatformerPathFinding {
    public interface IPathFindingRules {
        int GetHeuristic(Node node, Node goal);
        int GetDistance(Node node, Node neighbour);
        IEnumerable<Node> GetNeighbours(Grid grid, Node node);
    }
}