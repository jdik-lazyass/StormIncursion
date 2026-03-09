using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace stormincursion
{
    public class Keychain_Item
    {
        public static ItemDef ItemDef;
        public static ConfigEntry<float> LuckInc;
        public static ConfigEntry<int> LuckInc_MaxStack;

        public static void Init(ItemDef itemDef, ConfigFile config)
        {
            ItemDef = itemDef;
            LuckInc = config.Bind<float>("Item: Keychain", "Luck Inc", 0.01f, "");
            LuckInc_MaxStack = config.Bind<int>("Item: Keychain", "Luck Inc Max Stack", 5, "");
            Hooks();
        }

        private static void Hooks()
        {
            On.RoR2.CharacterBody.OnTakeDamageServer += CharacterBody_OnTakeDamageServer;
        }

        private static void CharacterBody_OnTakeDamageServer(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            orig(self, damageReport);
            if (damageReport == null) return;
            if (damageReport.attackerBody == null) return;
            if (damageReport.attackerMaster?.inventory == null) return;

            int buffCount = damageReport.attackerBody.GetBuffCount(KeychainBuff.BuffDef);
            int stackCount = damageReport.attackerMaster.inventory.GetItemCountEffective(ItemDef);
            if (stackCount <= 0) return;

            float procChance = damageReport.damageInfo.procCoefficient * 100f;
            if (procChance <= 0) return;

            float divisor = 1f + stackCount * 0.1f;
            bool procSuccess = Util.CheckRoll(procChance - procChance / divisor, damageReport.attackerMaster);

            if (procSuccess)
            {
                if (buffCount < 15)
                    damageReport.attackerBody.AddBuff(KeychainBuff.BuffDef);
                else
                    for (int i = 0; i < buffCount; i++)
                        damageReport.attackerBody.RemoveBuff(KeychainBuff.BuffDef);
            }
            else
            {
                if (buffCount >= 15)
                    for (int i = 0; i < buffCount; i++)
                        damageReport.attackerBody.RemoveBuff(KeychainBuff.BuffDef);
            }
        }
    }
}