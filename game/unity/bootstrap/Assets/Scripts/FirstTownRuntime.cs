using UnityEngine;
using UnityEngine.UI;

namespace Dolzore
{
    public sealed class FirstTownRuntime : MonoBehaviour
    {
        public DolzorePlayerController player;
        public Text districtLabel;
        public Text interactionLabel;
        public RectTransform minimapMarker;
        public RectTransform minimapRect;
        public Transform[] landmarks;
        public string[] landmarkNames;

        private int nearbyLandmark = -1;

        private void Start()
        {
            if (player != null)
                player.InteractPressed += HandleInteract;
            UpdateUi();
        }

        private void OnDestroy()
        {
            if (player != null)
                player.InteractPressed -= HandleInteract;
        }

        private void Update()
        {
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (player == null) return;
            Vector2 p = player.WorldPosition;

            if (districtLabel != null)
                districtLabel.text = ResolveDistrict(p).ToUpperInvariant() + "  //  PRESENT";

            nearbyLandmark = FindNearestLandmark(p, 2.2f);
            if (interactionLabel != null)
            {
                interactionLabel.text = nearbyLandmark >= 0
                    ? "ENTER   " + landmarkNames[nearbyLandmark]
                    : "EXPLORE   FIRST TOWN";
            }

            if (minimapMarker != null && minimapRect != null)
            {
                float nx = Mathf.InverseLerp(-24f, 24f, p.x);
                float ny = Mathf.InverseLerp(-17f, 17f, p.y);
                Rect r = minimapRect.rect;
                minimapMarker.anchoredPosition = new Vector2(
                    Mathf.Lerp(r.xMin + 8f, r.xMax - 8f, nx),
                    Mathf.Lerp(r.yMin + 8f, r.yMax - 8f, ny));
            }
        }

        private int FindNearestLandmark(Vector2 p, float radius)
        {
            if (landmarks == null || landmarkNames == null) return -1;
            int count = Mathf.Min(landmarks.Length, landmarkNames.Length);
            float best = radius * radius;
            int index = -1;
            for (int i = 0; i < count; i++)
            {
                if (landmarks[i] == null) continue;
                float d = ((Vector2)landmarks[i].position - p).sqrMagnitude;
                if (d <= best)
                {
                    best = d;
                    index = i;
                }
            }
            return index;
        }

        private void HandleInteract()
        {
            if (interactionLabel == null || nearbyLandmark < 0) return;
            interactionLabel.text = landmarkNames[nearbyLandmark] + "  //  ACCESS LOCKED FOR REVIEW SLICE";
        }

        private static string ResolveDistrict(Vector2 p)
        {
            if (p.y > 8f) return "Residential Hill";
            if (p.y > 2f && p.x < -5f) return "Market / Workshop";
            if (p.y > 2f && p.x > 5f) return "Civic / Journal";
            if (p.y > 2f) return "Central Main Street";
            if (p.y > -7f) return "Riverside";
            if (p.x > 7f) return "Station / East Gate";
            if (p.x < -7f) return "Back Alley";
            return "South Walk";
        }
    }
}
