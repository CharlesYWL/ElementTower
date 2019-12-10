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
            float AllFireRate = 0f;
            Tower Towers;
            switch(ElementType)
            {
                case ElementTypes.Crystal:
                    //AllFireRate = Towers.GetComponent<Tower>().FireRate;
                    TypeFactor = 0f;
                    break;
                case ElementTypes.Desert:
                    TypeFactor = 0.9f;
                    break;
                case ElementTypes.Fire:
                    TypeFactor = 0.5f;
                    break;
                case ElementTypes.Glacier:
                    TypeFactor = 0.5f;
                    break;
                case ElementTypes.Light:
                    TypeFactor = 0.9f;
                    break;
                case ElementTypes.Mountain:
                    TypeFactor = 0.7f;
                    break;
                case ElementTypes.Ocean:
                    TypeFactor = 0.9f;
                    break;
                case ElementTypes.Poison:
                    TypeFactor = 1.5f;
                    break;
                case ElementTypes.Shadow:
                    TypeFactor = 1.3f;
                    break;
                case ElementTypes.Wind:
                    TypeFactor = 0.5f;
                    break;
                case ElementTypes.Thunder:
                    TypeFactor = 0.7f;
                    break;
            }
            damage = ElementDamage * TypeFactor;
            return damage;
        }

        public void Combination(ElementTypes elementFirst, ElementTypes elementSecond)
        {
            switch(elementFirst)
            {
                case ElementTypes.Crystal:
                    if(elementSecond == ElementTypes.Fire)
                    {

                    }
                    break;
                case ElementTypes.Desert:
                    break;
                case ElementTypes.Fire:
                    break;
                case ElementTypes.Glacier:
                    break;
                case ElementTypes.Light:
                    break;
                case ElementTypes.Mountain:
                    break;
                case ElementTypes.Ocean:
                    break;
                case ElementTypes.Poison:
                    break;
                case ElementTypes.Shadow:
                    break;
                case ElementTypes.Thunder:
                    break;
                case ElementTypes.Wind:
                    break;
            }
        }
    }
}

