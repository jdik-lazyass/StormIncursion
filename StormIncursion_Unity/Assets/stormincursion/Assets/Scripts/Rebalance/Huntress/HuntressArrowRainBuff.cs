using BepInEx.Configuration;
using MonoMod.Cil;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.Projectile;
using stormincursion;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HuntressArrowRainBuff
{
    public const float BaseScale = 1f;
    public const float ScalePerLevel = 0.05f;

    public static void Init()
    {
        On.RoR2.Projectile.ProjectileController.Start += ProjectileController_Start;

        LanguageAPI.Add("HUNTRESS_SPECIAL_DESCRIPTION",
        $"<style=cIsUtility>Teleport</style> into the sky. Target an area to rain arrows, <style=cIsUtility>slowing</style> all enemies and dealing <style=cIsDamage>225% damage per second</style>. Area <style=cIsUtility>radius increases</style> with <style=cIsUtility>level</style>.");
    }

    private static void ProjectileController_Start(On.RoR2.Projectile.ProjectileController.orig_Start orig, ProjectileController self)
    {
        orig(self);

        if (self.name.Contains("HuntressArrowRain") && !self.name.Contains("Indicator"))
        {
            GameObject owner = self.owner;
            CharacterBody ownerBody = owner != null ? owner.GetComponent<CharacterBody>() : null;

            if (ownerBody != null)
            {
                float scale = BaseScale + (ownerBody.level * ScalePerLevel);
                self.gameObject.transform.localScale = Vector3.one * (15f * scale);
            }
        }
    }
}
