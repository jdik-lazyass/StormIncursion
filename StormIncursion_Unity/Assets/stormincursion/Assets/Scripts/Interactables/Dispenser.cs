using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace stormincursion
{
    public class Dispenser
    {
        public static InteractableSpawnCard card;

        public static void Init()
        {
            card = stormincursionContent._myBundle.LoadAsset<InteractableSpawnCard>("iscDispenser");

            PurchaseInteraction purchaseInteraction = card.prefab.AddComponent<PurchaseInteraction>();
            purchaseInteraction.contextToken = "DISPENSER_CONTEXT";
            purchaseInteraction.NetworkdisplayNameToken = "DISPENSER_NAME";
            purchaseInteraction.costType = CostTypeIndex.None;
            purchaseInteraction.available = true;

            DispenserManager manager = card.prefab.GetComponent<DispenserManager>();
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