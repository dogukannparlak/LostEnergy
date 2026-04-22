using UnityEngine.SceneManagement;

namespace LostEnergy
{
    /// <summary>
    /// Routes all scene transitions through the LoadingScreen scene.
    /// Use this instead of SceneManager.LoadScene() everywhere.
    /// </summary>
    public static class SceneLoader
    {
        private const string LOADING_SCENE = "LoadingScreen";

        /// <summary>Target scene to load after the loading screen.</summary>
        public static string TargetScene { get; private set; }

        /// <summary>Minimum seconds the loading screen will be shown.</summary>
        public static float MinDisplayTime { get; private set; } = 1.5f;

        /// <param name="minDisplayTime">Seconds to wait even if load finishes early. 0 = instant.</param>
        public static void LoadScene(string sceneName, float minDisplayTime = 1.5f)
        {
            TargetScene    = sceneName;
            MinDisplayTime = minDisplayTime;
            SceneManager.LoadScene(LOADING_SCENE);
        }
    }
}
