using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using RoR2;
using System;

namespace stormincursion
{
    public class InjectorManager : NetworkBehaviour
    {
        public PurchaseInteraction purchaseInteraction;
        private GameObject _shrineUseEffect;
        private int _batteriesLeftToInject = 2;
        public int maxBatteries = 2;

        public int BatteriesToInject => _batteriesLeftToInject;

        public static event Action<InjectorManager> onProgressChanged;

        private void Start()
        {
            _shrineUseEffect = Addressables
                .LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/ShrineUseEffect.prefab")
                .WaitForCompletion();

            if (NetworkServer.active && Run.instance)
                purchaseInteraction.SetAvailable(true);

            purchaseInteraction.onDetailedPurchaseServer.AddListener(OnPurchase);
        }

        [Server]
        public void OnPurchase(CostTypeDef.PayCostContext context, CostTypeDef.PayCostResults results)
        {
            if (!NetworkServer.active) return;

            CharacterBody body = context.activator?.GetComponent<CharacterBody>();
            if (body == null) return;

            if (body.inventory.GetEquipmentIndex() == QuestBattery.EquipmentDef.equipmentIndex && _batteriesLeftToInject >= 1)
            {
                body.inventory.SetEquipmentIndex(EquipmentIndex.None, true);
                _batteriesLeftToInject--;
            }
            else
            {
                return;
            }

            EffectManager.SpawnEffect(_shrineUseEffect, new EffectData
            {
                origin = transform.position,
                rotation = Quaternion.identity,
                scale = 3f,
                color = Color.cyan
            }, true);

            onProgressChanged?.Invoke(this);

            if (_batteriesLeftToInject == 0)
            {
                purchaseInteraction.SetAvailable(false);
                StormLevel.ChangeStormLevel(-2);
            };
        }
    }
}