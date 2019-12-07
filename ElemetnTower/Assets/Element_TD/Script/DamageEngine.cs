using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Element
{
    public class DamageEngine
    {
        public static float ElementCombatAlgorithm(float damage, ElementTypes ElementType)
        {
            float TypeFactor = 0.0f;
            float ElementDamage = damage;
            switch(ElementType)
            {
                case ElementTypes.Fire:
                    TypeFactor = 0f;
                    break;
                case ElementTypes.Glacier:
                    TypeFactor = 0.1f;
                    break;
                case ElementTypes.Wind:
                    TypeFactor = 0.5f;
                    break;
            }
            damage = ElementDamage * TypeFactor;
            return damage;
        }
    }
}

