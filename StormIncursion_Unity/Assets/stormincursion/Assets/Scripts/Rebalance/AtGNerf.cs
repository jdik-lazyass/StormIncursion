using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using BepInEx.Configuration;
using R2API;
using R2API.ScriptableObjects;
using MonoMod.Cil;

public class AtGNerf
{
    // Start is called before the first frame update
    public static void Init()
    {
        Hooks();
    }

    // Update is called once per frame
    private static void Hooks()
    {
        IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_ProcessHitEnemy;
    }

    private static void GlobalEventManager_ProcessHitEnemy(ILContext IL)
    {
        ILCursor c = new ILCursor(IL);

        if (c.TryGotoNext(
            x => x.MatchLdcR4(3f), // load constant with 3f
            x => x.MatchLdloc(out _), // get variable itemCountEffective6
            x => x.MatchConvR4(), // float conversion
            x => x.MatchMul() // multiplication
        ))
        {
            c.Next.Operand = 1.5f; // current instruction, operand is instructions argument
        }
    }
}
