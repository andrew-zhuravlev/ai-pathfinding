using System.Collections.Generic;
using PlatformerPathFinding;
using UnityEngine;

public class PathFindingAgent : MonoBehaviour {
    [SerializeField] PathFindingGrid _pathFindingGrid;
    [SerializeField] Transform _goalObject;
    [SerializeField] int _height;
    [SerializeField] int _width;
    [SerializeField] int _jumpHeight = 5;
    [SerializeField] int _jumpHorizontal = 5;

    public int JumpHeight => _jumpHeight;
    public int JumpHorizontal => _jumpHorizontal;

    public int Height => _height;
    public int Width => _width;

    List<Vector2> _path;
    
    void Update() {
        _path = _pathFindingGrid.FindPath(this, _goalObject);
    }

    void OnDrawGizmos() {
        if (_path == null || _path.Count == 0)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _path[0]);
        for (int i = 0; i < _path.Count - 1; i++) {
            Gizmos.color = Gizmos.color == Color.blue ? Color.magenta : Color.blue;
            Gizmos.DrawLine(_path[i], _path[i + 1]);
        }
    }
}
