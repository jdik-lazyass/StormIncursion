using BepInEx.Configuration;
using MonoMod.Cil;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MissileAdj
{
    public static void Init()
    {
        Hooks();
        var missilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/MissileProjectile.prefab").WaitForCompletion();

        var projSimple = missilePrefab.GetComponent<RoR2.Projectile.ProjectileController>();
        projSimple.procCoefficient = 0.2f;

        var missileController = missilePrefab.GetComponent<RoR2.Projectile.MissileController>();
        missileController.delayTimer = 0.1f;
        missileController.maxVelocity = 40f;
        missileController.turbulence = 4f;
    }

    private static void Hooks()
    {

    }
}
