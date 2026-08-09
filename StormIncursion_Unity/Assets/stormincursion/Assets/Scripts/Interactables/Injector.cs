using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace stormincursion
{
    public class Injector
    {
        public static InteractableSpawnCard card;

        public static void Init()
        {
            card = stormincursionContent._myBundle.LoadAsset<InteractableSpawnCard>("iscInjector");

            PurchaseInteraction purchaseInteraction = card.prefab.GetComponent<PurchaseInteraction>();

            InjectorManager manager = card.prefab.GetComponent<InjectorManager>();
            manager.purchaseInteraction = purchaseInteraction;

            DirectorCard directorCard = new DirectorCard
            {
                selectionWeight = 100,
                spawnCard = card,
            };

            DirectorAPI.DirectorCardHolder holder = new DirectorAPI.DirectorCardHolder
            {
                Card = directorCard,
                InteractableCategory = DirectorAPI.InteractableCategory.Shrines
            };

            DirectorAPI.Helpers.AddNewInteractable(holder);
        }
    }
}