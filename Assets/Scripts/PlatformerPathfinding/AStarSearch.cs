using System.Collections.Generic;

namespace PlatformerPathFinding {
    public static class AStarSearch {
        public static List<Node> Search(this PathFindingGrid pathFindingGrid, Node start, Node goal, 
            IPathFindingRules rules, PathFindingAgent agent) {
            
            // TODO: Remove new allocation.
            var openSet = new Heap<Node>(pathFindingGrid.MaxSize);
            var closedSet = new HashSet<Node>();
            openSet.Add(start);
            var foundGoal = false;

            while (openSet.Count > 0) {
                Node node = openSet.RemoveFirst();
                closedSet.Add(node);

                if (node == goal) {
                    foundGoal = true;     
                    break;                    
                }

                foreach (Node neighbour in rules.GetNeighbours(pathFindingGrid, node, agent)) {
                    if (/*!neighbour.IsWalkable ||*/ closedSet.Contains(neighbour))
                        continue;

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

            return foundGoal ? RetracePath(start, goal) : null;
        }

        // TODO: Capacity via Parent: counter.
        static List<Node> RetracePath(Node start, Node goal) {
            var path = new List<Node>();
            Node currentNode = goal;
            while (currentNode != start) {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}