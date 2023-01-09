using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterObj : MonoBehaviour
{
    public Vector2Int pos;
    public bool sleep = true;
    public uint hp;
    public uint damage;
    public uint range;
    public uint moveDelay;
    public uint moveStack;

    public void Init(MonsterData pMonsterData,Vector2Int pPos)
    {
        Init(pMonsterData.hp, pMonsterData.damage, pMonsterData.range, pMonsterData.moveDelay, pPos);
    }

    public void Init(uint pHp, uint pDamage, uint pRange, uint pMoveDelay, Vector2Int pPos)
    {
        pos = pPos;
        sleep = true;

        hp = pHp;
        damage = pDamage;
        range = pRange;
        moveDelay = pMoveDelay;
    }
}
