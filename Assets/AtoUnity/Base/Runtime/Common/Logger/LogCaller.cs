using System.Runtime.CompilerServices;

namespace AtoGame.Base {
    public static class LogExtension {
        public static void LogCaller(
              [CallerLineNumber] int line = 0
            , [CallerMemberName] string memberName = ""
            , [CallerFilePath] string filePath = ""
        )
        {
            UnityEngine.Debug.Log($"{line} :: {memberName} :: {filePath}");
        }
    }
}