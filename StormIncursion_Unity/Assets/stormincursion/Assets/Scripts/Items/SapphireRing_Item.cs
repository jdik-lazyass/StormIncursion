using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace stormincursion
{
    public class SapphireRing_Item
    {
        public static ItemDef ItemDef;
        public static GameObject Effect;
        public static GameObject DisplayPrefab;
        public static ConfigEntry<float> HealMod;

        public static void Init(ItemDef itemDef, ConfigFile config, GameObject displayPrefab, GameObject effect)
        {
            ItemDef = itemDef;
            DisplayPrefab = displayPrefab;
            Effect = effect;

            HealMod = config.Bind<float>("Item: Sapphire Ring", "Heal mod Inc", 0.1f, "");

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
                    childName = "Finger22R",
                    localPos = new Vector3(0.02988F, -0.02095F, -0.01344F),
                    localAngles = new Vector3(337.977F, 215.9772F, 344.9543F),
                    localScale = new Vector3(0.05F, 0.05F, 0.05F)
                }
            });

            displayRules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Finger22R",
                    localPos = new Vector3(-0.00029F, 0.0307F, -0.00317F),
                    localAngles = new Vector3(2.29923F, 336.0044F, 162.7472F),
                    localScale = new Vector3(0.08F, 0.08F, 0.08F)
                }
            });

            displayRules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Finger22R",
                    localPos = new Vector3(-0.02323F, -0.03904F, 0.04649F),
                    localAngles = new Vector3(18.72854F, 29.37996F, 210.6245F),
                    localScale = new Vector3(0.05F, 0.05F, 0.05F)
                }
            });

            displayRules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Finger21R",
                    localPos = new Vector3(0.48847F, 1.10847F, 0.69358F),
                    localAngles = new Vector3(6.69701F, 172.1418F, 231.2393F),
                    localScale = new Vector3(1F, 1F, 1F)
                }
            });

            displayRules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Finger42R",
                    localPos = new Vector3(0.0854F, 0.02972F, 0.03842F),
                    localAngles = new Vector3(287.5135F, 139.0516F, 215.7942F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlEngiTurret", new ItemDisplayRule[]
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

            displayRules.Add("mdlEngiWalkerTurret", new ItemDisplayRule[]
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
                    childName = "Finger42R",
                    localPos = new Vector3(0.02734F, 0.0535F, -0.00262F),
                    localAngles = new Vector3(8.78994F, 135.2391F, 187.1553F),
                    localScale = new Vector3(0.02F, 0.02F, 0.02F)
                }
            });

            displayRules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Finger42R",
                    localPos = new Vector3(-0.08916F, 0.06863F, -0.04224F),
                    localAngles = new Vector3(4.03309F, 332.023F, 175.0864F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "FootFrontREnd",
                    localPos = new Vector3(-0.01523F, -0.13549F, 0.00226F),
                    localAngles = new Vector3(2.1712F, 80.21092F, 1.84608F),
                    localScale = new Vector3(0.2F, 0.2F, 0.2F)
                }
            });

            displayRules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(-0.21947F, 0.53289F, 0.02554F),
                    localAngles = new Vector3(23.84824F, 175.8003F, 149.9765F),
                    localScale = new Vector3(0.3F, 0.3F, 0.3F)
                }
            });

            displayRules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(0.24578F, 0.57622F, 0.00829F),
                    localAngles = new Vector3(339.3337F, 299.5092F, 7.12188F),
                    localScale = new Vector3(3F, 3F, 3F)
                }
            });

            displayRules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Finger42R",
                    localPos = new Vector3(-0.087F, 0.01669F, 0.02402F),
                    localAngles = new Vector3(333.815F, 0.60752F, 187.0098F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Finger3R",
                    localPos = new Vector3(0.00297F, 0.01669F, 0.00537F),
                    localAngles = new Vector3(16.04355F, 174.1973F, 188.3252F),
                    localScale = new Vector3(0.03F, 0.03F, 0.03F)
                }
            });

            displayRules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.06078F, 0.02271F, 0.13807F),
                    localAngles = new Vector3(33.11209F, 167.9209F, 278.5068F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(0.02525F, 0.11796F, 0.009F),
                    localAngles = new Vector3(24.35769F, 167.0482F, 6.86717F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(-0.0319F, -0.03089F, 0.0073F),
                    localAngles = new Vector3(350.6919F, 166.2763F, 14.90965F),
                    localScale = new Vector3(0.13F, 0.13F, 0.13F)
                }
            });

            displayRules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(-0.07691F, 0.08157F, -0.03427F),
                    localAngles = new Vector3(61.01277F, 183.4492F, 323.1057F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlDroneTech", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(0.15723F, -0.00792F, -0.00185F),
                    localAngles = new Vector3(12.36965F, 287.7682F, 290.5601F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlDrifter", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(-0.06933F, 0.02573F, -0.07776F),
                    localAngles = new Vector3(29.91208F, 47.84863F, 82.28661F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            ItemAPI.Add(new CustomItem(ItemDef, displayRules));
        }

        private static void Hooks()
        {
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);

            if (!NetworkServer.active) return;
            if (!self.inventory) return;

            int itemCount = self.inventory.GetItemCountEffective(ItemDef);
            if (itemCount <= 0) return;

            var tracker = self.GetComponent<SapphireAmulet_Tracker>();
            if (tracker == null)
            {
                tracker = self.gameObject.AddComponent<SapphireAmulet_Tracker>();
            }

            bool isRegeneratingShield = self.healthComponent.shield < self.maxShield && self.maxShield > 0 && self.healthComponent.shield > tracker.lastShieldValue;

            if (isRegeneratingShield && !tracker.wasRegeneratingShield)
            {
                DoBlast(self, itemCount);
            }

            tracker.wasRegeneratingShield = isRegeneratingShield;
            tracker.lastShieldValue = self.healthComponent.shield;
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender == null || sender.inventory == null)
            {
                return;
            }

            if (sender.inventory.GetItemCountEffective(ItemDef) >= 1)
            {
                var healthComponent = sender.healthComponent;
                if (healthComponent != null)
                {
                    args.baseShieldAdd += healthComponent.fullHealth * 0.04f;
                }
            }
        }

        private static void DoBlast(CharacterBody body, int itemCount)
        {
            float damage = body.damage * (1.5f + 0.5f * (itemCount - 1));
            float radius = 20f;

            BlastAttack blast = new BlastAttack
            {
                attacker = body.gameObject,
                inflictor = body.gameObject,
                teamIndex = body.teamComponent.teamIndex,
                position = body.corePosition,
                procCoefficient = 0f,
                radius = radius,
                baseForce = 200f,
                baseDamage = 0f,
                bonusForce = Vector3.up * 100f,
                crit = body.RollCrit(),
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.None,
            };

            BlastAttack.Result result = blast.Fire();
            int hitCount = result.hitCount;

            EffectManager.SpawnEffect(
                Effect,
                new EffectData
                {
                    origin = body.corePosition,
                    scale = radius,
                },
                true
            );

            

            if (hitCount > 0)
            {
                body.healthComponent.health += (hitCount * (body.maxHealth / 100)) * (1 + (itemCount - 1) / 10);
                body.healthComponent.health += (body.maxHealth / 100) * 2;
                body.healthComponent.shield += (hitCount * (body.maxShield / 100)) * (1 + (itemCount - 1) / 10);
            }
        }
    }

    public class SapphireAmulet_Tracker : MonoBehaviour
    {
        public bool wasRegeneratingShield = false;
        public float lastShieldValue = 0f;
    }
}