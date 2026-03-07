using BepInEx;
using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JDContent.Scripts.Items
{
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(R2API.R2API.PluginGUID)]


    public class NoHealingForCrits_Item : ItemBase<NoHealingForCrits_Item>
    {
        public ConfigEntry<float> CritChanceInc;
        public ConfigEntry<float> CritDamageInc;
        public ConfigEntry<float> RegenDisableTime;

        public override string ItemName => "Cataclysmic Jawbreaker";

        public override string ItemLangTokenName => "CATACLYSMIC_JAWBREAKER";

        public override string ItemPickupDesc => $"Increase critical strike chance and critical strike damage. <style=cIsHealth>Recieving damage disables all healing for a time...</style>";

        public override string ItemFullDescription =>
            $"Increase <style=cIsDamage>critical strike chance</style> by " +
            $"<style=cIsDamage>{CritChanceInc.Value * 3}%</style> " +
            $"<style=cStack>(+{CritChanceInc.Value * 3}% per stack)</style>, and critical strike damage by " +
            $"<style=cIsDamage>{CritDamageInc.Value * 3}%</style> " +
            $"<style=cStack>(+{CritDamageInc.Value * 3}% per stack)</style>." +
            $" Recieving damage <style=cIsHealth>stops all healing</style> for " +
            $"<style=cIsHealth>{RegenDisableTime.Value} seconds</style> " +
            $"<style=cStack>(+{RegenDisableTime.Value} seconds per stack)</style>.";

        public override string ItemLore => "<style=cEvent>//--TRANSCRIPT FROM UNIDENTIFIED ARTIFACT STORAGE--//</style>" +
            "\r\n\n\"MY TEETH BLEED.\"" +
            "\r\n\n\"BLEED WITH MORE CALCIUM.\"" +
            "\r\n\n\"GRANTING MORE PAIN AND TEETH.\"" +
            "\r\n\n<style=cEvent>//--END TRANSCRIPT--//</style>";

        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[] { };

        public override bool CanRemove => true;

        public override bool hidden => false;

        //Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion()
        //Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion()

        public override GameObject ItemModel => JDContentPlugin.MainAssets.LoadAsset<GameObject>("Assets/Models/Prefabs/Item/JawBreaker/Jawbreaker_mesh.prefab");

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport obj)
        {
            if (!obj.victim || !obj.victimBody)
            {
                return;
            }

            var victimCharacterBody = obj.victimBody;
            if (victimCharacterBody == null) return;

            // We need an inventory to do check for our item
            if (victimCharacterBody.inventory == null) return;
            if (victimCharacterBody.inventory.GetItemCountPermanent(NoHealingForCrits_Item.instance.ItemDef) > 0)
            {
                // Store the amount of our item we have
                var garbCount = victimCharacterBody.inventory.GetItemCountPermanent(NoHealingForCrits_Item.instance.ItemDef);

                if (garbCount > 0)
                {
                    // Since we passed all checks, we now give our attacker the cloaked buff.
                    // Note how we are scaling the buff duration depending on the number of the custom item in our inventory.
                    victimCharacterBody.AddTimedBuff(RoR2Content.Buffs.HealingDisabled, (float)garbCount / 2f);
                }
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender == null)
            {
                return;
            }

            if (sender.isPlayerControlled)
            {
                int inventoryCount = GetCount(sender);

                if (sender.level <= 0 || inventoryCount <= 0)
                {
                    return;
                }

                float newCritChance = inventoryCount * CritChanceInc.Value * 3;
                args.critAdd = newCritChance;

                float newCritDamage = inventoryCount * CritDamageInc.Value / 10 * 3;
                args.critDamageMultAdd = newCritDamage;
            }
        }

        public override void CreateConfig(ConfigFile config)
        {
            CritChanceInc = config.Bind<float>("Item: " + ItemName, "Crit Chance Increase", 12.5f, "What should the player's crit chance be increased by?");
            CritDamageInc = config.Bind<float>("Item: " + ItemName, "Crit Damage Increase", 12.5f, "What should the player's crit damage be increased by?");
            RegenDisableTime = config.Bind<float>("Item: " + ItemName, "Amount of time with disabled Health Regeneration", 0.5f, "For how long health regeneration should be disabled after being hit?");
        }

    }

}
