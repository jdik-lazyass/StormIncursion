using BepInEx.Logging;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System.Collections;
using UnityEngine;
namespace stormincursion
{
    public class stormincursionContent : IContentPackProvider
    {
        public string identifier => stormincursionMain.GUID;

        public static ReadOnlyContentPack readOnlyContentPack => new ReadOnlyContentPack(stormincursionContentPack);
        internal static ContentPack stormincursionContentPack { get; } = new ContentPack();

        private static AssetBundle _myBundle;

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            var asyncOperation = AssetBundle.LoadFromFileAsync(stormincursionMain.assetBundleDir);
            while(!asyncOperation.isDone)
            {
                args.ReportProgress(asyncOperation.progress);
                yield return null;
            }

            //whole stuff

            _myBundle = asyncOperation.assetBundle;
            var expansionDef = _myBundle.LoadAsset<ExpansionDef>("StormIncursion_Expansion");
            stormincursionContentPack.expansionDefs.Add(new ExpansionDef[] { expansionDef });

            //buffs

            var keychainBuff = _myBundle.LoadAsset<BuffDef>("KeychainBuff");
            var icecreamCD = _myBundle.LoadAsset<BuffDef>("IcecreamCooldown");
            KeychainBuff.Init(keychainBuff);
            IcecreamCooldown_buff.Init(icecreamCD);
            stormincursionContentPack.buffDefs.Add(new BuffDef[] { keychainBuff, icecreamCD });

            //items

            Keychain_Item.Init(_myBundle.LoadAsset<ItemDef>("Keychain"), stormincursionMain.instance.Config, _myBundle.LoadAsset<GameObject>("KeyChainDisplay"));
            KeychainInvis_Item.Init(_myBundle.LoadAsset<ItemDef>("Keychain_InvisTracker"), stormincursionMain.instance.Config, null);

            IceCream_Item.Init(_myBundle.LoadAsset<ItemDef>("IcecreamEclipse"), stormincursionMain.instance.Config, _myBundle.LoadAsset<GameObject>("IceCreamDisplay"));

            SapphireRing_Item.Init(_myBundle.LoadAsset<ItemDef>("SapphireRing"), stormincursionMain.instance.Config, _myBundle.LoadAsset<GameObject>("SapphireRingDisplay"));

            MemorableWallet_item.Init(_myBundle.LoadAsset<ItemDef>("MemorableWallet"), stormincursionMain.instance.Config, null);
            WalletCount_Item.Init(_myBundle.LoadAsset<ItemDef>("MemorableWalletCount"), stormincursionMain.instance.Config, null);
            

            stormincursionContentPack.itemDefs.Add(new ItemDef[] { Keychain_Item.ItemDef, KeychainInvis_Item.ItemDef, IceCream_Item.ItemDef, SapphireRing_Item.ItemDef, MemorableWallet_item.ItemDef, WalletCount_Item.ItemDef});


            // language
            R2API.LanguageAPI.Add("stormincursion_lang", System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(stormincursionMain.pluginInfo.Location),"stormincursion_lang.language")));

            // compatibility
            if (stormincursionMain.isLookingGlassInstalled)
            {
                RoR2Application.onLoad += Compat.LGCompat;
                stormincursionMain.logger.LogInfo("Looking glass found, calling compatibility cs file.");
            }

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
