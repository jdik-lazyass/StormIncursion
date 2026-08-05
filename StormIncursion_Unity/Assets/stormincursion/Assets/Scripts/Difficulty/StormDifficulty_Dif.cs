using BepInEx.Configuration;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class StormDifficulty_Dif
    {
        public static DifficultyIndex difficultyIndex;

        public static void Init(DifficultyIndex index)
        {
            difficultyIndex = index;
            Hooks();
        }

        public static bool isActive()
        {
            if (Run.instance != null)
            {
                return Run.instance.selectedDifficulty == difficultyIndex;
            }
            else
            {
                return false;
            }
        }

        private static void Hooks()
        {
            On.RoR2.BossGroup.OnDefeatedServer += BossGroup_OnDefeatedServer;
        }

        private static IEnumerator ApplyStormBonusNextFrame()
        {
            yield return null;

            float bonus = 100f + (80f * (StormLevel.CurrentStormLevel * 0.1f));

            foreach (var director in CombatDirector.instancesList)
            {
                if (!director || !director.enabled) continue;

                director.monsterCredit += bonus;
            }
        }

        private static void BossGroup_OnDefeatedServer(On.RoR2.BossGroup.orig_OnDefeatedServer orig, BossGroup self)
        {
            orig(self);

            if (!isActive()) return;

            Run.instance.StartCoroutine(ApplyStormBonusNextFrame());
        }

    }
}