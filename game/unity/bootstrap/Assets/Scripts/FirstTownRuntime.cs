using UnityEngine;
using UnityEngine.UI;

namespace Dolzore
{
    public sealed class FirstTownRuntime : MonoBehaviour
    {
        public DolzorePlayerController player;
        public PlayerStateComponent playerState;
        public WorldStateService worldState;

        public Text playerNameLabel;
        public Text metaLabel;
        public Text districtLabel;
        public Text statusTargetLabel;
        public Text interactionLabel;
        public RectTransform minimapMarker;
        public RectTransform minimapRect;

        public Transform[] landmarks;
        public string[] landmarkNames;
        public EntityIdentity[] landmarkEntities;

        private int nearbyLandmark = -1;

        private void Start()
        {
            if (player != null)
                player.InteractPressed += HandleInteract;

            if (playerState != null)
                playerState.ConfigureForFirstTown();

            if (worldState != null)
                worldState.EnterSpace(DolzoreIds.FirstTownRegion, "FIRST TOWN", WorldSpaceKind.PublicRegion);

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
            DistrictInfo district = ResolveDistrict(p);

            if (playerState != null)
                playerState.SetDistrict(district.id);

            PlayerPersistentStateData model = playerState != null ? playerState.State : null;

            if (playerNameLabel != null)
                playerNameLabel.text = model != null ? model.identity.displayName : "SORA";

            if (metaLabel != null)
            {
                int level = model != null ? model.vitals.level : 1;
                string vocation = model != null ? DisplayVocation(model.vocation.primaryVocationId) : "WANDERER";
                metaLabel.text = "LV " + level.ToString("00") + "   VOCATION " + vocation;
            }

            if (districtLabel != null)
                districtLabel.text = district.displayName.ToUpperInvariant() + "  //  PRESENT";

            nearbyLandmark = FindNearestLandmark(p, 2.2f);
            string targetId = "";
            string targetName = "--";

            if (nearbyLandmark >= 0)
            {
                targetName = landmarkNames[nearbyLandmark];
                if (landmarkEntities != null && nearbyLandmark < landmarkEntities.Length && landmarkEntities[nearbyLandmark] != null)
                    targetId = landmarkEntities[nearbyLandmark].EntityId;
            }

            if (playerState != null)
                playerState.SetTarget(targetId);

            if (statusTargetLabel != null)
            {
                int statusCount = model != null && model.statuses != null ? model.statuses.Count : 0;
                statusTargetLabel.text = "STATUS " + (statusCount == 0 ? "CLEAR" : statusCount.ToString()) + "   TARGET " + targetName;
            }

            if (interactionLabel != null)
            {
                interactionLabel.text = nearbyLandmark >= 0
                    ? "ENTER   " + targetName
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
            interactionLabel.text = landmarkNames[nearbyLandmark] + "  //  INTERIOR BOUNDARY RESERVED";
        }

        private static string DisplayVocation(string vocationId)
        {
            if (string.IsNullOrEmpty(vocationId)) return "NONE";
            int split = vocationId.LastIndexOf('.');
            return (split >= 0 ? vocationId.Substring(split + 1) : vocationId).Replace('_', ' ').ToUpperInvariant();
        }

        private static DistrictInfo ResolveDistrict(Vector2 p)
        {
            if (p.y > 8f) return new DistrictInfo("district.first_town.residential", "Residential Hill");
            if (p.y > 2f && p.x < -5f) return new DistrictInfo("district.first_town.market", "Market / Workshop");
            if (p.y > 2f && p.x > 5f) return new DistrictInfo("district.first_town.civic", "Civic / Journal");
            if (p.y > 2f) return new DistrictInfo("district.first_town.central", "Central Main Street");
            if (p.y > -7f) return new DistrictInfo("district.first_town.riverside", "Riverside");
            if (p.x > 7f) return new DistrictInfo("district.first_town.station", "Station / East Gate");
            if (p.x < -7f) return new DistrictInfo("district.first_town.back_alley", "Back Alley");
            return new DistrictInfo("district.first_town.south_walk", "South Walk");
        }

        private readonly struct DistrictInfo
        {
            public readonly string id;
            public readonly string displayName;

            public DistrictInfo(string id, string displayName)
            {
                this.id = id;
                this.displayName = displayName;
            }
        }
    }
}
