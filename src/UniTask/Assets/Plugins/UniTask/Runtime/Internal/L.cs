using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Cysharp.Threading.Tasks.Internal
{
    static class L
    {
        [Conditional("DEBUG")]
        public static void I(string message)
        {
            Debug.Log(message);
        }
    }
}