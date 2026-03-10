using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class Keychain_Item
    {
        public static ItemDef ItemDef;
        public static GameObject DisplayPrefab;
        public static ConfigEntry<float> LuckInc;
        public static ConfigEntry<int> LuckInc_MaxStack;

        public static void Init(ItemDef itemDef, ConfigFile config, GameObject displayPrefab)
        {
            ItemDef = itemDef;
            DisplayPrefab = displayPrefab;

            LuckInc = config.Bind<float>("Item: Keychain", "Luck Inc", 0.01f, "");
            LuckInc_MaxStack = config.Bind<int>("Item: Keychain", "Luck Inc Max Stack", 5, "");

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
                    childName = "Pelvis",
                    localPos = new Vector3(0.14396F, -0.03933F, -0.19248F),
                    localAngles = new Vector3(330.3721F, 324.9917F, 176.7163F),
                    localScale = new Vector3(0.15083F, 0.14789F, 0.1384F)
                }
            });

            displayRules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.15941F, -0.0324F, 0.01423F),
                    localAngles = new Vector3(45.73383F, 121.9203F, 203.6731F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.15175F, 0.04859F, -0.12149F),
                    localAngles = new Vector3(24.66067F, 156.4204F, 184.2085F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)

                }
            });

            displayRules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Hip",
                    localPos = new Vector3(1.14234F, 2.97885F, 0.99265F),
                    localAngles = new Vector3(12.52436F, 181.5724F, 184.6927F),
                    localScale = new Vector3(1F, 1F, 1F)
                }
            });

            displayRules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.29391F, 0.11272F, 0.01421F),
                    localAngles = new Vector3(349.0753F, 78.62788F, 175.2363F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.2294F, -0.04192F, -0.08263F),
                    localAngles = new Vector3(356.3836F, 200.2574F, 215.6173F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.23394F, 0.04239F, -0.08164F),
                    localAngles = new Vector3(22.70468F, 207.8264F, 222.9708F),
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
                    localPos = new Vector3(0.09744F, 0.06356F, 0.47667F),
                    localAngles = new Vector3(71.08884F, 94.37516F, 0.27166F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.28263F, -0.0222F, 0.18861F),
                    localAngles = new Vector3(359.8702F, 190.9554F, 178.3042F),
                    localScale = new Vector3(0.20234F, 0.2F, 0.2F)
                }
            });

            displayRules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "HandR",
                    localPos = new Vector3(-0.30746F, -0.39923F, 1.37383F),
                    localAngles = new Vector3(1.14964F, 44.07939F, 224.1886F),
                    localScale = new Vector3(0.5F, 0.5F, 0.5F)
                }
            });

            displayRules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.09129F, -0.03002F, -0.18635F),
                    localAngles = new Vector3(17.05484F, 192.1197F, 180.2232F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.17902F, 0.12941F, -0.0086F),
                    localAngles = new Vector3(45.73383F, 121.9203F, 203.6731F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.24916F, -0.14349F, 0.13096F),
                    localAngles = new Vector3(24.02735F, 187.3164F, 242.598F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.22621F, 0.06674F, 0.07456F),
                    localAngles = new Vector3(42.7019F, 308.1681F, 52.06086F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.15496F, 0.31148F, 0.17763F),
                    localAngles = new Vector3(359.2818F, 165.2045F, 22.43329F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.02933F, 0.20975F, -0.17579F),
                    localAngles = new Vector3(68.71137F, 189.0023F, 281.6643F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlDroneTech", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.20716F, 0.06424F, 0.00639F),
                    localAngles = new Vector3(45.66182F, 266.3139F, 305.0764F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            displayRules.Add("mdlDrifter", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = DisplayPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(0.03682F, 0.12451F, -0.42984F),
                    localAngles = new Vector3(29.91208F, 47.84864F, 82.28661F),
                    localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });

            ItemAPI.Add(new CustomItem(ItemDef, displayRules));
        }

        private static void Hooks()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += PurchaseInteraction_OnInteractionBegin;
            On.RoR2.CharacterBody.Start += CharacterBody_Start;
        }

        private static void PurchaseInteraction_OnInteractionBegin(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            orig(self, activator);

            if (self.GetComponent<ChestBehavior>() == null && self.GetComponent<MultiShopController>() == null) return;

            if (self.costType != CostTypeIndex.Money) return;
            if (self.cost <= 0) return;

            CharacterBody body = activator.GetComponent<CharacterBody>();
            if (body == null) return;
            if (body.master?.inventory == null) return;

            int stackCount = body.master.inventory.GetItemCountEffective(ItemDef);
            if (stackCount <= 0) return;

            int buffCount = body.inventory.GetItemCountEffective(KeychainInvis_Item.ItemDef);
            if (buffCount < 15)
            {
                body.inventory.GiveItemPermanent(KeychainInvis_Item.ItemDef);
                buffCount = body.inventory.GetItemCountEffective(KeychainInvis_Item.ItemDef);
            }
            else
            {
                body.inventory.RemoveItemPermanent(KeychainInvis_Item.ItemDef, int.MaxValue);
                buffCount = body.inventory.GetItemCountEffective(KeychainInvis_Item.ItemDef);
            }
                

            body.SetBuffCount(KeychainBuff.BuffDef.buffIndex, buffCount);
        }

        private static void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            orig(self);

            if (self.master?.inventory == null) return;
            if (self.master.inventory.GetItemCountEffective(ItemDef) <= 0) return;

            int savedCount = self.master.inventory.GetItemCountEffective(KeychainInvis_Item.ItemDef);
            if (savedCount <= 0) return;

            self.SetBuffCount(KeychainBuff.BuffDef.buffIndex, savedCount);
        }
    }
}