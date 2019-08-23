using UnityEngine;

namespace PlatformerPathFinding {
    public class Node : IHeapItem<Node> {
        public bool IsEmpty { get; }
        public Vector2 WorldPosition { get; }

        public int Y { get; }
        public int X { get; }

        // Distance from starting node
        public int GCost { get; set; }

        // Distance to end node. (Heuristic).
        public int HCost { private get; set; }

        // G cost + H cost
        int FCost => GCost + HCost;

        public Node Parent { get; set; }
        
        public Node(bool isEmpty, Vector2 worldPosition, int x, int y) {
            IsEmpty = isEmpty;
            WorldPosition = worldPosition;
            X = x;
            Y = y;
        }

        public static bool IsWalkTransition(Node fromNode, Node toNode) {
            int deltaY = toNode.Y - fromNode.Y,
                deltaX = toNode.X - fromNode.X;
            return deltaY == 0 && (deltaX == -1 || deltaX == 1);
        }

        public int HeapIndex { get; set; }

        public int CompareTo(Node nodeToCompare) {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0) {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }

            return -compare;
        }
    }
}