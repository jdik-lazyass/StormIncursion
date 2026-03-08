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

    public class SystemCalculator_Item : ItemBase<SystemCalculator_Item>
    {
        public ConfigEntry<float> BuffLength;

        public override string ItemName => "System Calculator";

        public override string ItemLangTokenName => "SYSTEM_CALCULATOR";

        public override string ItemPickupDesc => $"Reduces skills cooldowns when spending money.";

        public override string ItemFullDescription =>
            $"Every <style=cIsDamage>15$ gold</style> spent <style=cStack>(scales with time)</style> " +
            $"grants a buff for 8 <style=cStack>(+{BuffLength.Value * 1} seconds per stack)</style> seconds that reduces Utility and Special skills cooldown.";

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
        }

        private void PurchaseHook(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            orig(self, activator);

            if (self.costType != CostTypeIndex.Money)
                return;

            var body = activator.GetComponent<CharacterBody>();
            if (!body || !body.inventory)
                return;

            if (body.inventory.GetItemCountPermanent(ItemDef) == 0) return;

            if (self.cost >= Run.instance.GetDifficultyScaledCost(15)){
                body.AddTimedBuff(Keychain_Buff.instance.BuffDef, body.inventory.GetItemCountEffective(SystemCalculator_Item.instance.ItemDef) * 2 + 3, 1);
            };
            
        }

        public override void CreateConfig(ConfigFile config)
        {
            BuffLength = config.Bind<float>("Item: " + ItemName, "Crit Chance Increase", 2f, "What should the player's crit chance be increased by?");
        }

    }

}
