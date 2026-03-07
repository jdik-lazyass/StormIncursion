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

    public class SaphireAmulet_item : ItemBase<SaphireAmulet_item>
    {
        public ConfigEntry<float> DMGMod;
        public ConfigEntry<float> Radius;

        public override string ItemName => "Sapphire Amulet";

        public override string ItemLangTokenName => "SAPPHIRE_AMULET";

        public override string ItemPickupDesc => $"When shield regeneration begins, emit a healing explosion.";

        public override string ItemFullDescription =>
            $"When shield regeneration starts, " +
            $"emits a <style=cIsDamage>20m explosion</style> that heals <style=cIsHealing>1% of max health and shields </style><style=cStack>(+0.1 multiplier per stack)</style> for each enemy hit, and an additional <style=cIsHealing>2% of max health</style>. Grants <style=cIsHealing>4% shields</style> on first pickup.";

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
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
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
                args.baseShieldAdd += sender.GetComponent<HealthComponent>().fullHealth * 0.04f;
            }
        }

        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);

            if (!NetworkServer.active) return;
            if (!self.inventory) return;

            int itemCount = self.inventory.GetItemCountEffective(ItemDef);
            if (itemCount <= 0) return;

            var tracker = self.GetComponent<SapphireAmulet_Tracker>();
            if (tracker == null)
            {
                tracker = self.gameObject.AddComponent<SapphireAmulet_Tracker>();
            }

            bool isRegeneratingShield = self.healthComponent.shield < self.maxShield && self.maxShield > 0 && self.healthComponent.shield > tracker.lastShieldValue;

            if (isRegeneratingShield && !tracker.wasRegeneratingShield)
            {
                DoBlast(self, itemCount);
            }

            tracker.wasRegeneratingShield = isRegeneratingShield;
            tracker.lastShieldValue = self.healthComponent.shield;
        }

        private void DoBlast(CharacterBody body, int itemCount)
        {
            float damage = body.damage * (1.5f + 0.5f * (itemCount - 1));
            float radius = 20f;

            BlastAttack blast = new BlastAttack
            {
                attacker = body.gameObject,
                inflictor = body.gameObject,
                teamIndex = body.teamComponent.teamIndex,
                position = body.transform.position,
                procCoefficient = 0f,
                radius = radius,
                baseForce = 200f,
                baseDamage = 0f,
                bonusForce = Vector3.up * 100f,
                crit = body.RollCrit(),
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.None,
            };

            BlastAttack.Result result = blast.Fire();
            int hitCount = result.hitCount;

            EffectManager.SpawnEffect(
                Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFX.prefab").WaitForCompletion(),
                new EffectData
                {
                    origin = body.transform.position,
                    scale = radius,
                },
                true
            );

            if (hitCount > 0)
            {
                body.healthComponent.health += (hitCount * (body.maxHealth / 100)) * (1 + (itemCount - 1) / 10);
                body.healthComponent.health += (body.maxHealth / 100) * 2;
                body.healthComponent.shield += (hitCount * (body.maxShield / 100)) * (1 + (itemCount - 1) / 10);
            }
        }

        public override void CreateConfig(ConfigFile config)
        {
            DMGMod = config.Bind<float>("Item: " + ItemName, "Damagemod", 1.5f, "What should the player's crit chance be increased by?");
            Radius = config.Bind<float>("Item: " + ItemName, "Radius", 20f, "What should the player's crit chance be increased by?");
        }
    }

    public class SapphireAmulet_Tracker : MonoBehaviour
    {
        public bool wasRegeneratingShield = false;
        public float lastShieldValue = 0f;
    }

}
