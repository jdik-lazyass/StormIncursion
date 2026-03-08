using BepInEx;
using ExamplePlugin;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JDContent.Scripts.Interactables
{
    public class SpecificSuppressorSpawn : MonoBehaviour
    {
        private InteractableSpawnCard suppressorCard;

        private Vector3 spawnPosition = new Vector3(-76f, -26.4f, 13f); // координаты в Lunar Bazaar

        private BepInEx.Logging.ManualLogSource Logger;

        public void SpecificSuppressorSpawnF()
        {

            // Загружаем SpawnCard
            suppressorCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/VoidSuppressor/iscVoidSuppressor.asset").WaitForCompletion();

            if (suppressorCard == null)
            {
                return;
            }

            // Подписка на начало стейджа
            Stage.onStageStartGlobal += OnStageStart;

        }

        private void OnStageStart(Stage stage)
        {
            // Проверяем, что это Lunar Bazaar
            if (stage.sceneDef.cachedName != "bazaar") return;

            if (suppressorCard?.prefab != null)
            {
                // Создаём объект на конкретной позиции
                Instantiate(suppressorCard.prefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
