using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacierDebuff : Debuffs
{
    private float countDown = 3f;
    public GlacierDebuff(Enemy Target) : base(Target)
    {

    }
    public override void Update()
    {
        Target.Speed -= countDown * Time.deltaTime;
        if(countDown < 0)
        {
            countDown = 3f;
        }
        base.Update();
    }
}
