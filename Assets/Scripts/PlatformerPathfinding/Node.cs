using UnityEngine;

namespace PlatformerPathFinding {
    public class Node : IHeapItem<Node> {
        public bool IsEmpty { get; }
        public Vector2 WorldPositionCenter { get; }

        public int Y { get; }
        public int X { get; }

        // Distance from starting node
        public int GCost { get; set; }

        // Distance to end node. (Heuristic).
        public int HCost { get; set; }

        // G cost + H cost
        public int FCost => GCost + HCost;

        public Node Parent { get; set; }

        public Node(bool isEmpty, Vector2 worldPositionCenter, int x, int y) {
            IsEmpty = isEmpty;
            WorldPositionCenter = worldPositionCenter;
            X = x;
            Y = y;
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