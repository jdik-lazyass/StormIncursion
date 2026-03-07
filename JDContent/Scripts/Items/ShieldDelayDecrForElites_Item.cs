using BepInEx;
using BepInEx.Configuration;
using JDContent.Scripts.Buffs;
using R2API;
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

    public class ShieldDelayDecrForElites_Item : ItemBase<ShieldDelayDecrForElites_Item>
    {
        public ConfigEntry<float> ShieldRegenDelay;

        public override string ItemName => "Ancient Accumulator";

        public override string ItemLangTokenName => "ANCIENT_ACCUMULATOR";

        public override string ItemPickupDesc => $"Gives a buff that regenerates shields and increases armor upon killing an elite.";

        public override string ItemFullDescription =>
            $"Upon killing an enemy, replenish <style=cIsHealing>2% (5% if elite) of your max health as shields</style> <style=cStack>(+0.2 multiplier per stack)</style> " + 
            $"and recieve buff for <style=cIsUtility>2.5 seconds</style> " +
            $"<style=cStack>(+{ShieldRegenDelay.Value} second per stack)</style> that passively grants <style=cIsHealing>5% armor of your max health</style>. " + 
            $"Grants <style=cIsHealing>12% shield of max health</style> on first pickup.";

        public override string ItemLore => "<style=cEvent>//--TRANSCRIPT FROM UNIDENTIFIED ARTIFACT STORAGE--//</style>" +
            "\r\n\n\"MY TEETH BLEED.\"" +
            "\r\n\n\"BLEED WITH MORE CALCIUM.\"" +
            "\r\n\n\"GRANTING MORE PAIN AND TEETH.\"" +
            "\r\n\n<style=cEvent>//--END TRANSCRIPT--//</style>";

        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { };

        public override bool CanRemove => true;

        public override bool hidden => false;

        //Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
        //Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

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
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender == null)
            {
                return;
            }

            if (sender.inventory.GetItemCountEffective(ItemDef) >= 1)
            {
                args.baseShieldAdd += sender.GetComponent<HealthComponent>().fullHealth * 0.12f;
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {

            if (!report.attackerBody)
                return;

            var body = report.attackerBody;

            int itemCount = body.inventory.GetItemCountEffective(ItemDef);
            if (itemCount <= 0)
                return;

            float calculatedPercentage = body.maxHealth / 100f;
            float calculatedModifider = 1f + (0.2f * ((float)itemCount - 1f));

            if (report.victimIsElite)
            {
                body.healthComponent.RechargeShield((calculatedPercentage * 5f) * calculatedModifider);
            }
            else
            {
                body.healthComponent.RechargeShield((calculatedPercentage * 2f) * calculatedModifider);
            }

            body.AddTimedBuff(AncientAccumulatorBuff.instance.BuffDef, 2.5f + (ShieldRegenDelay.Value * ((float)itemCount - 1f)));
        }


        public override void CreateConfig(ConfigFile config)
        {
            ShieldRegenDelay = config.Bind<float>("Item: " + ItemName, "Timer Delay", 1f, "By what amount timer should increase when killing elite?");
        }

    }

}
