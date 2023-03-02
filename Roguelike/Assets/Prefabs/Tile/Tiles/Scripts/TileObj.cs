using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.Universal;

////////////////////////////////////////////////////////////////////////////////
/// : 게임에서 사용되는 타일 객체
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

            //알맞는 스프라이트로 변경
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
                if(TileManager.IsFloor(tileType))
                {
                    shadowCaster2D.enabled = false;
                }
                else
                {
                    shadowCaster2D.enabled = IsRun;
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
