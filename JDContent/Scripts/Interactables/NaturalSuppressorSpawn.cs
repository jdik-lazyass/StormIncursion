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

    public class NaturalSuppressorSpawn : MonoBehaviour
    {
        private InteractableSpawnCard suppressorCard;
        private DirectorCardHolder holder;
        private List<DirectorAPI.Stage> stageList;

        private ManualLogSource Logger;

        public void NaturalSuppressorSpawnF()
        {
            // whitelisted stages
            stageList = new List<DirectorAPI.Stage>
            {
                DirectorAPI.Stage.VoidCell,
                DirectorAPI.Stage.VoidLocus,
                DirectorAPI.Stage.SiphonedForest,

                DirectorAPI.Stage.AphelianSanctuarySimulacrum,
                DirectorAPI.Stage.AbandonedAqueductSimulacrum,
                DirectorAPI.Stage.AbyssalDepthsSimulacrum,
                DirectorAPI.Stage.CommencementSimulacrum,
                DirectorAPI.Stage.RallypointDeltaSimulacrum,
                DirectorAPI.Stage.SkyMeadowSimulacrum,
                DirectorAPI.Stage.TitanicPlainsSimulacrum
            };

            // loading SpawnCard
            suppressorCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidSuppressor/iscVoidSuppressor.asset").WaitForCompletion();

            if (suppressorCard == null)
            {
                return;
            }

            // creating DirectorCard
            var directorCard = new DirectorCard
            {
                spawnCard = suppressorCard,
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
