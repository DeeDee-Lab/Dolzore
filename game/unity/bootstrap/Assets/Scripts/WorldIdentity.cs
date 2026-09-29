using System;
using UnityEngine;

namespace Dolzore
{
    public enum WorldSpaceKind
    {
        PublicRegion = 0,
        Interior = 1,
        PrivateInstance = 2
    }

    [Serializable]
    public sealed class RegionStateData
    {
        public string regionId = DolzoreIds.FirstTownRegion;
        public string displayName = "FIRST TOWN";
        public WorldSpaceKind spaceKind = WorldSpaceKind.PublicRegion;
        public string parentRegionId = "";
        public string instanceId = "";
    }

    public sealed class EntityIdentity : MonoBehaviour
    {
        [SerializeField] private string entityId;
        [SerializeField] private string regionId = DolzoreIds.FirstTownRegion;
        [SerializeField] private string archetypeId;

        public string EntityId => entityId;
        public string RegionId => regionId;
        public string ArchetypeId => archetypeId;

        public void Configure(string stableEntityId, string stableRegionId, string stableArchetypeId)
        {
            entityId = stableEntityId;
            regionId = stableRegionId;
            archetypeId = stableArchetypeId;
        }
    }

    public sealed class PlayerStateComponent : MonoBehaviour
    {
        [SerializeField] private PlayerPersistentStateData state = new PlayerPersistentStateData();

        public PlayerPersistentStateData State => state;

        public void ConfigureForFirstTown()
        {
            state.currentRegionId = DolzoreIds.FirstTownRegion;
            if (string.IsNullOrEmpty(state.currentDistrictId))
                state.currentDistrictId = "district.first_town.central";
        }

        public void SetDistrict(string districtId)
        {
            state.currentDistrictId = districtId;
        }

        public void SetTarget(string entityId)
        {
            state.targetEntityId = entityId ?? "";
        }
    }

    public sealed class WorldStateService : MonoBehaviour
    {
        [SerializeField] private RegionStateData currentRegion = new RegionStateData();
        [SerializeField] private double worldSeconds;

        public RegionStateData CurrentRegion => currentRegion;
        public double WorldSeconds => worldSeconds;

        private void Update()
        {
            worldSeconds += Time.unscaledDeltaTime;
        }

        public void EnterSpace(string regionId, string displayName, WorldSpaceKind kind, string parentRegionId = "", string instanceId = "")
        {
            currentRegion.regionId = regionId;
            currentRegion.displayName = displayName;
            currentRegion.spaceKind = kind;
            currentRegion.parentRegionId = parentRegionId;
            currentRegion.instanceId = instanceId;
        }
    }
}
