using System.Collections.Generic;

namespace PlatformerPathFinding {
    public static class AStarSearch {
        
        public delegate int Heuristics(Node start, Node goal);
        
        public static List<Node> Search(this Grid grid, Node start, Node goal, INeighboursProvider neighboursProvider,
            Heuristics heuristics) {
            
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            openSet.Add(start);

            while (openSet.Count > 0) {
                // Get the node with lowest FCost
                Node node = openSet[0];
                for (var i = 1; i < openSet.Count; i ++) {
                    if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost) {
                        if (openSet[i].HCost < node.HCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == goal)
                    break;

                foreach (Node neighbour in neighboursProvider.GetNeighbours(grid, node)) {
                    if (/*!neighbour.IsWalkable ||*/ closedSet.Contains(neighbour))
                        continue;

                    int newCostToNeighbour = node.GCost + 1;
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = heuristics(neighbour, goal);
                        neighbour.Parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return RetracePath(goal);
        }

        static List<Node> RetracePath(Node goal) {
            var result = new List<Node> { goal };
            var currentNode = goal.Parent;

            while (currentNode != null) {
                result.Insert(0, currentNode);
                currentNode = currentNode.Parent;
            }
            return result;
        }
    }
}