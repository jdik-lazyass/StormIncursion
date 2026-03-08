using BepInEx;
using BepInEx.Configuration;
using JDContent.Scripts.Buffs;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace JDContent.Scripts.Items
{
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(R2API.R2API.PluginGUID)]

    public class SaveGoldForStage_Item : ItemBase<SaveGoldForStage_Item>
    {

        public override string ItemName => "Memorable Wallet";

        public override string ItemLangTokenName => "MEMORABLE_WALLET";

        public override string ItemPickupDesc => $"Saves money from purchases and gives it on the start of next stage.";

        public override string ItemFullDescription =>
            $"Whenever you make a gold purchase, stores <style=cIsDamage>10%</style> <style=cStack>(+ 10% per stack, hyperbolic)</style> of spent gold. Upon entering next stage, gain all the stored gold back.";

        public override string ItemLore => "<style=cEvent>//--TRANSCRIPT FROM UNIDENTIFIED ARTIFACT STORAGE--//</style>" +
            "\r\n\n\"MY TEETH BLEED.\"" +
            "\r\n\n\"BLEED WITH MORE CALCIUM.\"" +
            "\r\n\n\"GRANTING MORE PAIN AND TEETH.\"" +
            "\r\n\n<style=cEvent>//--END TRANSCRIPT--//</style>";

        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { };

        public override bool CanRemove => true;

        public override bool hidden => false;

        //Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion()
        //Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion()

        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            SetLogbookAppearance(.8f, 1.8f);
            CreateItem();
            Hooks();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            return rules;
        }

        public override void Hooks()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += PurchaseHook;
            Stage.onServerStageBegin += Stage_onServerStageBegin;
        }

        private void Stage_onServerStageBegin(Stage obj)
        {
            foreach (CharacterBody body in CharacterBody.readOnlyInstancesList)
            {
                var master = body.master;
                if (master == null || master.inventory == null)
                    continue;

                int storedGold =
                    master.inventory.GetItemCountPermanent(
                        SavedGoldInvis_Item.instance.ItemDef
                    );

                if (storedGold <= 0)
                    continue;

                master.GiveMoney((uint)storedGold);

                //master.inventory.RemoveItemPermanent(SavedGoldInvis_Item.instance.ItemDef,int.MaxValue);


            }
        }

        private void PurchaseHook(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator){
            orig(self, activator);

            if (self.costType != CostTypeIndex.Money)
                return;

            var body = activator.GetComponent<CharacterBody>();
            if (!body || !body.inventory)
                return;

            int walletStacks = body.inventory.GetItemCountPermanent(ItemDef);
            if (walletStacks <= 0)
                return;

            float percent = (0.12f * walletStacks / 0.10f * walletStacks + 1) * 100;
            int goldToStore = Mathf.FloorToInt((self.cost / 100) * percent);
            if (goldToStore <= 0)
                return;

            body.inventory.GiveItemPermanent(
                SavedGoldInvis_Item.instance.ItemDef,
                goldToStore
            );
        }

        public override void CreateConfig(ConfigFile config)
        {
            base.CreateConfig(config);
        }

    }

}
