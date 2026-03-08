using BepInEx.Configuration;
using R2API;
using Rewired;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JDContent.Scripts.Items
{
    public class PermanentEnemyBuff_Item : ItemBase<PermanentEnemyBuff_Item>
    {
        public override string ItemName => "PERMASTORM_ITEM";

        public override string ItemLangTokenName => "PERMASTORMITEM";

        public override string ItemPickupDesc => $".";

        public override string ItemFullDescription =>
            $".";

        public override string ItemLore => "<style=cEvent>//--TRANSCRIPT FROM UNIDENTIFIED ARTIFACT STORAGE--//</style>" +
            "\r\n\n\"MY TEETH BLEED.\"" +
            "\r\n\n\"BLEED WITH MORE CALCIUM.\"" +
            "\r\n\n\"GRANTING MORE PAIN AND TEETH.\"" +
            "\r\n\n<style=cEvent>//--END TRANSCRIPT--//</style>";

        public override ItemTier Tier => ItemTier.NoTier;

        public override ItemTag[] ItemTags => new ItemTag[] { };

        public override bool CanRemove => true;

        public override bool hidden => true;

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender == null || sender.inventory == null) return;
            var ItemCount = sender.inventory.GetItemCountEffective(ItemDef);
            args.baseAttackSpeedAdd += ItemCount / 2.5f;
            args.primarySkill.cooldownFlatReduction += (float)Math.Pow(0.25f * ItemCount, 1.1);
            args.secondarySkill.cooldownFlatReduction += (float)Math.Pow(0.75f * ItemCount, 0.7);
            args.utilitySkill.cooldownFlatReduction += (float)Math.Pow(0.5f * ItemCount, 0.9);
            args.specialSkill.cooldownFlatReduction += (float)Math.Pow(ItemCount, 0.8f);
        }

        public override void CreateConfig(ConfigFile config)
        {
            base.CreateConfig(config);
        }
    }
}
