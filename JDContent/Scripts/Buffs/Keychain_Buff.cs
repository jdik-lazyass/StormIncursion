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
    public class Keychain_Buff : BuffBase<Keychain_Buff>
    {
        public override string BuffName => "Keychain_Buff";

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
            if (sender.HasBuff(BuffDef))
            {
                if ((sender.GetBuffCount(BuffDef) * Keychain_Item.instance.LuckInc.Value * 8f) >= 1)
                {
                    args.luckAdd += 1f; // luck only increases proc if its incremented in int (e.g. 0.5 luck will act like 0)
                }
                args.critAdd += sender.GetBuffCount(BuffDef) * 1.15f;
                JDContentPlugin.Log.LogInfo($"Luck: {sender.master.luck} stacks : {sender.GetBuffCount(BuffDef)}, luckInc: {sender.GetBuffCount(BuffDef) * Keychain_Item.instance.LuckInc.Value}");
            }
        }

    }
}