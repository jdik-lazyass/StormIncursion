using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using RoR2;
using R2API;
using BepInEx;
using RoR2BepInExPack.GameAssetPaths;
using RoR2.ExpansionManagement;
using System.Linq;

namespace JDContent.Scripts.Interactables.Custom
{
    public class FriendshipAltar_Interactable
    {
        private GameObject friendshipStatue = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/mdlBeetle.fbx").WaitForCompletion(), "BeebleMemorialStatue");
        private Material StatueMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/MonstersOnShrineUse/matMonstersOnShrineUse.mat").WaitForCompletion();


        public void Init()
        {
            friendshipStatue.name = "FriendshipStatue";
            friendshipStatue.AddComponent<NetworkIdentity>();
            friendshipStatue.transform.localScale = new Vector3(3f, 3f, 3f);
            friendshipStatue.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMaterial = StatueMat;
            friendshipStatue.transform.GetChild(1).gameObject.AddComponent<BoxCollider>();

            // interactable
            FriendshipAltar_manager mgr = friendshipStatue.AddComponent<FriendshipAltar_manager>();
            PurchaseInteraction interaction = friendshipStatue.AddComponent<PurchaseInteraction>();
            interaction.contextToken = "Pray to Friendship Memorial";
            interaction.NetworkdisplayNameToken = "Friendship Memorial";
            mgr.purchaseInteraction = interaction;

            Highlight highlight = friendshipStatue.GetComponent<Highlight>() ?? friendshipStatue.AddComponent<Highlight>();
            highlight.targetRenderer = friendshipStatue.GetComponentInChildren<SkinnedMeshRenderer>();
            GameObject something = new GameObject();
            GameObject trigger = GameObject.Instantiate(something, friendshipStatue.transform);
            trigger.AddComponent<BoxCollider>().isTrigger = true;
            trigger.AddComponent<EntityLocator>().entity = friendshipStatue;

            // card
            InteractableSpawnCard spawnCard = ScriptableObject.CreateInstance<InteractableSpawnCard>();
            spawnCard.name = "iscFriendshipStatue";
            spawnCard.prefab = friendshipStatue;
            spawnCard.sendOverNetwork = true;
            spawnCard.hullSize = HullClassification.Golem;
            spawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            spawnCard.requiredFlags = RoR2.Navigation.NodeFlags.None;
            spawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.NoShrineSpawn;
            spawnCard.directorCreditCost = 1;
            spawnCard.occupyPosition = true;
            spawnCard.orientToFloor = false;
            spawnCard.skipSpawnWhenSacrificeArtifactEnabled = false;

            DirectorCard directorCard = new DirectorCard
            {
                selectionWeight = 100,
                spawnCard = spawnCard,
            };

            DirectorAPI.DirectorCardHolder directorCardHolder = new DirectorAPI.DirectorCardHolder
            {
                Card = directorCard,
                InteractableCategory = DirectorAPI.InteractableCategory.Shrines
            };

            DirectorAPI.Helpers.AddNewInteractable(directorCardHolder);
            PrefabAPI.RegisterNetworkPrefab(friendshipStatue);
        }
    }

    public class FriendshipAltar_manager : NetworkBehaviour
    {
        public PurchaseInteraction purchaseInteraction;
        private GameObject shrineUseEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/ShrineUseEffect.prefab").WaitForCompletion();

        public void Start()
        {
            if (NetworkServer.active && Run.instance)
            {
                int cost = Run.instance.GetDifficultyScaledCost(55);
                purchaseInteraction.cost = cost;
                purchaseInteraction.costType = CostTypeIndex.Money;
                purchaseInteraction.SetAvailable(true);
            }

            purchaseInteraction.onDetailedPurchaseServer.AddListener(OnPurchase);
        }

        [Server]
        public void OnPurchase(CostTypeDef.PayCostContext context, CostTypeDef.PayCostResults results)
        {
            purchaseInteraction.SetAvailable(false);
            if (!NetworkServer.active)
            {
                return;
            }

            FriendshipAltar_logic.SpawnAllies(gameObject.transform.position);
            purchaseInteraction.SetAvailable(false);

            EffectManager.SpawnEffect(shrineUseEffect, new EffectData()
            {
                origin = gameObject.transform.position,
                rotation = Quaternion.identity,
                scale = 3f,
                color = Color.cyan
            },  true);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "<style=cEvent><color=#F990FC>You pray and feel the atmosphere getting more windy and pleasant.</color></style>" });
        }
    }

    public class FriendshipAltar_logic
    {
        private const string BEETLE_MASTER = "BeetleMaster";
        private const string BEETLE_GUARD_MASTER = "BeetleGuardMaster";
        private const string LEMURIAN_MASTER = "LemurianMaster";
        private const string ELDER_LEMURIAN_MASTER = "LemurianBruiserMaster";
        private const string WISP_MASTER = "WispMaster";
        private const string GREATER_WISP_MASTER = "GreaterWispMaster";

        private static Dictionary<string, List<ItemDef>> monsterItems = new Dictionary<string, List<ItemDef>>
        {
            ["BeetleMaster"] = new List<ItemDef> {RoR2Content.Items.FireRing, RoR2Content.Items.Medkit, RoR2Content.Items.SiphonOnLowHealth},
            ["BeetleGuardMaster"] = new List<ItemDef> { RoR2Content.Items.FireRing, RoR2Content.Items.Medkit, RoR2Content.Items.SiphonOnLowHealth},
            ["LemurianMaster"] = new List<ItemDef> {RoR2Content.Items.Phasing, RoR2Content.Items.HealWhileSafe, RoR2Content.Items.SlowOnHit, RoR2Content.Items.SprintOutOfCombat},
            ["LemurianBruiserMaster"] = new List<ItemDef> { RoR2Content.Items.Phasing, RoR2Content.Items.HealWhileSafe, RoR2Content.Items.SlowOnHit, RoR2Content.Items.SprintOutOfCombat},
            ["WispMaster"] = new List<ItemDef> {RoR2Content.Items.FireballsOnHit, RoR2Content.Items.BoostAttackSpeed, RoR2Content.Items.ArmorPlate, RoR2Content.Items.Behemoth},
            ["GreaterWispMaster"] = new List<ItemDef> { RoR2Content.Items.FireballsOnHit, RoR2Content.Items.BoostAttackSpeed, RoR2Content.Items.ArmorPlate, RoR2Content.Items.Behemoth},
        };

        private static Dictionary<string, List<ItemDef>> monsterItemsDLC2 = new Dictionary<string, List<ItemDef>>
        {
            ["BeetleMaster"] = new List<ItemDef> {RoR2.DLC2Content.Items.KnockBackHitEnemies },
            ["BeetleGuardMaster"] = new List<ItemDef> {RoR2.DLC2Content.Items.KnockBackHitEnemies },
            ["LemurianMaster"] = new List<ItemDef> { },
            ["LemurianBruiserMaster"] = new List<ItemDef> { },
            ["WispMaster"] = new List<ItemDef> { },
            ["GreaterWispMaster"] = new List<ItemDef> { },
        };

        private static Dictionary<string, List<ItemDef>> monsterItemsDLC3 = new Dictionary<string, List<ItemDef>>
        {
            ["BeetleMaster"] = new List<ItemDef> {  },
            ["BeetleGuardMaster"] = new List<ItemDef> {  },
            ["LemurianMaster"] = new List<ItemDef> {RoR2.DLC3Content.Items.ShieldBooster },
            ["LemurianBruiserMaster"] = new List<ItemDef> {RoR2.DLC3Content.Items.ShieldBooster },
            ["WispMaster"] = new List<ItemDef> {  },
            ["GreaterWispMaster"] = new List<ItemDef> {  },
        };

        private static string GetBeetleMaster(int stage)
        {
            if (stage >= 4) return BEETLE_GUARD_MASTER;
            return BEETLE_MASTER;
        }

        private static string GetLemurianMaster(int stage)
        {
            if (stage >= 4) return ELDER_LEMURIAN_MASTER;
            return LEMURIAN_MASTER;
        }

        private static string GetWispMaster(int stage)
        {
            if (stage >= 4) return GREATER_WISP_MASTER;
            return WISP_MASTER;
        }

        public static void SpawnAllies(Vector3 position)
        {
            if (!NetworkServer.active) return;

            int stage = Run.instance != null ? Run.instance.stageClearCount + 1 : 1;

            for (int i = 0; i < 4; i++)
                SpawnAlly(GetBeetleMaster(stage), position);

            for (int i = 0; i < 2; i++)
                SpawnAlly(GetLemurianMaster(stage), position);

            for (int i = 0; i < 3; i++)
                SpawnAlly(GetWispMaster(stage), position);
        }

        private static void SpawnAlly(string masterName, Vector3 position)
        {
            int stage = Run.instance != null ? Run.instance.stageClearCount + 1 : 1;
            bool shouldBeElite = stage >= 2;

            GameObject masterPrefab = MasterCatalog.FindMasterPrefab(masterName);
            if (masterPrefab == null)
            {
                return;
            }

            Vector3 spawnPos = position + UnityEngine.Random.insideUnitSphere * 5f;
            spawnPos.y = position.y;

            CharacterMaster master = new MasterSummon()
            {
                masterPrefab = masterPrefab,
                position = spawnPos,
                rotation = Quaternion.identity,
                teamIndexOverride = TeamIndex.Player,
                ignoreTeamMemberLimit = true,
                useAmbientLevel = true,
            }.Perform();

            if (master == null)
            {
                return;
            }

            if (monsterItems.ContainsKey(masterName)){
                var needed = monsterItems[masterName];
                foreach (var item in needed)
                {
                    master.inventory.GiveItemPermanent(item);
                }
            };

            var dlc2 = ExpansionCatalog.expansionDefs.FirstOrDefault(e => e.name == "DLC2");
            if (Run.instance != null && dlc2 != null && Run.instance.IsExpansionEnabled(dlc2))
            {
                var needed = monsterItemsDLC2[masterName];
                foreach (var item in needed)
                {
                    master.inventory.GiveItemPermanent(item);
                }
            }

            var dlc3 = ExpansionCatalog.expansionDefs.FirstOrDefault(e => e.name == "DLC3");
            if (Run.instance != null && dlc3 != null && Run.instance.IsExpansionEnabled(dlc3))
            {
                var needed = monsterItemsDLC3[masterName];
                foreach (var item in needed)
                {
                    master.inventory.GiveItemPermanent(item);
                }
            }
        }
    }

    public class AllyPermanentMarker : MonoBehaviour { }
}
