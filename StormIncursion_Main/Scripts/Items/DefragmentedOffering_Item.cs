using BepInEx;
using BepInEx.Configuration;
using IL.RoR2;
using JDContent.Scripts.Buffs;
using On.RoR2;
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

    public class DefragmentedOffering_Item : ItemBase<DefragmentedOffering_Item>
    {
        public ConfigEntry<float> BonusDamage;

        public override string ItemName => "Defragmented Offering";

        public override string ItemLangTokenName => "DEFRAGMENTEDOFFERING";

        public override string ItemPickupDesc => $"Heavy damage bursts freeze and deal bonus damage to enemies.";

        public override string ItemFullDescription =>
            $"<style=cIsDamage>Dealing more than 15% of max health</style> in less than a second <style=cIsUtility>freezes</style> enemy and deals <style=cIsDamage>80% base damage</style> <style=cStack>(+60% per stack)</style>. Has a cooldown of <style=cIsUtility>5 seconds</style>.";

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
            On.RoR2.CharacterBody.OnTakeDamageServer += CharacterBody_OnTakeDamageServer;
        }

        private void CharacterBody_OnTakeDamageServer(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, RoR2.CharacterBody self, RoR2.DamageReport report)
        {
            orig(self, report);
            if (!NetworkServer.active) return;
            if (report.attacker == null || report.attackerBody == null) return;
            if (report.victim == null || report.victimBody == null) return;
            if (report.victimBody.inventory == null) return;

            var attackerBody = report.attackerBody;
            if (!attackerBody.inventory) return;

            int itemCount = attackerBody.inventory.GetItemCountEffective(ItemDef);
            if (itemCount <= 0) return;

            var victimBody = report.victimBody;
            if (!victimBody.healthComponent) return;

            var tracker = victimBody.GetComponent<DefragmentedOffering_Tracker>();
            if (tracker == null) tracker = victimBody.gameObject.AddComponent<DefragmentedOffering_Tracker>();

            if (tracker.isOnCooldown) return;

            if (attackerBody.HasBuff(DefragmentedOffering_debuff.instance.BuffDef)) return;

            float maxHealth = victimBody.healthComponent.fullCombinedHealth;
            float threshold = maxHealth * 0.15f;

            tracker.accumulatedDamage += report.damageDealt;
            tracker.resetTimer = 1f;

            if (tracker.accumulatedDamage >= threshold)
            {
                tracker.accumulatedDamage = 0f;
                tracker.isOnCooldown = true;

                float bonusDamage = attackerBody.damage * (0.8f + BonusDamage.Value * (itemCount - 1));

                RoR2.SetStateOnHurt setStateOnHurt = victimBody.GetComponent<RoR2.SetStateOnHurt>();
                if (setStateOnHurt && setStateOnHurt.canBeFrozen)
                {
                    setStateOnHurt.SetFrozen(0.7f);
                }

                RoR2.DamageInfo damageInfo = new RoR2.DamageInfo
                {
                    attacker = attackerBody.gameObject,
                    damage = bonusDamage,
                    procCoefficient = 0f,
                    crit = false,
                    damageType = DamageType.Generic,
                };

                victimBody.healthComponent.TakeDamage(damageInfo);

                attackerBody.AddTimedBuff(DefragmentedOffering_debuff.instance.BuffDef, 5f);
            }
        }

        public override void CreateConfig(ConfigFile config)
        {
            //public ConfigEntry<float> LuckInc;
            //public ConfigEntry<int> LuckInc_MaxStack;
            BonusDamage = config.Bind<float>("Item: " + ItemName, "Luck Inc", 0.6f, "What should the player's crit chance be increased by?");
        }

    }

    public class DefragmentedOffering_Tracker : MonoBehaviour
    {
        public float accumulatedDamage = 0f;
        public float resetTimer = 0f;
        public bool isOnCooldown = false;
        private float cooldownTimer = 0f;

        private void FixedUpdate()
        {
            if (resetTimer > 0f)
            {
                resetTimer -= Time.fixedDeltaTime;
                if (resetTimer <= 0f)
                    accumulatedDamage = 0f;
            }

            if (isOnCooldown)
            {
                cooldownTimer += Time.fixedDeltaTime;
                if (cooldownTimer >= 5f)
                {
                    isOnCooldown = false;
                    cooldownTimer = 0f;
                }
            }
        }
    }

}
