using System;
using UnityEngine;

namespace BorschtCraft
{
    public class Logger : Debug
    {
        public static void LogInfo(object sender,string message)
        {
            Log($"[INFO] {sender.GetType().Name}: {message}");
        }

        public static void LogWarning(object sender, string message)
        {
            LogWarning($"[WARNING] {sender.GetType().Name}: {message}");
        }

        public static void LogError(object sender, string message)
        {
            LogError($"[ERROR] {sender.GetType().Name}: {message}");
        }

        public static void LogInfoFormat(object sender, string format, params object[] args)
        {
            LogFormat($"[INFO] {sender.GetType().Name}: {format}", args);
        }

        public static void LogWarningFormat(object sender, string format, params object[] args)
        {
            LogFormat($"[WARNING] {sender.GetType().Name}: {format}", args);
        }

        public static void LogErrorFormat(object sender, string format, params object[] args)
        {
            LogFormat($"[ERROR] {sender.GetType().Name}: {format}", args);
        }

        public static void LogException(object sender, Type exceptionType, string message)
        {
            var exceptionMessage = $"[EXCEPTION] {sender.GetType().Name}: {message}";
            var exception = Activator.CreateInstance(exceptionType, exceptionMessage) as Exception;
            LogException(exception);
        }
    }
}