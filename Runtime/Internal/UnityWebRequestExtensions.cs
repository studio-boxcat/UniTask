using UnityEngine.Networking;

namespace Cysharp.Threading.Tasks.Internal
{
    internal static class UnityWebRequestResultExtensions
    {
        public static bool IsError(this UnityWebRequest unityWebRequest)
        {
            return unityWebRequest.result
                is UnityWebRequest.Result.ConnectionError
                or UnityWebRequest.Result.DataProcessingError
                or UnityWebRequest.Result.ProtocolError;
        }
    }
}