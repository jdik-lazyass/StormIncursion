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

    public class SavedGoldInvis_Item : ItemBase<SavedGoldInvis_Item>
    {

        public override string ItemName => "SAVEDMONEY";

        public override string ItemLangTokenName => "SAVED_MONEY";

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
            
        }

        public override void CreateConfig(ConfigFile config)
        {
            base.CreateConfig(config);
        }

    }

}
