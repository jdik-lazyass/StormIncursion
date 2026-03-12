using R2API;
using RoR2;
using UnityEngine;
using static R2API.RecalculateStatsAPI;

namespace stormincursion
{
    public class IcecreamCooldown_buff
    {
        public static BuffDef BuffDef;

        public static void Init(BuffDef buffDef)
        {
            BuffDef = buffDef;
        }
    }
}