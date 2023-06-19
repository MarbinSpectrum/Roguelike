using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private DamageAni damageAniPrefabs;
    [SerializeField]
    private DamageAni damageCriAniPrefabs;

    private Queue<DamageAni> effectQueue = new Queue<DamageAni>();
    private Queue<DamageAni> effectCriQueue = new Queue<DamageAni>();
    private bool isLoad = false;

    public IEnumerator runCreateObj()
    {
        if (isLoad)
            yield break;
        isLoad = true;

        for (int i = 0; i < 2; i++)
        {
            DamageAni damageCriAni = Instantiate(damageCriAniPrefabs,transform);
            DamageAni damageAni = Instantiate(damageAniPrefabs, transform);
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

        damageAni.transform.position = 
            new Vector3(CreateMap.tileSize*pPos.x, CreateMap.tileSize * pPos.y + 0.5f, 0)
            + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0, 0.5f),0);
    }
}
