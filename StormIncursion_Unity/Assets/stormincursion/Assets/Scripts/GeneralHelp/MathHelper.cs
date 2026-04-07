using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stormincursion
{
    public class MathHelper
    {
        public float HyperbolicScaling(float stackInput, float scaling, float maxVal)
        {
            return stackInput * scaling / (stackInput * scaling + maxVal);
        }
    }
}
