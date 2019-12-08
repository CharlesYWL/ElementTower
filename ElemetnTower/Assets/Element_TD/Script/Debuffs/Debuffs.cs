using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuffs
{
    protected Enemy Target;
    
    public Debuffs(Enemy Target)
    {
        this.Target = Target;
    }

    public virtual void Update()
    {

    }
}
