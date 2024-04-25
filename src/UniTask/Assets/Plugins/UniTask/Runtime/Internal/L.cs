using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Cysharp.Threading.Tasks.Internal
{
    static class L
    {
        [Conditional("DEBUG"), HideInCallstack]
        public static void I(string message)
        {
            Debug.Log(message);
        }
    }
}