using BepInEx.Configuration;
using BepInEx.Logging;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class MemorableWallet_item
    {
        public static ItemDef ItemDef;
        public static GameObject DisplayPrefab;
        public static ConfigEntry<float> perStack;
        public static ConfigEntry<float> maxVal;

        public static void Init(ItemDef itemDef, ConfigFile config, GameObject displayPrefab)
        {
            ItemDef = itemDef;
            DisplayPrefab = displayPrefab;

            perStack = config.Bind<float>("Item: Wallet", "Percent Per Stack", 10f, "");
            maxVal = config.Bind<float>("Item: Wallet", "Modifier Max Value", 30f, "");

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
                    localPos = new Vector3(-0.26282F, -0.38102F, 1.48507F),
                    localAngles = new Vector3(26.58615F, 24.69656F, 220.1612F),
                    localScale = new Vector3(0.5F, 0.5F, 0.5F)
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
            On.RoR2.PurchaseInteraction.OnInteractionBegin += PurchaseHook;
            Stage.onStageStartGlobal += Stage_onServerStageBegin;
        }

        private static void Stage_onServerStageBegin(Stage obj)
        {
            foreach (PlayerCharacterMasterController controller in PlayerCharacterMasterController.instances)
            {
                var master = controller.master;
                if (master == null || master.inventory == null)
                    continue;

                int storedGold = master.inventory.GetItemCountPermanent(WalletCount_Item.ItemDef);
                stormincursionMain.logger.LogInfo($"Body: {master.name}, storedGold: {storedGold}");

                if (storedGold <= 0)
                    continue;

                master.GiveMoney((uint)storedGold);
                master.inventory.RemoveItemPermanent(WalletCount_Item.ItemDef, storedGold / 2);
                stormincursionMain.logger.LogInfo("Gave money: " + storedGold);

            }
        }

        private static void PurchaseHook(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            orig(self, activator);

            if (self.costType != CostTypeIndex.Money)
                return;

            var body = activator.GetComponent<CharacterBody>();
            if (!body || !body.inventory)
                return;

            int walletStacks = body.inventory.GetItemCountPermanent(ItemDef);
            if (walletStacks <= 0)
                return;

            MathHelper help = new MathHelper();
            float percent = help.HyperbolicScaling(walletStacks, perStack.Value, maxVal.Value) * 100;
            stormincursionMain.logger.LogInfo(percent);

            int goldToStore = Mathf.FloorToInt((self.cost / 100f) * percent);
            stormincursionMain.logger.LogInfo(goldToStore);

            if (goldToStore <= 0)
                return;

            body.inventory.GiveItemPermanent(
                WalletCount_Item.ItemDef,
                goldToStore
            );
        }
    }
}