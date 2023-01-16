using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ȶ�� ���� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class TorchManager : FieldObjectSingleton<TorchManager>
{
    public Torch torchPrefab;
    public int actSize = 10;

    private Queue<Torch> torchQueue = new Queue<Torch>();
    private HashSet<Vector2Int> torchPos = new HashSet<Vector2Int>();

    private Dictionary<Vector2Int, Torch> isTorch = new Dictionary<Vector2Int, Torch>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos�� ȶ���� �ִٴ� ���� ���
    ////////////////////////////////////////////////////////////////////////////////
    public void AddTorchPos(Vector2Int pPos)
    {
        torchPos.Add(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ȶ�Ұ�ü�� �޴´�.
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
            torch = Instantiate(torchPrefab);
        }

        return torch;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos�� ȶ���� ��ġ
    ////////////////////////////////////////////////////////////////////////////////
    private void SetTorch(Vector2Int pPos)
    {
        if (isTorch.ContainsKey(pPos))
            return;
        Torch torch = GetTorch();
        isTorch[pPos] = torch;
        torch.transform.position = new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);

        MapManager mapManager = MapManager.instance;
        if (mapManager.IsWall(pPos.x, pPos.y + 1) == false)
            torch.ActDownTorch();
        else if (mapManager.IsWall(pPos.x - 1, pPos.y) == false)
            torch.ActLeftTorch();
        else if (mapManager.IsWall(pPos.x + 1, pPos.y) == false)
            torch.ActRightTorch();
        else
            torch.ActUpTorch();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos�� �ش��ϴ� ȶ�� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveTorch(Vector2Int pPos)
    {
        if(isTorch.ContainsKey(pPos))
        {
            Torch torch = isTorch[pPos];
            isTorch.Remove(pPos);
            torchQueue.Enqueue(torch);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش���ġ(x,y)�ֺ��� actSize��ŭ�� ������ ȶ���� Ȱ��ȭ ��Ų��.
    ////////////////////////////////////////////////////////////////////////////////
    public void ActAreaTorch(int x, int y)
    {
        int arrayW = MapManager.instance.arrayW;
        int arrayH = MapManager.instance.arrayH;

        bool[,] isActCheck = new bool[arrayW, arrayH];

        //��ü ��Ȱ��ȭ
        for (int i = 0; i < arrayW; i++)
            for (int j = 0; j < arrayH; j++)
                isActCheck[i, j] = false;

        //x,y �ֺ� ȶ�� Ȱ��ȭ
        for (int ax = x - actSize; ax <= x + actSize; ax++)
        {
            for (int ay = y - actSize; ay <= y + actSize; ay++)
            {
                if (ax < 0 || ay < 0 || ax >= arrayW || ay >= arrayH)
                    continue;
                isActCheck[ax, ay] = true;
            }
        }

        //�������� Ȱ��ȭ
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
