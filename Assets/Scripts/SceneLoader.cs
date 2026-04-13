using UnityEngine.SceneManagement;

namespace LostEnergy
{
    /// <summary>
    /// Tüm sahne geçişlerini LoadingScreen üzerinden yönlendirir.
    /// SceneManager.LoadScene() yerine her yerde bunu kullan.
    /// </summary>
    public static class SceneLoader
    {
        private const string LOADING_SCENE = "LoadingScreen";

        /// <summary>LoadingScreen'in yükleyeceği hedef sahne adı.</summary>
        public static string TargetScene { get; private set; }

        /// <summary>LoadingScreen'in en az kaç saniye gösterileceği.</summary>
        public static float MinDisplayTime { get; private set; } = 1.5f;

        /// <param name="minDisplayTime">Yükleme bitse bile kaç saniye beklensin. 0 = anında geç.</param>
        public static void LoadScene(string sceneName, float minDisplayTime = 1.5f)
        {
            TargetScene    = sceneName;
            MinDisplayTime = minDisplayTime;
            SceneManager.LoadScene(LOADING_SCENE);
        }
    }
}
