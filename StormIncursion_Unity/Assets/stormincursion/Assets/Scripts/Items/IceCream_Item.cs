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

            SetupDisplayRules();
            Hooks();
        }

        private static void SetupDisplayRules()
        {
            ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict();

            displayRules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HeadCenter",
                    localPos = new Vector3(0.08829F, -0.0975F, 0.22369F),
                    localAngles = new Vector3(58.07353F, 290.6651F, 91.21642F),
                    localScale = new Vector3(0.15083F, 0.14789F, 0.1384F)
                }
            });

            displayRules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HeadCenter",
                    localPos = new Vector3(0.08947F, -0.10576F, 0.13781F),
                    localAngles = new Vector3(62.02094F, 280.4277F, 42.50446F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(0.04876F, -0.03571F, 0.14836F),
                    localAngles = new Vector3(66.21909F, 174.2497F, 326.6435F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Chest",
                    localPos = new Vector3(0.72464F, 1.58983F, 2.42929F),
                    localAngles = new Vector3(76.99681F, 63.85303F, 47.08825F),
                    localScale = new Vector3(1F, 1F, 1F)
                }
            });

            displayRules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.08732F, -0.08337F, 0.19637F),
                    localAngles = new Vector3(53.15427F, 201.1938F, 49.45714F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.05961F, -0.04141F, 0.14436F),
                    localAngles = new Vector3(59.7324F, 173.0528F, 23.55104F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.07125F, 0.02026F, 0.19109F),
                    localAngles = new Vector3(70.75821F, 205.8668F, 53.34343F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "PlatformBase",
                    localPos = new Vector3(0.09956F, -0.62205F, 0.47659F),
                    localAngles = new Vector3(27.46745F, 186.9706F, 345.0695F),
                    localScale = new Vector3(0.3F, 0.3F, 0.3F)
                }
            });

            displayRules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.04794F, -0.12204F, 0.18907F),
                    localAngles = new Vector3(63.11122F, 198.9952F, 38.56545F),
                    localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });

            displayRules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HeadCenter",
                    localPos = new Vector3(-1.32146F, 3.74112F, -0.45552F),
                    localAngles = new Vector3(342.3329F, 168.6139F, 133.2572F),
                    localScale = new Vector3(1.5F, 1.5F, 1.5F)
                }
            });

            displayRules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(0.0843F, -0.01889F, 0.15712F),
                    localAngles = new Vector3(58.03229F, 195.7653F, 329.7492F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.06539F, -0.00391F, 0.11305F),
                    localAngles = new Vector3(69.37233F, 123.9522F, 2.10089F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(0.0373F, 0.08235F, 0.20011F),
                    localAngles = new Vector3(67.80087F, 329.6857F, 135.3359F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(0.06265F, -0.00475F, 0.15467F),
                    localAngles = new Vector3(54.27256F, 277.8099F, 61.19162F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(0.15236F, 0.04069F, 0.25227F),
                    localAngles = new Vector3(62.90203F, 200.8919F, 342.4941F),
                    localScale = new Vector3(0.2F, 0.2F, 0.2F)
                }
            });

            displayRules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.03463F, 0.19023F, -0.09039F),
                    localAngles = new Vector3(34.96603F, 88.9302F, 141.5371F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlDroneTech", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.06166F, -0.21606F, 0.11747F),
                    localAngles = new Vector3(353.7832F, 117.948F, 314.3687F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlDrifter", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(0.01997F, 0.21986F, -0.10706F),
                    localAngles = new Vector3(21.74954F, 67.9724F, 116.428F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            ItemAPI.Add(new CustomItem(ItemDef, displayRules));
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