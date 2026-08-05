using BepInEx.Configuration;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace stormincursion
{
    public static class StageTimer
    {
        public static int stageTimeCounted = 0;
        public static Stage CurrentStage;

        public static bool BossDefeated = false;
        public static bool GivenBuffFromTimer = false;

        public const int MinuteLimiter = 1200; // 19500 for 6.5 minutes

        public static void Init()
        {
            Hooks();
        }

        private static void Hooks()
        {
            On.RoR2.BossGroup.OnDefeatedServer += BossGroup_OnDefeatedServer;
            On.RoR2.Run.OnFixedUpdate += Run_OnFixedUpdate;
            On.RoR2.Stage.Start += Stage_Start;
            On.RoR2.Stage.CompleteServer += Stage_CompleteServer;
        }

        private static void BossGroup_OnDefeatedServer(On.RoR2.BossGroup.orig_OnDefeatedServer orig, BossGroup self)
        {
            orig(self);
            BossDefeated = true;
        }

        private static void Run_OnFixedUpdate(On.RoR2.Run.orig_OnFixedUpdate orig, Run self) // 50 times per second
        {
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage
            {
                baseToken = Convert.ToString(stageTimeCounted)
            });
            orig(self);
            if (!BossDefeated && Run.instance != null && GivenBuffFromTimer == false)
            {
                stageTimeCounted++;
            }

            if (stageTimeCounted > MinuteLimiter && GivenBuffFromTimer == false)
            {
                GivenBuffFromTimer = true;
                stormincursionMain.logger.LogInfo("Given buff!");
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                {
                    baseToken = "buff given"
                });
            }
        }

        private static IEnumerator Stage_Start(On.RoR2.Stage.orig_Start orig, Stage self)
        {
            BossDefeated = false;
            stageTimeCounted = 0;
            GivenBuffFromTimer = false;
            yield return orig(self);
        }

        private static void Stage_CompleteServer(On.RoR2.Stage.orig_CompleteServer orig, Stage self)
        {
            orig(self);
            CurrentStage = self;
            BossDefeated = false;
            GivenBuffFromTimer = false;
            stageTimeCounted = 0;
        }
    }
}