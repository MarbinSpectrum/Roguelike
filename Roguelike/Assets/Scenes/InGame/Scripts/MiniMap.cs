using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 미니맵 생성 관련 스크립트
////////////////////////////////////////////////////////////////////////////////
public class MiniMap : MonoBehaviour
{
    private Texture2D mapTexture;
    private RenderTexture renderTexture;
    private MeshRenderer meshRenderer;
    private Texture2D brushTexture;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 미니맵을 생성
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateMiniMap()
    {
        MapManager mapManager = MapManager.instance;
        mapTexture = new Texture2D(mapManager.arrayW, mapManager.arrayH);
       
        for (int i = 0; i < mapManager.arrayW; i++)
            for (int j = 0; j < mapManager.arrayH; j++)
                mapTexture.SetPixel(i, j, new Color(0, 0, 0, 0.5f));
        mapTexture.Apply();

        yield break;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos 위치의 미니맵 갱신
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateMiniMapPos(Vector2Int pPos)
    {
        MapManager mapManager = MapManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
         Vector2Int cPos = characterManager.character.GetPos();
        if (Vector2.Distance(pPos,cPos) <= 0)
        {
            //캐릭터 위치 표시
            mapTexture.SetPixel(pPos.x, pPos.y, new Color(0, 1, 0, 1f));
        }
        else if (mapManager.IsEdge(pPos.x, pPos.y))
        {
            //외각선 표시
            mapTexture.SetPixel(pPos.x, pPos.y, new Color(0.3f, 0.3f, 0.3f, 1f));
        }
        else if (mapManager.IsWall(pPos.x, pPos.y) == false)
        {
            //바닥 표시
            mapTexture.SetPixel(pPos.x, pPos.y, new Color(0.5f, 0.5f, 0.5f, 1f));
        }
        mapTexture.Apply();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 미니맵 텍스쳐 반환
    ////////////////////////////////////////////////////////////////////////////////
    public Texture2D GetMiniMapTexture()
    {
        return mapTexture;
    }
}
