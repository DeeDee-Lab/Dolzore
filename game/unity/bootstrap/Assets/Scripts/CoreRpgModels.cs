using System;
using System.Collections.Generic;

namespace Dolzore
{
    [Serializable]
    public sealed class CharacterIdentityData
    {
        public string characterId = "player.local.review";
        public string displayName = "SORA";
        public string appearancePresetId = "appearance.sora.default";
    }

    [Serializable]
    public sealed class VocationStateData
    {
        public string primaryVocationId = "vocation.wanderer";
        public string secondaryVocationId = "";
        public int vocationLevel = 1;
    }

    [Serializable]
    public sealed class VitalStateData
    {
        public int level = 1;
        public int heartCurrent = 82;
        public int heartMax = 100;
        public int focusCurrent = 64;
        public int focusMax = 100;
    }

    [Serializable]
    public sealed class StatusEffectData
    {
        public string statusId;
        public float remainingSeconds;
        public int stacks = 1;
    }

    [Serializable]
    public sealed class InventoryItemData
    {
        public string itemInstanceId;
        public string itemDefinitionId;
        public int quantity = 1;
    }

    [Serializable]
    public sealed class EquipmentSlotData
    {
        public string slotId;
        public string itemInstanceId;
    }

    [Serializable]
    public sealed class LoadoutData
    {
        public string loadoutId;
        public string displayName;
        public List<EquipmentSlotData> slots = new List<EquipmentSlotData>();
    }

    [Serializable]
    public sealed class PlayerPersistentStateData
    {
        public CharacterIdentityData identity = new CharacterIdentityData();
        public VocationStateData vocation = new VocationStateData();
        public VitalStateData vitals = new VitalStateData();
        public List<StatusEffectData> statuses = new List<StatusEffectData>();
        public List<InventoryItemData> inventory = new List<InventoryItemData>();
        public List<LoadoutData> loadouts = new List<LoadoutData>();
        public string activeLoadoutId = "loadout.exploration.default";
        public string currentRegionId = "region.first_town.present";
        public string currentDistrictId = "district.first_town.central";
        public string targetEntityId = "";
    }

    public static class DolzoreIds
    {
        public const string FirstTownRegion = "region.first_town.present";
        public const string FirstTownHome = "entity.first_town.home";
        public const string FirstTownCafe = "entity.first_town.cafe_luma";
        public const string FirstTownBar = "entity.first_town.bar_13";
        public const string FirstTownMarket = "entity.first_town.market_hall";
        public const string FirstTownJournal = "entity.first_town.journal";
        public const string FirstTownCivic = "entity.first_town.civic_clock";
        public const string FirstTownWorkshop = "entity.first_town.workshop";
        public const string FirstTownRiverside = "entity.first_town.riverside_kiosk";
        public const string FirstTownStation = "entity.first_town.east_station";
        public const string FirstTownDepot = "entity.first_town.back_alley_depot";
        public const string FirstTownBridge = "entity.first_town.riverside_bridge";
    }
}
