using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 에디터에서 사용되는 타일 객체
////////////////////////////////////////////////////////////////////////////////
public class EditorTileObj : MonoBehaviour
{
    public TileType tileType;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SetSprite(TileData pTileData)
    {
        spriteRenderer.sprite = pTileData.tileSprite;
    }
}
