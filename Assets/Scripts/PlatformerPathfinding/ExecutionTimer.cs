using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace PlatformerPathFinding {
    public static class ExecutionTimer {
        public static void LogExecutionTime(Action action, string methodName) {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            action();

            sw.Stop();
            Debug.Log(methodName + ": " + sw.ElapsedMilliseconds + " ms.");
        }
    }
}