using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace stormincursion
{
    public class NoHealingForCrits_Item
    {
        public static ItemDef ItemDef;
        public static ConfigEntry<float> CritChanceInc;
        public static ConfigEntry<float> CritDamageInc;
        public static ConfigEntry<float> RegenDisableTime;

        public static void Init(ItemDef itemDef, ConfigFile config)
        {
            ItemDef = itemDef;
            CritChanceInc = config.Bind<float>("Item: Cataclysmic Jawbreaker", "Crit Chance Increase", 12.5f, "");
            CritDamageInc = config.Bind<float>("Item: Cataclysmic Jawbreaker", "Crit Damage Increase", 12.5f, "");
            RegenDisableTime = config.Bind<float>("Item: Cataclysmic Jawbreaker", "Regen Disable Time", 0.5f, "");
            Hooks();
        }

        private static void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }

        private static void GlobalEventManager_onServerDamageDealt(DamageReport obj)
        {
            if (!obj.victim || !obj.victimBody) return;
            var victimCharacterBody = obj.victimBody;
            if (victimCharacterBody.inventory == null) return;

            var garbCount = victimCharacterBody.inventory.GetItemCountPermanent(ItemDef);
            if (garbCount > 0)
            {
                victimCharacterBody.AddTimedBuff(RoR2Content.Buffs.HealingDisabled, (float)garbCount / 2f);
            }
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender == null || !sender.isPlayerControlled) return;
            if (sender.inventory == null) return;

            int inventoryCount = sender.inventory.GetItemCountEffective(ItemDef);
            if (inventoryCount <= 0) return;

            args.critAdd += inventoryCount * CritChanceInc.Value * 3;
            args.critDamageMultAdd += inventoryCount * CritDamageInc.Value / 10 * 3;
        }
    }
}