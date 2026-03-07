using BepInEx.Logging;
using RoR2;
using UnityEngine;

namespace JDContent.Scripts.Difficulty
{
    public class TeleporterDirectorBonus
    {

        // Настройки
        private const float BONUS_CREDITS = 400f;
        private const float PASSIVE_CREDIT_MULTIPLIER = 35.5f;

        private bool _creditsGranted = false;
        private bool _bossDefeated = false;
        private float _originalCreditMultiplier = -1f;
        private bool _passiveRestored = false;

        public static bool ExtraCredits = false;

        public void Init()
        {
            On.RoR2.BossGroup.OnDefeatedServer += OnBossGroupDefeated;
            On.RoR2.TeleporterInteraction.FixedUpdate += OnTeleporterFixedUpdate;
            Stage.onStageStartGlobal += OnStageStart;
        }

        private void OnStageStart(Stage stage)
        {
            _creditsGranted = false;
            _passiveRestored = false;
            _bossDefeated = false;
            _originalCreditMultiplier = -1f;
        }

        private void OnBossGroupDefeated(
            On.RoR2.BossGroup.orig_OnDefeatedServer orig,
            BossGroup self)
        {
            orig(self);
            _bossDefeated = true;
        }

        private void OnTeleporterFixedUpdate(On.RoR2.TeleporterInteraction.orig_FixedUpdate orig, TeleporterInteraction self)
        {
            orig(self);

            if (self.activationState == TeleporterInteraction.ActivationState.Charged && !_passiveRestored)
            {
                _passiveRestored = true;
                RestorePassiveCredits();
            }

            if (!ExtraCredits) return;

            bool isCharging = self.activationState == TeleporterInteraction.ActivationState.Charging;

            if (isCharging && _bossDefeated && !_creditsGranted)
            {
                _creditsGranted = true;
                GrantDirectorCredits();
            }
        }

        private void GrantDirectorCredits()
        {
            if (Run.instance == null) return;
            if (Run.instance.selectedDifficulty != JDContent.Scripts.Difficulty.StormIncursion.ModDifIndex) return;
            if (!ExtraCredits) return;
            float difficulty = Run.instance != null ? Run.instance.difficultyCoefficient : 1f;
            float scaledBonus = BONUS_CREDITS * difficulty;

            foreach (CombatDirector director in CombatDirector.instancesList)
            {
                director.monsterCredit += scaledBonus;

                if (_originalCreditMultiplier < 0f)
                    _originalCreditMultiplier = director.creditMultiplier;

                director.creditMultiplier *= PASSIVE_CREDIT_MULTIPLIER;
                director.monsterSpawnTimer = 0f; // сразу начать спавнить
            }
        }

        private void RestorePassiveCredits()
        {
            if (Run.instance == null) return;
            if (Run.instance.selectedDifficulty != JDContent.Scripts.Difficulty.StormIncursion.ModDifIndex) return;
            if (_originalCreditMultiplier < 0f) return;

            foreach (CombatDirector director in CombatDirector.instancesList)
            {
                director.creditMultiplier = _originalCreditMultiplier;
            }

            _originalCreditMultiplier = -1f;
        }
    }
}