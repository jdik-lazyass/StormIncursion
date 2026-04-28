using BepInEx.Configuration;
using MonoMod.Cil;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CreditCostAdjustment
{
    public static void Init()
    {
        Hooks();
        var cscTanker = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC3/Tanker/cscTanker.asset").WaitForCompletion();

        cscTanker.directorCreditCost = 35;

        var cscDistributor = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC3/MinePod/cscMinePod.asset").WaitForCompletion();

        cscTanker.directorCreditCost = 55;
    }

    private static void Hooks()
    {

    }
}
