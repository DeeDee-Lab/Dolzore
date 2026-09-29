using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dolzore
{
    public sealed class TitleScreenController : MonoBehaviour
    {
        public Button newGameButton;
        public Button continueButton;
        public Button settingsButton;
        public Button bgmButton;
        public Button settingsCloseButton;
        public GameObject settingsPanel;
        public Text bgmLabel;
        public Text statusLabel;
        public Image fadeImage;
        public AudioSource ambientSource;

        private bool transitioning;
        private bool bgmEnabled;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            bgmEnabled = PlayerPrefs.GetInt("dolzore.bgm", 1) == 1;
            if (continueButton != null)
                continueButton.interactable = PlayerPrefs.HasKey("dolzore.save.0");
            if (settingsPanel != null)
                settingsPanel.SetActive(false);
            ApplyBgmState(false);
        }

        private void Start()
        {
            if (newGameButton != null) newGameButton.onClick.AddListener(OnNewGame);
            if (settingsButton != null) settingsButton.onClick.AddListener(OnSettings);
            if (bgmButton != null) bgmButton.onClick.AddListener(OnToggleBgm);
            if (settingsCloseButton != null) settingsCloseButton.onClick.AddListener(OnCloseSettings);
            if (EventSystem.current != null && newGameButton != null)
                EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        }

        public void OnNewGame()
        {
            if (!transitioning)
                StartCoroutine(FadeToScene("FirstTownShell"));
        }

        public void OnSettings()
        {
            if (settingsPanel == null) return;
            settingsPanel.SetActive(true);
            if (EventSystem.current != null && settingsCloseButton != null)
                EventSystem.current.SetSelectedGameObject(settingsCloseButton.gameObject);
        }

        public void OnCloseSettings()
        {
            if (settingsPanel == null) return;
            settingsPanel.SetActive(false);
            if (EventSystem.current != null && settingsButton != null)
                EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
        }

        public void OnToggleBgm()
        {
            bgmEnabled = !bgmEnabled;
            PlayerPrefs.SetInt("dolzore.bgm", bgmEnabled ? 1 : 0);
            PlayerPrefs.Save();
            ApplyBgmState(true);
        }

        private void ApplyBgmState(bool userGesture)
        {
            if (bgmLabel != null)
                bgmLabel.text = bgmEnabled ? "BGM  ON" : "BGM  OFF";

            if (ambientSource == null) return;
            ambientSource.mute = !bgmEnabled;
            if (bgmEnabled && userGesture && !ambientSource.isPlaying)
                ambientSource.Play();
        }

        private IEnumerator FadeToScene(string sceneName)
        {
            transitioning = true;
            if (statusLabel != null)
                statusLabel.text = "CONNECTING TO FIRST TOWN...";

            float t = 0f;
            Color c = fadeImage != null ? fadeImage.color : Color.black;
            while (t < 0.55f)
            {
                t += Time.unscaledDeltaTime;
                float a = Mathf.SmoothStep(0f, 1f, t / 0.55f);
                if (fadeImage != null)
                {
                    c.a = a;
                    fadeImage.color = c;
                }
                if (ambientSource != null)
                    ambientSource.volume = Mathf.Lerp(0.18f, 0f, a);
                yield return null;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
