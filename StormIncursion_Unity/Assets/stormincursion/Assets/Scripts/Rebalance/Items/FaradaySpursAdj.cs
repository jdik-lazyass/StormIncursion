using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using BepInEx.Configuration;
using R2API;
using R2API.ScriptableObjects;
using MonoMod.Cil;
using RoR2.Items;
using System.Reflection;


public class FaradaySpursAdj
{
    public static void Init()
    {
        Hooks();

        var field1 = typeof(JumpDamageStrikeBodyBehavior).GetField("MoveSpeedVelocityPerCharge", BindingFlags.Public | BindingFlags.Static);

        field1.SetValue(null, 0.006f);

        LanguageAPI.Add("ITEM_JUMPDAMAGESTRIKE_DESC",
                $"Moving around builds up <style=cIsUtility>charge</style>, granting up to <style=cIsUtility>+60% movement speed</style> and <style=cIsUtility>+200% jump height</style> at 100%. At 25% charge or higher, jumping triggers an <style=cIsDamage>explosive discharge</style> for <style=cIsDamage>400% <style=cStack>(+280% per stack)</style> damage</style> in a 5m to 32.3m <style=cStack>(+7.5m per stack)</style> area.");
    }

    private static void Hooks()
    {
        IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
    }

    private static void CharacterBody_RecalculateStats(ILContext il)
    {
        ILCursor c = new ILCursor(il);

        if (c.TryGotoNext(
            x => x.MatchLdcR4(1.6f), // load constant with 1.6f
            x => x.MatchLdloc(out _), // get variable num92
            x => x.MatchBgt(out _)
        ))
        {
            c.Next.Operand = 0.6f;
        }
    }
}
