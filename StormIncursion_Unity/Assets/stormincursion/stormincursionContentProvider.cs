using RoR2.ContentManagement;
using UnityEngine;
using RoR2;
using System.Collections;
using RoR2.ExpansionManagement;
using R2API;
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
            KeychainBuff.Init(keychainBuff);
            stormincursionContentPack.buffDefs.Add(new BuffDef[] { keychainBuff });

            //items

            NoHealingForCrits_Item.Init(_myBundle.LoadAsset<ItemDef>("CataclysmicJawbreaker"), stormincursionMain.instance.Config);
            Keychain_Item.Init(_myBundle.LoadAsset<ItemDef>("Keychain"), stormincursionMain.instance.Config);
            stormincursionContentPack.itemDefs.Add(new ItemDef[] { NoHealingForCrits_Item.ItemDef, Keychain_Item.ItemDef });


            // language
            R2API.LanguageAPI.Add("stormincursion_lang", System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(stormincursionMain.pluginInfo.Location),"stormincursion_lang.language")));

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
