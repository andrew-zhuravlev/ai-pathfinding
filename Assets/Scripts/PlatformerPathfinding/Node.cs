using UnityEngine;

namespace PlatformerPathFinding {
    public class Node {
        public bool IsWalkable { get; }
        Vector2 _worldPositionCenter;

        public int Y { get; }
        public int X { get; }

        // Distance from starting node
        public int GCost { get; set; }

        // Distance to end node. (Heuristic).
        public int HCost { get; set; }

        // G cost + H cost
        public int FCost => GCost + HCost;

        public Node Parent { get; set; }

        public Node(bool isWalkable, Vector2 worldPositionCenter, int x, int y) {
            IsWalkable = isWalkable;
            _worldPositionCenter = worldPositionCenter;
            X = x;
            Y = y;
        }
    }
}