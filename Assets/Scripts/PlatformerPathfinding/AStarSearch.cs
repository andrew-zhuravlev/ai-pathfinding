using System.Collections.Generic;

namespace PlatformerPathFinding {
    public class AStarSearch {
        
        readonly Heap<Node> _openSet;
        readonly HashSet<Node> _closedSet = new HashSet<Node>();

        readonly PathFindingGrid _pathFindingGrid;
        
        public AStarSearch(PathFindingGrid pathFindingGrid) {
            _pathFindingGrid = pathFindingGrid;
            _openSet = new Heap<Node>(pathFindingGrid.MaxSize);
        }
        
        public List<Node> Search(Node start, Node goal, IPathFindingRules rules, PathFindingAgent agent) {
            
            _openSet.Clear();
            _closedSet.Clear();
            
            _openSet.Add(start);
            var foundGoal = false;

            while (_openSet.Count > 0) {
                Node node = _openSet.RemoveFirst();
                _closedSet.Add(node);

                if (node == goal) {
                    foundGoal = true;     
                    break;                    
                }

                int neighboursCount;
                var nodes = rules.GetNeighbours(_pathFindingGrid, node, agent, out neighboursCount);
                for (var i = 0; i < neighboursCount; i++) {
                    Node neighbour = nodes[i];
                    if ( /*!neighbour.IsWalkable ||*/ _closedSet.Contains(neighbour))
                        continue;

                    int newCost = node.GCost + rules.GetCost(node, neighbour, agent);
                    if (newCost < neighbour.GCost || !_openSet.Contains(neighbour)) {
                        neighbour.GCost = newCost;
                        neighbour.HCost = rules.GetHeuristic(neighbour, goal, agent);
                        neighbour.Parent = node;

                        if (!_openSet.Contains(neighbour))
                            _openSet.Add(neighbour);
                        else
                            _openSet.UpdateItem(neighbour);
                    }
                }
            }

            return foundGoal ? RetracePath(start, goal) : null;
        }

        static List<Node> RetracePath(Node start, Node goal) {
            var path = new List<Node>();
            Node currentNode = goal;

            TransitionType previousTransition = TransitionType.None;
            
            while (currentNode != start) {

                Node parent = currentNode.Parent;
                var currentTransition = Node.IsWalkTransition(parent, currentNode);

                if (!(previousTransition == TransitionType.Walk && currentTransition == TransitionType.Walk) 
                    && !(previousTransition == TransitionType.Fall && currentTransition == TransitionType.Fall))
                    path.Add(currentNode);

                previousTransition = currentTransition;
                currentNode = parent;
            }
            
            path.Reverse();
            return path;
        }
    }
}