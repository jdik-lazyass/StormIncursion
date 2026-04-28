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
using UnityEngine.AddressableAssets;
using RoR2.Projectile;

public class CommandoFMJBuff
{
    public static void Init()
    {
        Hooks();
        var projPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/FMJRamping.prefab").WaitForCompletion();

        var simple = projPrefab.GetComponent<ProjectileSimple>();
        simple.lifetime = 2;
        simple.desiredForwardSpeed = 210;

        var sphereCollider = projPrefab.GetComponent<SphereCollider>();
        sphereCollider.radius = 3f;

        var hitbox = projPrefab.transform.Find("Hitbox");
        hitbox.localScale = new Vector3(3.72f, 3.72f, 7.8f);
    }

    private static void Hooks()
    {
        
    }

}
