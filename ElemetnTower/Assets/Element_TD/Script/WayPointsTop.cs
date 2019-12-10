using UnityEngine;

public class WayPointsTop : WayPointFather
{

    public static Transform[] TopPoints;

    void Awake()
    {
        TopPoints = new Transform[transform.childCount];
        for (int i = 0; i < TopPoints.Length; i++)
        {
            TopPoints[i] = transform.GetChild(i);
        }
    }

}