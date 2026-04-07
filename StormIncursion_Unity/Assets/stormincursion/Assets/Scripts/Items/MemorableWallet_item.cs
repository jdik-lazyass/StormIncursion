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

            Hooks();
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