using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using Rewired;
using RoR2;
using UnityEngine;

namespace JDContent.Scripts.Difficulty
{
    public class DifIncOverStageTime
    {
        private float TimerFromStageStart = 0f;
        private float lastTick = 0f;
        private const float TICK_INTERVAL = 60f;
        private int TimesBuffed = 0;

        public float TimeOnStage => Run.instance != null ? Run.instance.fixedTime - TimerFromStageStart : 0f; // time on stage is fixed time - timer from stage start, else is 0

        public void Init()
        {
            Stage.onStageStartGlobal += OnStageStart;
            RoR2Application.onFixedUpdate += RoR2Application_onFixedUpdate;
            CharacterMaster.onStartGlobal += OnMasterStart;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        private void Run_onRunStartGlobal(Run obj)
        {
            TimesBuffed = 0;
            lastTick = 0f;
            TimerFromStageStart = 0f;
        }

        private void RoR2Application_onFixedUpdate()
        {
            if (Run.instance == null) return;

            if (Run.instance.isRunStopwatchPaused == true) return;

            float tempStageTime = TimeOnStage;

            if (tempStageTime - lastTick >= TICK_INTERVAL)
            {
                lastTick += TICK_INTERVAL;
                int minuteCount = Mathf.FloorToInt(lastTick / TICK_INTERVAL);

                if (minuteCount % 4 == 0)
                {
                    TimesBuffed += 1;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                    {
                        baseToken = "{0}",
                        paramTokens = new[] { "<style=cDeath>The storm blusters...</style>" }
                    });
                }
            }
        }

        private void OnStageStart(Stage stage)
        {
            TimerFromStageStart = Run.instance != null ? Run.instance.fixedTime : 0f;
            lastTick = 0f;
        }

        private void OnMasterStart(CharacterMaster master)
        {
            if (master.teamIndex != TeamIndex.Monster) return;
            if (master.inventory == null) return;
            if (Run.instance == null) return; 

            if (Run.instance.selectedDifficulty == JDContent.Scripts.Difficulty.StormIncursion.ModDifIndex)
            {
                master.inventory.GiveItemPermanent(JDContent.Scripts.Items.PermanentEnemyBuff_Item.instance.ItemDef, TimesBuffed);
            }
            
        }
    }
}
