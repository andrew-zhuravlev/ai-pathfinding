using System.Collections.Generic;

namespace PlatformerPathFinding {
    public static class AStarSearch {
        public static List<Node> Search(this Grid grid, Node start, Node goal, IPathFindingRules rules,
            PathFindingAgent agent) {
            
            // TODO: Use heap.
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            openSet.Add(start);
            var foundGoal = false;

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

                if (node == goal) {
                    foundGoal = true;     
                    break;                    
                }

                foreach (Node neighbour in rules.GetNeighbours(grid, node, agent)) {
                    if (/*!neighbour.IsWalkable ||*/ closedSet.Contains(neighbour))
                        continue;

                    //int newCost = node.GCost + 1;
                    int newCost = rules.GetCost(node, neighbour, agent);
                    if (newCost < neighbour.GCost || !openSet.Contains(neighbour)) {
                        neighbour.GCost = newCost;
                        neighbour.HCost = rules.GetHeuristic(neighbour, goal, agent);
                        neighbour.Parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return foundGoal ? RetracePath(goal) : null;
        }

        // TODO: Capacity via Parent: counter.
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