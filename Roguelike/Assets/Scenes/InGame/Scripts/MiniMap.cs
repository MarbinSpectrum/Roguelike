using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �̴ϸ� ���� ���� ��ũ��Ʈ
////////////////////////////////////////////////////////////////////////////////
public class MiniMap : MonoBehaviour
{
    private Texture2D mapTexture;
    private RenderTexture renderTexture;
    private MeshRenderer meshRenderer;
    private Texture2D brushTexture;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �̴ϸ��� ����
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
    /// : pPos ��ġ�� �̴ϸ� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateMiniMapPos(Vector2Int pPos)
    {
        MapManager mapManager = MapManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
         Vector2Int cPos = characterManager.CharactorGamePos();
        if (Vector2.Distance(pPos,cPos) <= 0)
        {
            //ĳ���� ��ġ ǥ��
            mapTexture.SetPixel(pPos.x, pPos.y, new Color(0, 1, 0, 1f));
        }
        else if (mapManager.IsEdge(pPos.x, pPos.y))
        {
            //�ܰ��� ǥ��
            mapTexture.SetPixel(pPos.x, pPos.y, new Color(0.3f, 0.3f, 0.3f, 1f));
        }
        else if (mapManager.IsWall(pPos.x, pPos.y) == false)
        {
            //�ٴ� ǥ��
            mapTexture.SetPixel(pPos.x, pPos.y, new Color(0.5f, 0.5f, 0.5f, 1f));
        }
        mapTexture.Apply();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �̴ϸ� �ؽ��� ��ȯ
    ////////////////////////////////////////////////////////////////////////////////
    public Texture2D GetMiniMapTexture()
    {
        return mapTexture;
    }
}
