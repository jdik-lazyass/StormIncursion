using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using RoR2;

namespace stormincursion
{
    public class DispenserManager : NetworkBehaviour
    {
        public PurchaseInteraction purchaseInteraction;
        private GameObject _shrineUseEffect;
        private int _batteriesLeft = 4; 

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

            if (body.inventory.GetEquipmentIndex() == EquipmentIndex.None && _batteriesLeft >= 1)
            {
                body.inventory.SetEquipmentIndex(QuestBattery.EquipmentDef.equipmentIndex, true);
                _batteriesLeft--;
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

            if (_batteriesLeft == 0)
            {
                purchaseInteraction.SetAvailable(false);
            };
        }
    }
}