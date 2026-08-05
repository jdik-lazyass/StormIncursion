using BepInEx.Configuration;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class StormLevel
    {
        public static int CurrentStormLevel { get; private set; } = 1;
        public const int MaxStormLevel = 10;

        public static DifficultyIndex difficultyIndex;

        public static void Init(DifficultyIndex index)
        {
            difficultyIndex = index;
            Hooks();
        }

        private static void Hooks()
        {
            On.RoR2.Stage.BeginAdvanceStage += Stage_BeginAdvanceStage;
            On.RoR2.Run.Start += Run_Start;
        }

        // funcs

        public static void ChangeStormLevel(int changeVal)
        {
            CurrentStormLevel = CurrentStormLevel + changeVal;
            if (CurrentStormLevel < 1)
            {
                CurrentStormLevel = 1;
            }
            if (CurrentStormLevel > MaxStormLevel)
            {
                CurrentStormLevel = MaxStormLevel;
            }
            stormincursionMain.logger.LogInfo(CurrentStormLevel);
        }

        // events

        private static void Run_Start(On.RoR2.Run.orig_Start orig, Run self)
        {
            CurrentStormLevel = 1;

            orig(self);
        }

        private static void Stage_BeginAdvanceStage(On.RoR2.Stage.orig_BeginAdvanceStage orig, Stage self, SceneDef destinationStage)
        {
            if (CurrentStormLevel < MaxStormLevel && StormDifficulty_Dif.isActive())
            {
                ChangeStormLevel(1);
            }
            stormincursionMain.logger.LogInfo(CurrentStormLevel);
            orig(self, destinationStage);
        }
    }
}