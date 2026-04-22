using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace LostEnergy
{
    public class LoadingScreenManager : MonoBehaviour
    {
        [Header("Progress Bar")]
        public Slider progressBar;
        public TMP_Text progressText;       // Displays percentage text
        public Image    fillImage;          // Slider fill image for color animation

        [Header("Loading Title")]
        public TMP_Text loadingTitleText;   // Loading title text
        [Tooltip("Dot animation speed (changes per second).")]
        public float dotSpeed = 0.4f;

        [Header("Loading Image")]
        public Image loadingImage;
        public Sprite[] loadingSprites;

        [Header("Settings")]
        [Tooltip("Used when not overridden by SceneLoader.LoadScene().")]
        public float fallbackDisplayTime = 1.5f;

        [Header("Editor Test")]
        [Tooltip("Editor only: used when Play is pressed directly without SceneLoader.")]
        public string editorTestScene = "SampleScene";

        private float _dotTimer;
        private int   _dotCount;

        void Start()
        {
            // Select a random loading image
            if (loadingImage != null && loadingSprites != null && loadingSprites.Length > 0)
                loadingImage.sprite = loadingSprites[Random.Range(0, loadingSprites.Length)];

            if (progressBar != null)
            {
                progressBar.minValue = 0f;
                progressBar.maxValue = 1f;
                progressBar.value    = 0f;
            }

            string target = SceneLoader.TargetScene;

            if (string.IsNullOrEmpty(target))
            {
#if UNITY_EDITOR
                Debug.LogWarning("[LoadingScreen] TargetScene is empty — using editor test scene: " + editorTestScene);
                target = editorTestScene;
#else
                Debug.LogError("[LoadingScreen] Target scene is empty! Use SceneLoader.LoadScene() instead.");
                return;
#endif
            }

            // Use duration from SceneLoader, or fall back to default
            float minTime = SceneLoader.MinDisplayTime > 0f
                ? SceneLoader.MinDisplayTime
                : fallbackDisplayTime;

            StartCoroutine(LoadAsync(target, minTime));
        }

        void Update()
        {
            // Animate loading dots: "LOADING." → "LOADING.." → "LOADING..."
            if (loadingTitleText == null) return;
            _dotTimer += Time.deltaTime;
            if (_dotTimer >= dotSpeed)
            {
                _dotTimer = 0f;
                _dotCount = (_dotCount + 1) % 4;
                loadingTitleText.text = "LOADING" + new string('.', _dotCount);
            }
        }

        private IEnumerator LoadAsync(string sceneName, float minDisplayTime)
        {
            float elapsed         = 0f;
            float displayedProgress = 0f;

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            while (!op.isDone)
            {
                elapsed += Time.deltaTime;

                // Real load progress (0 → 1)
                float realProgress = Mathf.Clamp01(op.progress / 0.9f);

                // Time-based visual progress over minDisplayTime (0 → 1)
                float timedProgress = minDisplayTime > 0f
                    ? Mathf.Clamp01(elapsed / minDisplayTime)
                    : realProgress;

                // Take the minimum so real progress never goes backward
                // and visual progress never reaches 100% before time elapses
                float targetProgress = Mathf.Min(realProgress, timedProgress);

                // Smooth with lerp
                displayedProgress = Mathf.Lerp(displayedProgress, targetProgress, Time.deltaTime * 8f);

                if (progressBar  != null) progressBar.value = displayedProgress;
                if (progressText != null) progressText.text = Mathf.RoundToInt(displayedProgress * 100f) + "%";

                // Activate once load is done, min time elapsed, and visual progress is complete
                if (op.progress >= 0.9f && elapsed >= minDisplayTime && displayedProgress >= 0.99f)
                    op.allowSceneActivation = true;

                yield return null;
            }
        }
    }
}
