using JDContent.Scripts.Buffs;
using JDContent.Scripts.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements.UIR;
using static R2API.RecalculateStatsAPI;

namespace JDContent.Scripts.Buffs
{
    public class DefragmentedOffering_debuff : BuffBase<DefragmentedOffering_debuff>
    {
        public override string BuffName => "DefragmentedOffering_debuff";

        public override Color Color => new Color32(0, 77, 255, 255);

        public override Sprite BuffIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();

        public override bool CanStack { get; set; } = true;

        public override void Init()
        {
            base.Init();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += HandleBuff;
        }

        private void HandleBuff(CharacterBody sender, StatHookEventArgs args)
        {
            
        }

    }
}