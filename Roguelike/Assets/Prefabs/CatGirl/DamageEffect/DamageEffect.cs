using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : FieldObjectSingleton<DamageEffect>
{
    [SerializeField]
    private DamageAni damageAniPrefabs;
    [SerializeField]
    private DamageAni damageCriAniPrefabs;

    private Queue<DamageAni> effectQueue = new Queue<DamageAni>();
    private Queue<DamageAni> effectCriQueue = new Queue<DamageAni>();

    public IEnumerator runCreateObj()
    {
        for (int i = 0; i < 2; i++)
        {
            DamageAni damageCriAni = Instantiate(damageCriAniPrefabs);
            DamageAni damageAni = Instantiate(damageAniPrefabs);
            damageCriAni.transform.parent = transform;
            damageAni.transform.parent = transform;
            Enqueue(damageCriAni, true);
            Enqueue(damageAni, false);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Enqueue(DamageAni pDamageAni, bool pCritical)
    {
        if(pCritical)
            effectCriQueue.Enqueue(pDamageAni);
        else
            effectQueue.Enqueue(pDamageAni);
    }

    public DamageAni GetDamagePrefabs(bool pCritical)
    {
        DamageAni damageAni = null;
        if(pCritical)
        {
            if (effectCriQueue.Count > 0)
                damageAni = effectCriQueue.Dequeue();
        }
        else
        {
            if (effectQueue.Count > 0)
                damageAni = effectQueue.Dequeue();
        }
        return damageAni;
    }

    public void DamageEffectRun(Vector2Int pPos,int pDamage, bool pCritical)
    {
        DamageAni damageAni = GetDamagePrefabs(pCritical);
        if (damageAni == null)
        {
            if(pCritical)
                damageAni = Instantiate(damageCriAniPrefabs);
            else
                damageAni = Instantiate(damageAniPrefabs);
        }
        damageAni.UpdateText(pDamage.ToString());
        damageAni.gameObject.SetActive(true);
        damageAni.transform.parent = transform;

        damageAni.transform.position = 
            new Vector3(CreateMap.tileSize*pPos.x, CreateMap.tileSize * pPos.y, 0)
            + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),0);
    }
}
