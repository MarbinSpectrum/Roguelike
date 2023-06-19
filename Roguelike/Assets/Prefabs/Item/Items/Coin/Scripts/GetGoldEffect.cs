using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoldEffect : MonoBehaviour
{
    [SerializeField]
    private GetGold getGoldPrefabs;
    private Queue<GetGold> effectQueue = new Queue<GetGold>();
    private bool isLoad = false;
    public IEnumerator runCreateObj()
    {
        if (isLoad)
            yield break;
        isLoad = true;

        for (int i = 0; i < 2; i++)
        {
            GetGold getGold = Instantiate(getGoldPrefabs,transform);
            Enqueue(getGold);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Enqueue(GetGold pGetGold)
    {
        effectQueue.Enqueue(pGetGold);
    }

    public GetGold GetGoldPrefabs()
    {
        GetGold getGold = null;
        if(effectQueue.Count > 0)
            getGold = effectQueue.Dequeue();
        return getGold;
    }

    public void GetGoldEffectRun(int pGold)
    {
        GetGold getGold = GetGoldPrefabs();
        if(getGold == null)
            getGold = Instantiate(getGoldPrefabs);

        getGold.UpdateText(pGold + "$");
        getGold.gameObject.SetActive(true);

        CharacterManager characterManager = CharacterManager.instance;
        Vector3 goldPos = characterManager.CharactorTransPos();
        goldPos += new Vector3(0, 1f, 0);
        getGold.transform.position = goldPos;
    }
}
