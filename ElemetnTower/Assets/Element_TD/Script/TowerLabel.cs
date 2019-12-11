using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TowerLabel : MonoBehaviour
{
    public enum ElementType { FireTower, GlacierTower, WindTower, OceanTower, DesertTower, ThunderTower, MountainTower, LightTower, ShadowTower, CyrstalTower, PoisonTower }

    // Start is called before the first frame update
    public ElementType elementType;
    public int Level;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static bool Compare(GameObject tower1,GameObject tower2)
    {
        TowerLabel t1 = tower1.GetComponent<TowerLabel>();
        TowerLabel t2 = tower2.GetComponent<TowerLabel>();
        return (t1.elementType == t2.elementType && t1.Level == t2.Level);
    }
}
