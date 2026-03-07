using BepInEx;
using BepInEx.Logging;
using HarmonyLib.Tools;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.DirectorAPI;

namespace JDContent.Scripts.Interactables
{
    [BepInDependency(R2API.R2API.PluginGUID)]

    public class NaturalDroneUpgraderSpawn : MonoBehaviour
    {
        private InteractableSpawnCard upgraderCard;
        private DirectorCardHolder holder;
        private List<DirectorAPI.Stage> stageList;

        private ManualLogSource Logger;

        public void NaturalUpgraderSpawnF()
        {
            // whitelisted stages
            stageList = new List<DirectorAPI.Stage>
            {
                DirectorAPI.Stage.TitanicPlains,
                DirectorAPI.Stage.DistantRoost,
                DirectorAPI.Stage.AbandonedAqueduct,
                DirectorAPI.Stage.WetlandAspect,
            };

            // loading SpawnCard
            upgraderCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC3/DroneAssemblyStation/iscDroneAssemblyStation.asset").WaitForCompletion();

            if (upgraderCard == null)
            {
                return;
            }

            // creating DirectorCard
            var directorCard = new DirectorCard
            {
                spawnCard = upgraderCard,
                selectionWeight = 1
            };

            // creating DirectorCardHolder
            holder = new DirectorCardHolder
            {
                Card = directorCard,
                InteractableCategory = InteractableCategory.Misc
            };

            // register for stages
            foreach (var stage in stageList)
            {
                Helpers.AddNewInteractableToStage(holder, stage);
            }
        }
    }
}
