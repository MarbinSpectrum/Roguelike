using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : GurdianRing ������ ����Ʈ�� �����մϴ�.
////////////////////////////////////////////////////////////////////////////////
public class GurdianEffect : FieldObjectSingleton<GurdianEffect>
{
    public GurdianEffect_Prefab gurdianPrefabs;
    private Queue<GurdianEffect_Prefab> effectQueue = new Queue<GurdianEffect_Prefab>();
    public static void RunEffect()
    {
        instance.PlayEffect();
    }

    public void Enqueue(GurdianEffect_Prefab pGetGold)
    {
        effectQueue.Enqueue(pGetGold);
    }

    public GurdianEffect_Prefab GurdianPrefabs()
    {
        GurdianEffect_Prefab effect = null;
        if (effectQueue.Count > 0)
            effect = effectQueue.Dequeue();
        return effect;
    }
    private void PlayEffect()
    {
        GurdianEffect_Prefab effect = GurdianPrefabs();
        if (effect == null)
            effect = Instantiate(gurdianPrefabs);

        CharacterManager characterManager = CharacterManager.instance;
        effect.transform.position = characterManager.CharactorTransPos();
        effect.animation.Play();
    }
}
