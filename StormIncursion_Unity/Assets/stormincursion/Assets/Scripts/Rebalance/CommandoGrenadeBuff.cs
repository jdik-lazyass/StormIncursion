using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using BepInEx.Configuration;
using R2API;
using R2API.ScriptableObjects;
using MonoMod.Cil;
using stormincursion;
using Unity;
using UnityEditor;
using UnityEngine.AddressableAssets;
using RoR2.Projectile;

public class CommandoGrenadeBuff
{
    public static void Init()
    {
        Hooks();
        var grenadePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoGrenadeProjectile.prefab").WaitForCompletion();

        var explosion = grenadePrefab.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>();

        explosion.detonateOnEnemy = true;
        explosion.destroyOnEnemy = true;

        var simple = grenadePrefab.GetComponent<ProjectileSimple>();
        simple.desiredForwardSpeed = 75f; 
    }

    // Update is called once per frame
    private static void Hooks()
    {
        
    }

}
