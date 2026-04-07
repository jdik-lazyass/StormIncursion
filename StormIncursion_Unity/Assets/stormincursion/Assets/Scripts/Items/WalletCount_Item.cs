using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class WalletCount_Item
    {
        public static ItemDef ItemDef;

        public static void Init(ItemDef itemDef, ConfigFile config, GameObject displayPrefab)
        {
            ItemDef = itemDef;
            Hooks();
        }

        private static void Hooks()
        {

        }
    }
}