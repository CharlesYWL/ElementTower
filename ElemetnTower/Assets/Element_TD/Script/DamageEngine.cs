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
                    TypeFactor = 3f;
                    break;
                case ElementTypes.Wind:
                    TypeFactor = 0.1f;
                    break;
                case ElementTypes.Crystal:
                    TypeFactor = 0f;
                    break;
                case ElementTypes.Desert:
                    TypeFactor = 0.1f;
                    break;
                case ElementTypes.Light:
                    TypeFactor = 0.3f;
                    break;
                case ElementTypes.Glacier:
                    TypeFactor = 1.5f;
                    break;
                case ElementTypes.Poison:
                    TypeFactor = 0.5f;
                    break;
                case ElementTypes.Shadow:
                    TypeFactor = 1.2f;
                    break;
                case ElementTypes.Thunder:
                    TypeFactor = 0.5f;
                    break;
                case ElementTypes.Mountain:
                    TypeFactor = 0.6f;
                    break;
            }
            damage = ElementDamage * TypeFactor;
            return damage;
        }
    }
}

