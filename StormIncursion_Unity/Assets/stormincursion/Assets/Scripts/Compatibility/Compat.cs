using BepInEx.Configuration;
using LookingGlass.ItemStatsNameSpace;
using MonoMod.Cil;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using LG = LookingGlass.ItemStatsNameSpace;

namespace stormincursion
{
    public static class Compat
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void LGCompat()
        {
            MathHelper helper = new MathHelper();

            void addMemorableWalletSupport()
            {
                ItemStatsDef ISD = new ItemStatsDef();

                // memorable wallet
                ISD.descriptions.Add("Percentage of Gold Saved: ");
                ISD.valueTypes.Add(ItemStatsDef.ValueType.Gold);
                ISD.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage); //float percent = help.HyperbolicScaling(walletStacks, perStack.Value, maxVal.Value) * 100;

                ISD.descriptions.Add("Saved Gold: ");
                ISD.valueTypes.Add(ItemStatsDef.ValueType.Gold);
                ISD.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Money);

                ISD.calculateValues = (CharacterMaster master, int stackCount) => new List<float>
                {
                    helper.HyperbolicScaling(master.inventory.GetItemCountEffective(MemorableWallet_item.ItemDef), 10f, 30f),
                    master.inventory.GetItemCountEffective(WalletCount_Item.ItemDef)
                };
                LookingGlass.ItemStatsNameSpace.ItemDefinitions.allItemDefinitions.Add((int)MemorableWallet_item.ItemDef.itemIndex, ISD);
            }

            void addSapphireRingSupport()
            {
                ItemStatsDef ISD = new ItemStatsDef();

                // sapphire ring
                ISD.descriptions.Add("Healing Modifier: ");
                ISD.valueTypes.Add(ItemStatsDef.ValueType.Health);
                ISD.measurementUnits.Add(ItemStatsDef.MeasurementUnits.FlatHealth);

                ISD.calculateValues = (CharacterMaster master, int stackCount) => new List<float>
                {
                    master.inventory.GetItemCountEffective(SapphireRing_Item.ItemDef) / 10f + 1
                };
                LookingGlass.ItemStatsNameSpace.ItemDefinitions.allItemDefinitions.Add((int)SapphireRing_Item.ItemDef.itemIndex, ISD);
            }

            addMemorableWalletSupport();
            addSapphireRingSupport();
        }
    }
}