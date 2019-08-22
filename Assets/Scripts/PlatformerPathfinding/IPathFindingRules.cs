using System.Collections.Generic;

namespace PlatformerPathFinding {
    public interface IPathFindingRules {
        int GetHeuristic(Node node, Node goal, PathFindingAgent agent);
        int GetCost(Node fromNode, Node toNode, PathFindingAgent agent);
        IEnumerable<Node> GetNeighbours(PathFindingGrid pathFindingGrid, Node node,  PathFindingAgent agent);
    }
}