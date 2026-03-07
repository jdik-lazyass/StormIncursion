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

    public class Keychain_Item : ItemBase<Keychain_Item>
    {
        public ConfigEntry<float> LuckInc;
        public ConfigEntry<int> LuckInc_MaxStack;

        public override string ItemName => "Keychain";

        public override string ItemLangTokenName => "KEYCHAIN";

        public override string ItemPickupDesc => $"Grants temporary luck on unlucky shots.";

        public override string ItemFullDescription =>
            $"Chance on hit to give luck buff <style=cStack>(chance increases per stack)</style> that stacks up to <style=cIsDamage>15 times</style> and grants <style=cIsUtility>+1 luck</style> on <style=cIsDamage>13 or more stacks</style>. Each buff stack grants a <style=cIsDamage>critical strike chance</style>. Buff stacks reset upon <style=cIsDamage>reaching 15</style>.";

        public override string ItemLore => "<style=cEvent>//--TRANSCRIPT FROM ROOM 8395 OF UES [Redacted]--//</style>" +
            "\r\n\n\"God, take this crap away already, how can noisy bunch of keys bring you faith?\"" +
            "\r\n\nLiam shouts at Elijah, packing up his own items into a cardboard box and preparing them for transfer." +
            "\r\n\nAnother day at Contact Light's most unpaid jobs, both Liam and Elijah being manual transferers, brings them closer to light form of depression." +
            "\r\n\n\"The only thing that's left from my home, to be exact. It was... evaporated, if in short.\"" +
            "\r\n\nLiam leaves room with hollow mind." +
            "\r\n\nElijah's mind sparked a bit when he understood that only after 3 years of cooperative work, Liam noticed his keychain." +
            "\r\n\n\"I bet you have some meaning relic with yourself. I believe all people have one.\"" +
            "\r\n\nElijah closes up their room after they both leave it. No further audio detected." +
            "\r\n\n<style=cEvent>//--END TRANSCRIPT--//</style>";

        public override ItemTier Tier => ItemTier.Tier2;

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
            On.RoR2.CharacterBody.OnTakeDamageServer += CharacterBody_OnTakeDamageServer;
        }

        private void CharacterBody_OnTakeDamageServer(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            orig(self, damageReport);

            if (damageReport == null) return;
            if (damageReport.attackerBody == null) return;

            int buffCount = damageReport.attackerBody.GetBuffCount(Keychain_Buff.instance.BuffDef);

            int stackCount = damageReport.attackerMaster.inventory.GetItemCountEffective(ItemDef);
            if (stackCount <= 0) return;

            float procChance = damageReport.damageInfo.procCoefficient * 100f;
            if (procChance <= 0) return;

            float divisor = 1f + stackCount * 0.1f;
            bool procSuccess = Util.CheckRoll(procChance - procChance / divisor, damageReport.attackerMaster);

            if (procSuccess)
            {
                if (buffCount < 15)
                {
                    damageReport.attackerBody.AddBuff(Keychain_Buff.instance.BuffDef);
                } else
                {
                    for (int i = 0; i < buffCount; i++)
                    {
                        damageReport.attackerBody.RemoveBuff(Keychain_Buff.instance.BuffDef);
                    }
                }
            }
            else
            {
                if (buffCount >= 15)
                {
                    for (int i = 0; i < buffCount; i++)
                    {
                        damageReport.attackerBody.RemoveBuff(Keychain_Buff.instance.BuffDef);
                    }
                }
                
            }
            
        }

        public override void CreateConfig(ConfigFile config)
        {
            //public ConfigEntry<float> LuckInc;
            //public ConfigEntry<int> LuckInc_MaxStack;
            LuckInc = config.Bind<float>("Item: " + ItemName, "Luck Inc", 0.01f, "What should the player's crit chance be increased by?");
            LuckInc_MaxStack = config.Bind<int>("Item: " + ItemName, "Luck Inc Max Stack", 5, "What should the player's crit chance be increased by?");
        }

    }

}
