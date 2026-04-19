using BepInEx.Configuration;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class StormDifficulty_Dif
    {
        public static DifficultyDef DifDef;

        public static void Init(DifficultyDef difdef)
        {
            difdef = DifDef;
            Hooks();
        }

        private static void Hooks()
        {
            
        }
    }
}