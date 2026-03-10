using R2API;
using RoR2;
using UnityEngine;
using static R2API.RecalculateStatsAPI;

namespace stormincursion
{
    public class KeychainBuff
    {
        public static BuffDef BuffDef;
        public static float LuckInc = 0.01f;

        public static void Init(BuffDef buffDef)
        {
            BuffDef = buffDef;
            RecalculateStatsAPI.GetStatCoefficients += HandleBuff;
        }

        private static void HandleBuff(CharacterBody sender, StatHookEventArgs args)
        {
            if (sender.HasBuff(BuffDef))
            {
                int count = sender.GetBuffCount(BuffDef);

                int stacks = sender.inventory.GetItemCountEffective(Keychain_Item.ItemDef);

                if (count * LuckInc * 8f >= 1)
                    args.luckAdd += 1f;
                args.critAdd += count * (1.15f + (1 - stacks));
            }
        }
    }
}