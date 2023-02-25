using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �����Ϳ��� ���Ǵ� Ÿ�� ��ü
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
