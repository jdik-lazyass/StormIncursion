using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class IceCream_Item
    {
        public static ItemDef ItemDef;
        public static GameObject DisplayPrefab;
        public static ConfigEntry<float> FreezeTime;
        public static ConfigEntry<float> CooldownTime;
        public static ConfigEntry<float> FreezeTimePerStack;

        public static void Init(ItemDef itemDef, ConfigFile config, GameObject displayPrefab)
        {
            ItemDef = itemDef;
            DisplayPrefab = displayPrefab;

            FreezeTime = config.Bind<float>("Item: IceCream", "Freeze Mod", 2f, "");
            CooldownTime = config.Bind<float>("Item: IceCream", "Cooldown Time", 8f, "");
            FreezeTimePerStack = config.Bind<float>("Item: IceCream", "Freeze Time Per Stack", 0.25f, "");

            Hooks();
        }

        private static void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (damageInfo == null || victim == null) return;
            if (damageInfo.attacker == null) return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            if (attackerBody == null) return;
            if (attackerBody.master?.inventory == null) return;

            int stackCount = attackerBody.master.inventory.GetItemCountEffective(ItemDef);
            if (stackCount <= 0) return;

            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (victimBody == null) return;

            SetStateOnHurt hurtState = victim.GetComponent<SetStateOnHurt>();
            if (hurtState == null) return;

            bool alreadyStunned = hurtState.targetStateMachine?.state is EntityStates.StunState;
            if (!alreadyStunned) return;

            EntityStates.StunState stunState = hurtState.targetStateMachine?.state as EntityStates.StunState;
            if (stunState == null) return;

            var durationField = typeof(EntityStates.StunState).GetField("duration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            float duration = durationField != null ? (float)durationField.GetValue(stunState) : 1f;

            var fixedAgeField = typeof(EntityStates.EntityState).GetField("fixedAge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            float fixedAge = fixedAgeField != null ? (float)fixedAgeField.GetValue(stunState) : 0f;

            float remaining = duration - fixedAge;
            if (remaining <= 0f) return;

            if (attackerBody.HasBuff(IcecreamCooldown_buff.BuffDef)) return;
            if (!hurtState.canBeFrozen) return;

            RoR2.SetStateOnHurt setStateOnHurt = victimBody.GetComponent<RoR2.SetStateOnHurt>();
            if (setStateOnHurt && setStateOnHurt.canBeFrozen)
            {
                var extraStacks = stackCount - 1;
                setStateOnHurt.SetFrozen(remaining * FreezeTime.Value + (FreezeTimePerStack.Value * extraStacks));
            }

            attackerBody.AddTimedBuff(IcecreamCooldown_buff.BuffDef, CooldownTime.Value);
        }
    }
}