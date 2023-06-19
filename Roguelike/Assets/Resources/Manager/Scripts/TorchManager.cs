using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 횃불 관련 매니저
////////////////////////////////////////////////////////////////////////////////
public class TorchManager : DontDestroySingleton<TorchManager>
{
    public Torch torchPrefab;
    public int actSize = 10;

    private Queue<Torch> torchQueue = new Queue<Torch>();
    private HashSet<Vector2Int> torchPos = new HashSet<Vector2Int>();

    private Dictionary<Vector2Int, Torch> isTorch = new Dictionary<Vector2Int, Torch>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos에 횃불이 있다는 것을 등록
    ////////////////////////////////////////////////////////////////////////////////
    public void AddTorchPos(Vector2Int pPos)
    {
        torchPos.Add(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 횃불객체를 받는다.
    ////////////////////////////////////////////////////////////////////////////////
    private Torch GetTorch()
    {
        Torch torch = null;

        if (torchQueue.Count > 0)
        {
            torch = torchQueue.Dequeue();
        }
        else
        {
            torch = Instantiate(torchPrefab, transform);
        }

        return torch;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos에 횃불을 설치
    ////////////////////////////////////////////////////////////////////////////////
    private void SetTorch(Vector2Int pPos)
    {
        if (isTorch.ContainsKey(pPos))
            return;
        Torch torch = GetTorch();
        torch.gameObject.SetActive(true);
        isTorch[pPos] = torch;
        torch.transform.position = new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);

        MapManager mapManager = MapManager.instance;

        if (mapManager.IsWall(pPos.x, pPos.y - 1) == false)
            torch.ActUpTorch(); 
        else if (mapManager.IsWall(pPos.x, pPos.y + 1) == false)
            torch.ActDownTorch();
        else if (mapManager.IsWall(pPos.x - 1, pPos.y) == false)
            torch.ActLeftTorch();
        else if (mapManager.IsWall(pPos.x + 1, pPos.y) == false)
            torch.ActRightTorch();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos에 해당하는 횃불 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveTorch(Vector2Int pPos)
    {
        if(isTorch.ContainsKey(pPos))
        {
            Torch torch = isTorch[pPos];
            torch.gameObject.SetActive(false);
            isTorch.Remove(pPos);
            torchQueue.Enqueue(torch);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 모든 궤작을 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveAll_TorchObj()
    {
        foreach (Vector2Int tPos in torchPos)
        {
            RemoveTorch(tPos);
        }

        torchPos.Clear();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 해당위치(x,y)주변에 actSize만큼의 공간의 횃불을 활성화 시킨다.
    ////////////////////////////////////////////////////////////////////////////////
    public void ActAreaTorch(int x, int y)
    {
        int arrayW = MapManager.instance.arrayW;
        int arrayH = MapManager.instance.arrayH;

        bool[,] isActCheck = new bool[arrayW, arrayH];

        //전체 비활성화
        for (int i = 0; i < arrayW; i++)
            for (int j = 0; j < arrayH; j++)
                isActCheck[i, j] = false;

        //x,y 주변 횃불 활성화
        for (int ax = x - actSize; ax <= x + actSize; ax++)
        {
            for (int ay = y - actSize; ay <= y + actSize; ay++)
            {
                if (ax < 0 || ay < 0 || ax >= arrayW || ay >= arrayH)
                    continue;
                isActCheck[ax, ay] = true;
            }
        }

        //실질적인 활성화
        for (int i = 0; i < arrayW; i++)
            for (int j = 0; j < arrayH; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                if (torchPos.Contains(pos))
                {
                    if(isActCheck[i,j])
                        SetTorch(pos);
                    else
                        RemoveTorch(pos);
                }
            }
    }
}
