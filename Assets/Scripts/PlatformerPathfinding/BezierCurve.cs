using System.Threading;
using UnityEngine;

namespace PlatformerPathFinding {
    public struct BezierCurve {
        readonly Vector2 _a, _b, _c, _d;

        public BezierCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }
        
        public Vector2 GetValue(float t) {
            float oneMinusT = 1 - t;
            return oneMinusT * oneMinusT * oneMinusT * _a + 3 * oneMinusT * oneMinusT * t * _b +
                   3 * oneMinusT * t * t * _c + t * t * t * _d;
        }
    }
}