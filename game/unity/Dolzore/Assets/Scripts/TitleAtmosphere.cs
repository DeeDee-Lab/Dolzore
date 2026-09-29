using UnityEngine;

namespace Dolzore
{
    public sealed class TitleAtmosphere : MonoBehaviour
    {
        public RectTransform background;
        public CanvasGroup haze;
        private Vector2 origin;

        private void Start()
        {
            if (background != null)
                origin = background.anchoredPosition;
        }

        private void Update()
        {
            float t = Time.unscaledTime;
            if (background != null)
            {
                background.anchoredPosition = origin + new Vector2(
                    Mathf.Sin(t * 0.10f) * 5f,
                    Mathf.Cos(t * 0.075f) * 3f);
            }

            if (haze != null)
                haze.alpha = 0.18f + Mathf.Sin(t * 0.17f) * 0.035f;
        }
    }
}
