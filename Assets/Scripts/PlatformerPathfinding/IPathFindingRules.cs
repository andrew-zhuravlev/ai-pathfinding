using System.Collections.Generic;

namespace PlatformerPathFinding {
    public interface IPathFindingRules {
        int GetHeuristic(Node node, Node goal);
        int GetCost(Node fromNode, Node toNode);
        IEnumerable<Node> GetNeighbours(Grid grid, Node node);
    }
}