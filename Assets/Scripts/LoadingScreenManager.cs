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
        public TMP_Text progressText;       // "75%" yazısı
        public Image    fillImage;          // Slider'ın Fill alanı — renk animasyonu için

        [Header("Loading Title")]
        public TMP_Text loadingTitleText;   // "LOADING..." yazısı
        [Tooltip("Nokta animasyonu hızı (saniye başına değişim).")]
        public float dotSpeed = 0.4f;

        [Header("Loading Image")]
        public Image loadingImage;          // arka plan veya ipucu resmi
        public Sprite[] loadingSprites;     // birden fazla varsa random seçilir

        [Header("Settings")]
        [Tooltip("SceneLoader.LoadScene() ile override edilmezse bu değer kullanılır.")]
        public float fallbackDisplayTime = 1.5f;

        [Header("Editor Test")]
        [Tooltip("Sadece Editor'da direkt Play basınca kullanılır. Build'de etkisi yok.")]
        public string editorTestScene = "SampleScene";

        private float _dotTimer;
        private int   _dotCount;

        void Start()
        {
            // Resim seç
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
                Debug.LogWarning("[LoadingScreen] TargetScene boş — Editor test modu: " + editorTestScene);
                target = editorTestScene;
#else
                Debug.LogError("[LoadingScreen] Hedef sahne boş! SceneLoader.LoadScene() kullanımına geç.");
                return;
#endif
            }

            // SceneLoader'da ayarlanan süreyi kullan; yoksa fallback değer
            float minTime = SceneLoader.MinDisplayTime > 0f
                ? SceneLoader.MinDisplayTime
                : fallbackDisplayTime;

            StartCoroutine(LoadAsync(target, minTime));
        }

        void Update()
        {
            // "LOADING." → "LOADING.." → "LOADING..." animasyonu
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
            float displayedProgress = 0f;   // ekranda gösterilen yumuşak değer

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            while (!op.isDone)
            {
                elapsed += Time.deltaTime;

                // Gerçek yükleme ilerlemesi (0 → 1)
                float realProgress = Mathf.Clamp01(op.progress / 0.9f);

                // Süreye göre beklenen görsel ilerleme (minDisplayTime boyunca 0 → 1)
                float timedProgress = minDisplayTime > 0f
                    ? Mathf.Clamp01(elapsed / minDisplayTime)
                    : realProgress;

                // İkisinin minimumunu al — gerçek yükleme geri gitmesin,
                // ama zamandan da önce %100 görünmesin
                float targetProgress = Mathf.Min(realProgress, timedProgress);

                // Lerp ile yumuşat (hızı ayarlamak için 8f'yi değiştirebilirsin)
                displayedProgress = Mathf.Lerp(displayedProgress, targetProgress, Time.deltaTime * 8f);

                if (progressBar  != null) progressBar.value = displayedProgress;
                if (progressText != null) progressText.text = Mathf.RoundToInt(displayedProgress * 100f) + "%";

                // Gerçek yükleme bitti VE minimum süre doldu VE görsel %99'a ulaştı
                if (op.progress >= 0.9f && elapsed >= minDisplayTime && displayedProgress >= 0.99f)
                    op.allowSceneActivation = true;

                yield return null;
            }
        }
    }
}
