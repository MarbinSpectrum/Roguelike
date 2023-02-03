using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : �����Ϳ��� ���Ǵ� ������Ʈ ��ü
////////////////////////////////////////////////////////////////////////////////

[ExecuteInEditMode]
public class EditorObj : SerializedMonoBehaviour
{
    [Title("Obj")]
    [SerializeField,HideInInspector]
    private Obj Obj;
    [ShowInInspector, PropertyOrder(-2)]
    public Obj obj
    {
        get
        {
            return Obj;
        }
        set
        {
            Obj = value;

            Sprite sprite = objManager.GetSprite(Obj);
            if(sprite == null)
                sprite = monsterManager.GetSprite(Obj);
            spriteRenderer.sprite = sprite;
        }
    }

    [Title("RequireData")]
    [SerializeField]
    private ObjManager objManager;
    [SerializeField]
    private MonsterManager monsterManager;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Sprite sprite = objManager.GetSprite(Obj);
        if (sprite == null)
            sprite = monsterManager.GetSprite(Obj);
        spriteRenderer.sprite = sprite;
    }
}


