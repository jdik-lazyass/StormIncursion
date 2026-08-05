using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

namespace stormincursion
{
    public static class StormStageController
    {
        private const int CandidateCount = 8; // больше = точнее расстояние, но дольше расчёт

        public static void Init()
        {
            On.RoR2.Stage.Start += Stage_Start;
        }

        private static IEnumerator Stage_Start(On.RoR2.Stage.orig_Start orig, Stage self)
        {
            yield return orig(self);

            if (!NetworkServer.active) yield break;
            if (!StormDifficulty_Dif.isActive()) yield break;
            if (Run.instance.stageClearCount % 2 != 1) yield break;

            SpawnPair();
        }

        private static void SpawnPair()
        {
            List<Vector3> candidates = new List<Vector3>();
            List<GameObject> tempObjects = new List<GameObject>();

            DirectorPlacementRule randomRule = new DirectorPlacementRule
            {
                placementMode = DirectorPlacementRule.PlacementMode.Random
            };

            for (int i = 0; i < CandidateCount; i++)
            {
                DirectorSpawnRequest request = new DirectorSpawnRequest(
                    Injector.card,
                    randomRule,
                    RoR2Application.rng
                );

                GameObject spawned = DirectorCore.instance.TrySpawnObject(request);
                if (spawned != null)
                {
                    candidates.Add(spawned.transform.position);
                    tempObjects.Add(spawned);
                }
            }

            foreach (var obj in tempObjects)
                if (obj != null) NetworkServer.Destroy(obj);

            if (candidates.Count < 2) return;

            Vector3 posA = Vector3.zero, posB = Vector3.zero;
            float bestDist = -1f;

            for (int i = 0; i < candidates.Count; i++)
            {
                for (int j = i + 1; j < candidates.Count; j++)
                {
                    float dist = Vector3.Distance(candidates[i], candidates[j]);
                    if (dist > bestDist)
                    {
                        bestDist = dist;
                        posA = candidates[i];
                        posB = candidates[j];
                    }
                }
            }

            SpawnAt(Dispenser.card, posA);
            SpawnAt(Injector.card, posB);
        }

        private static void SpawnAt(InteractableSpawnCard card, Vector3 position)
        {
            DirectorPlacementRule directRule = new DirectorPlacementRule
            {
                placementMode = DirectorPlacementRule.PlacementMode.Direct,
                position = position
            };

            DirectorSpawnRequest request = new DirectorSpawnRequest(card, directRule, RoR2Application.rng);
            DirectorCore.instance.TrySpawnObject(request);
        }
    }
}