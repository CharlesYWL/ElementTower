using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuffs
{
    protected EnemyMovement Target;
    
    public Debuffs(EnemyMovement Target)
    {
        this.Target = Target;
    }

    public virtual void Update()
    {

    }
}
