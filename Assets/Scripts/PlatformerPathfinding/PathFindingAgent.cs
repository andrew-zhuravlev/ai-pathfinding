using System.Collections.Generic;
using System.Diagnostics;
using PlatformerPathFinding;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathFindingAgent : MonoBehaviour {
    [SerializeField] PathFindingGrid _pathFindingGrid;
    [SerializeField] Transform _goalObject;
    [SerializeField] int _height;
    [SerializeField] int _width;
    [SerializeField] int _jumpStrength = 5;

    public int JumpStrength => _jumpStrength;

    public int Height => _height;
    public int Width => _width;

    List<Node> _path;
    
    void Update() {
        // (!Input.GetMouseButtonDown(0))
            //return;
            
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        
        _path = _pathFindingGrid.FindPath(this, _goalObject);
        
        //sw.Stop();
        //Debug.Log(sw.ElapsedMilliseconds);
    }

    void OnDrawGizmos() {
        if (_path == null || _path.Count == 0)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _path[0].WorldPosition);
        for (int i = 0; i < _path.Count - 1; i++) {
            Gizmos.color = Gizmos.color == Color.blue ? Color.magenta : Color.blue;
            Gizmos.DrawLine(_path[i].WorldPosition, _path[i + 1].WorldPosition);
        }
    }
}
