using System;
using System.IO;
using UnityEngine;

namespace LostEnergy
{
    /// <summary>
    /// File-based game logger that works in editor and builds.
    /// Usage: GameLogger.Instance.LogEvent("EVENT_TYPE", "detail");
    /// Log file: Application.persistentDataPath/game_log.txt
    /// </summary>
    public class GameLogger : MonoBehaviour
    {
        public static GameLogger Instance { get; private set; }

        private string _logFilePath;
        private readonly object _lock = new object();

        // Auto-spawned before any scene loads — no manual setup required.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInitialize()
        {
            if (Instance != null) return;

            GameObject go = new GameObject("[GameLogger]");
            go.AddComponent<GameLogger>();
            DontDestroyOnLoad(go);
        }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _logFilePath = Path.Combine(Application.persistentDataPath, "game_log.txt");

            WriteSessionHeader();

            Debug.Log($"[GameLogger] Log file: {_logFilePath}");
        }

        /// <summary>
        /// Appends one event line to the log file.
        /// Format: [yyyy-MM-dd HH:mm:ss] | eventType | detail
        /// </summary>
        /// <param name="eventType">Event type (e.g. "CRYSTAL", "DEATH", "PAUSE")</param>
        /// <param name="detail">Event detail</param>
        public void LogEvent(string eventType, string detail)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{timestamp}] | {eventType} | {detail}";

            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, line + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[GameLogger] Failed to write log: {ex.Message}");
                }
            }
        }

        private void WriteSessionHeader()
        {
            string separator = new string('=', 60);
            string header = $"{separator}{Environment.NewLine}" +
                            $"SESSION START | {DateTime.Now:yyyy-MM-dd HH:mm:ss} | v{Application.version}{Environment.NewLine}" +
                            $"Platform: {Application.platform} | Unity: {Application.unityVersion}{Environment.NewLine}" +
                            $"{separator}{Environment.NewLine}";

            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, header);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[GameLogger] Failed to write session header: {ex.Message}");
                }
            }
        }
    }
}
