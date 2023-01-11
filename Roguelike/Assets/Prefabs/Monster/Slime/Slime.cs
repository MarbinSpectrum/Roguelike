using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonsterObj
{
    public override void Hit(uint pDamage)
    {
        base.Hit(pDamage);

        moveStack = 0;

    }
}
