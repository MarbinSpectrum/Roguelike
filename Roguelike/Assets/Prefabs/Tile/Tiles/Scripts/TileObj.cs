using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.Universal;

////////////////////////////////////////////////////////////////////////////////
/// : ���ӿ��� ���Ǵ� Ÿ�� ��ü
////////////////////////////////////////////////////////////////////////////////
public class TileObj : SerializedMonoBehaviour
{
    #region[IsTile]
    [SerializeField, HideInInspector]
    private Tile IsTile;

    [Title("Data")]       
    [ShowInInspector, PropertyOrder(-2)]
    public Tile isTile
    {
        get
        {
            return IsTile;
        }
        set
        {
            IsTile = value;

            //�˸´� ��������Ʈ�� ����
            tileSprite.sprite = tileManager.GetSprite(IsTile);

            tileType = tileManager.GetType(isTile);
        }
    }
    #endregion

    public TileType tileType;

    #region[IsRun]
    [SerializeField, HideInInspector]
    private bool IsRun;
    [ShowInInspector, PropertyOrder(-1)]
    public bool isRun
    {
        set
        {
            if(IsRun != value)
            {
                IsRun = value;
                TileType tileType = tileManager.GetType(IsTile);
                switch (tileType)
                {
                    //Ÿ�� ������ ���� ȿ���� ó��������Ѵ�.
                    case TileType.Floor:
                        {
                            shadowCaster2D.enabled = false;
                            break;
                        }
                    case TileType.Wall:
                        {
                            shadowCaster2D.enabled = IsRun;
                            break;
                        }
                }
            }
        }
    }
    #endregion

    [Title("RequireData")]
    [SerializeField]
    private SpriteRenderer tileSprite;

    [SerializeField]
    private TileManager tileManager;

    [SerializeField]
    private ShadowCaster2D shadowCaster2D;
}
