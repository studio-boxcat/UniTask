using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Cysharp.Threading.Tasks
{
    static class L
    {
        [Conditional("DEBUG"), HideInCallstack]
        public static void I(string message) => Debug.Log(message);
        [Conditional("DEBUG"), HideInCallstack]
        public static void W(string message) => Debug.LogWarning(message);

        [HideInCallstack]
        public static void E(Exception ex)
        {
            if (ex is null or OperationCanceledException) return;
            Debug.LogException(ex);
        }
    }
}