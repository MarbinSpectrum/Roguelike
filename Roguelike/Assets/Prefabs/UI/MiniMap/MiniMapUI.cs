using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour
{

    [SerializeField]
    private Image mapImg;

    public void UpdateMiniMap(Texture2D pTexture2D)
    {
        MapManager mapManager = MapManager.instance;
        pTexture2D.filterMode = FilterMode.Point;
        Sprite sprite = Sprite.Create(pTexture2D, new Rect(0, 0, mapManager.arrayW, mapManager.arrayH)
            , new Vector2(0.5f, 0.5f));
        mapImg.sprite = sprite;
    }
}