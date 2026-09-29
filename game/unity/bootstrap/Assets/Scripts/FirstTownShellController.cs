using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dolzore
{
    public sealed class FirstTownShellController : MonoBehaviour
    {
        public Text clockLabel;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                SceneManager.LoadScene("Title");

            if (clockLabel != null)
            {
                int sec = Mathf.FloorToInt(Time.unscaledTime) % 60;
                clockLabel.text = "PRESENT  //  00:" + sec.ToString("00");
            }
        }
    }
}
