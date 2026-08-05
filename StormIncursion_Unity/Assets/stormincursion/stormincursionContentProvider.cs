using BepInEx.Logging;
using R2API;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace stormincursion
{
    public class stormincursionContent : IContentPackProvider
    {
        public string identifier => stormincursionMain.GUID;

        public static ReadOnlyContentPack readOnlyContentPack => new ReadOnlyContentPack(stormincursionContentPack);
        internal static ContentPack stormincursionContentPack { get; } = new ContentPack();

        public static AssetBundle _myBundle;

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            var asyncOperation = AssetBundle.LoadFromFileAsync(stormincursionMain.assetBundleDir);
            while(!asyncOperation.isDone)
            {
                args.ReportProgress(asyncOperation.progress);
                yield return null;
            }

            // whole stuff
            _myBundle = asyncOperation.assetBundle;
            var expansionDef = _myBundle.LoadAsset<ExpansionDef>("StormIncursion_Expansion");
            stormincursionContentPack.expansionDefs.Add(new ExpansionDef[] { expansionDef });

            #region content
            // buffs
            var keychainBuff = _myBundle.LoadAsset<BuffDef>("KeychainBuff");
            var icecreamCD = _myBundle.LoadAsset<BuffDef>("IcecreamCooldown");
            KeychainBuff.Init(keychainBuff);
            IcecreamCooldown_buff.Init(icecreamCD);
            stormincursionContentPack.buffDefs.Add(new BuffDef[] { keychainBuff, icecreamCD });

            // vfx
            var sapphireEffect = _myBundle.LoadAsset<GameObject>("sapphireRingEffect");
            var mat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matGenericFlash.mat").WaitForCompletion();
            sapphireEffect.GetComponentInChildren<ParticleSystemRenderer>().material = mat;

            stormincursionContentPack.effectDefs.Add(new EffectDef[] { new EffectDef(sapphireEffect) });

            // items
            Keychain_Item.Init(_myBundle.LoadAsset<ItemDef>("Keychain"), stormincursionMain.instance.Config, _myBundle.LoadAsset<GameObject>("KeyChainDisplay"));
            KeychainInvis_Item.Init(_myBundle.LoadAsset<ItemDef>("Keychain_InvisTracker"), stormincursionMain.instance.Config, null);

            IceCream_Item.Init(_myBundle.LoadAsset<ItemDef>("IcecreamEclipse"), stormincursionMain.instance.Config, _myBundle.LoadAsset<GameObject>("IceCreamDisplay"));

            SapphireRing_Item.Init(_myBundle.LoadAsset<ItemDef>("SapphireRing"), stormincursionMain.instance.Config, _myBundle.LoadAsset<GameObject>("SapphireRingDisplay"), sapphireEffect);

            MemorableWallet_item.Init(_myBundle.LoadAsset<ItemDef>("MemorableWallet"), stormincursionMain.instance.Config, _myBundle.LoadAsset<GameObject>("WalletDisplay"));
            WalletCount_Item.Init(_myBundle.LoadAsset<ItemDef>("MemorableWalletCount"), stormincursionMain.instance.Config, null);
            
            stormincursionContentPack.itemDefs.Add(new ItemDef[] { Keychain_Item.ItemDef, KeychainInvis_Item.ItemDef, IceCream_Item.ItemDef, SapphireRing_Item.ItemDef, MemorableWallet_item.ItemDef, WalletCount_Item.ItemDef});

            // equipment
            QuestBattery.Init(_myBundle.LoadAsset<EquipmentDef>("QuestBattery"), stormincursionMain.instance.Config);

            stormincursionContentPack.equipmentDefs.Add(new EquipmentDef[] { QuestBattery.EquipmentDef });

            // difficulty
            var serializableDifficulty = _myBundle.LoadAsset<SerializableDifficultyDef>("Storm_Difficulty");
            DifficultyAPI.AddDifficulty(serializableDifficulty);

            StormDifficulty_Dif.Init(serializableDifficulty.DifficultyIndex);
            StormLevel.Init(serializableDifficulty.DifficultyIndex);
            StormStageController.Init();
            StageTimer.Init();

            // interactables
            Dispenser.Init();
            Injector.Init();

            // language
            R2API.LanguageAPI.Add("stormincursion_lang", System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(stormincursionMain.pluginInfo.Location),"stormincursion_lang.language")));
            #endregion

            // compatibility
            if (stormincursionMain.isLookingGlassInstalled)
            {
                RoR2Application.onLoad += Compat.LGCompat;
                stormincursionMain.logger.LogInfo("Looking glass found, calling compatibility cs file.");
            }

            #region rebalances
            // items
            AtGNerf.Init();
            MissileAdj.Init();
            FaradaySpursAdj.Init();

            // director
            CreditCostAdjustment.Init();

            // characters
            CommandoGrenadeBuff.Init();
            CommandoFMJBuff.Init();

            HuntressArrowRainBuff.Init();
            #endregion
        }
        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(stormincursionContentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }
        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }
        private void AddSelf(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(this);
        }
        internal stormincursionContent()
        {
            ContentManager.collectContentPackProviders += AddSelf;
        }
    }
}
