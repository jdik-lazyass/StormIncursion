using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using IL.RoR2.ExpansionManagement;
using JDContent.Scripts;
using JDContent.Scripts.Buffs;
using JDContent.Scripts.Difficulty;
using JDContent.Scripts.Interactables.Custom;
using JDContent.Scripts.Items;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using static R2API.DirectorAPI;


namespace JDContent
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInDependency(DirectorAPI.PluginGUID)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(PrefabAPI))]

    [BepInPlugin("author.JDContent", "JD'S Content", "1.0.0")]

    public class JDContentPlugin : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "AuthorName";
        public const string PluginName = "ExamplePlugin";
        public const string PluginVersion = "1.0.0";

        public static ManualLogSource Log;

        public List<ItemBase> Items = new List<ItemBase>();
        public static List<BuffBase> Buffs = new();

        public static Dictionary<BuffBase, bool> BuffStatusDictionary = new();
        public static AssetBundle MainAssets;

        public bool ValidateItem(ItemBase item, List<ItemBase> itemList)
        {
            var enabled = Config.Bind<bool>("Item: " + item.ItemName, "Enable Item?", true, "Should this item appear in runs?").Value;
            var aiBlacklist = Config.Bind<bool>("Item: " + item.ItemName, "Blacklist Item from AI Use?", false, "Should the AI not be able to obtain this item?").Value;
            if (enabled)
            {
                itemList.Add(item);
                if (aiBlacklist)
                {
                    item.AIBlacklisted = true;
                }
            }
            return enabled;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {
            // If a character was killed by the world, we shouldn't do anything.
            if (!report.attacker || !report.attackerBody)
            {
                return;
            }

            var attackerCharacterBody = report.attackerBody;

            Logger.LogInfo(report.attackerBody.outOfCombatStopwatch);
        }

        public bool LoadBuff(BuffBase buff, List<BuffBase> buffList)
        {
            BuffStatusDictionary.Add(buff, true);

            buffList.Add(buff);

            return true;
        }

        public void Awake()
        {
            Log = Logger;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JDContent.jawbreaker_assets"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }

            var suppressorSpawner = new Scripts.Interactables.NaturalSuppressorSpawn();
            suppressorSpawner.NaturalSuppressorSpawnF();

            Logger.LogInfo("Void Supressor natural -  good");

            var upgraderSpawner = new Scripts.Interactables.NaturalDroneUpgraderSpawn();
            upgraderSpawner.NaturalUpgraderSpawnF();

            Logger.LogInfo("Natural drone upgrade spawner - good");

            var suppressorInBazaar = new Scripts.Interactables.SpecificSuppressorSpawn();
            suppressorInBazaar.SpecificSuppressorSpawnF();

            Logger.LogInfo("Suppressor in bazar - good");

            var teleporterBonus = new Scripts.Difficulty.TeleporterDirectorBonus();
            teleporterBonus.Init();

            var ExtraDif = new Scripts.Difficulty.DifIncOverStageTime();
            ExtraDif.Init();

            Logger.LogInfo("Teleport bonus from dif - good");

            var FriendshipAltar = new FriendshipAltar_Interactable();
            FriendshipAltar.Init();

            Logger.LogInfo("Friendship - good");

            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));

            var BuffTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BuffBase)));

            var modDiff = new StormIncursion();

            modDiff.AddDifficulty();
            modDiff.FillTokens();

            Logger.LogInfo("Dif and items - good");

            foreach (var buffType in BuffTypes)
            {
                BuffBase buff = (BuffBase)Activator.CreateInstance(buffType);
                if (LoadBuff(buff, Buffs))
                {
                    buff.Init();
                }
            }

            foreach (var itemType in ItemTypes)
            {
                ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
                if (ValidateItem(item, Items))
                {
                    item.Init(Config);

                    var tags = item.ItemTags;
                    bool aiValid = true;
                    bool aiBlacklist = false;

                    if (item.ItemDef.deprecatedTier == ItemTier.NoTier)
                    {
                        aiBlacklist = true;
                        aiValid = false;
                    }
                    string name = item.ItemName;
                    name = name.Replace("'", "");

                    foreach (var tag in tags)
                    {
                        if (tag == ItemTag.AIBlacklist)
                        {
                            aiBlacklist = true;
                            aiValid = false;
                            break;
                        }
                    }
                    if (aiValid)
                    {
                        aiBlacklist = Config.Bind<bool>("Item: " + name, "Blacklist Item from AI Use?", false, "Should the AI not be able to obtain this item?").Value;
                    }
                    else
                    {
                        aiBlacklist = true;
                    }

                    if (aiBlacklist)
                    {
                        item.AIBlacklisted = true;
                    }
                }
            }



        }
    }
}
