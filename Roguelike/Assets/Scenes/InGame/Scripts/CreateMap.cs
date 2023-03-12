using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : ûũ ���� �ǹ��Ѵ�. //�� ���� //ȸ����
////////////////////////////////////////////////////////////////////////////////
public struct ChunkValue
{
    public RoomType1 roomType;
    public uint rotate;
    public ChunkValue(RoomType1 pRoomType, uint pRotate)
    {
        roomType = pRoomType;
        rotate = pRotate;       
    }
}

public struct MapMonster
{
    public Obj monsterType;
    public Vector2Int pos;
    public MapMonster(Obj pMonsterType, int pX,int pY)
    {
        monsterType = pMonsterType;
        pos = new Vector2Int(pX,pY);
    }

}

////////////////////////////////////////////////////////////////////////////////
/// : �����ϰ� ���� �����ϴ� �κ�
////////////////////////////////////////////////////////////////////////////////
public abstract class CreateMap : SerializedMonoBehaviour
{
    public const float tileSize = 2.46f;

    [SerializeField]
    protected GetTile getTile;
    [SerializeField]
    protected TileObj tileObj;

    [HideInInspector]
    public List<Vector2Int> startPosList = new List<Vector2Int>();
    [HideInInspector]
    public List<Vector2Int> gameEndPos;
    [HideInInspector]
    public List<Vector2Int> gunBenchPos;
    [HideInInspector]
    public List<Vector2Int> shopPos;
    [HideInInspector]
    public List<MapMonster> monsterList = new List<MapMonster>();

    [HideInInspector]
    public TileObj[,] tileObjs;

    public virtual IEnumerator runCreateGameMap()
    {
        yield break;
    }

    protected virtual void SetTileType(int pX, int pY, uint mapW, uint mapH,
        ref TileObj pTileObj, ref TileType[,] pTileType)
    {
        //�ֺ� ����� �ǵ��ؼ� � ����� ����� ������ ���Ѵ�.
        //�ֺ� 8���� ����� �����´�.
        List<Vector2Int> aroundPos = Calculator.GetAround8Pos(pX, pY);
        TileType[,] aroundTile = new TileType[3, 3];
        foreach (Vector2Int pos in aroundPos)
        {
            int aroundX = pos.x - pX + 1;
            int aroundY = pos.y - pY + 1;
            if (pos.x < 0 || pos.y < 0
                || pos.x >= mapW || pos.y >= mapH)
                aroundTile[aroundX, aroundY] = TileType.Null;
            else
                aroundTile[aroundX, aroundY] = pTileType[pos.x, pos.y];
        }
        aroundTile[1, 1] = pTileType[pX, pY];

        //�˸´� Ÿ�� ����� �����Ѵ�.
        TileData tileData = getTile.GetTileData(aroundTile);
        pTileObj.isTile = tileData.tile;
    }

    protected virtual void SetObj(Obj pObj,int pX,int pY)
    {
        switch (pObj)
        {
            case Obj.Null:
                break;
            case Obj.StartPos:
                {
                    startPosList.Add(new Vector2Int(pX, pY));
                }
                break;
            case Obj.EndPos:
                {
                    gameEndPos.Add(new Vector2Int(pX, pY));
                    EtcObjManager.CreateEtcObj(gameEndPos[0], pObj);
                }
                break;
            case Obj.StoneDoor:
                    EtcObjManager.CreateEtcObj(new Vector2Int(pX, pY), pObj);
                break;
            case Obj.TorchLight:
                {
                    TorchManager torchManager = TorchManager.instance;
                    torchManager.AddTorchPos(new Vector2Int(pX, pY));
                }
                break;
            case Obj.Jar:
                {
                    JarManager jarManager = JarManager.instance;
                    jarManager.AddJarPos(new Vector2Int(pX, pY));
                }
                break;
            case Obj.Chest_Normal:
                {
                    ChestManager chestManager = ChestManager.instance;
                    chestManager.AddMakeChestPos(new Vector2Int(pX, pY), Obj.Chest_Normal);
                }
                break;
            case Obj.Chest_Normal_Pos:
                {
                    ChestManager chestManager = ChestManager.instance;
                    chestManager.AddRandomChestPos(new Vector2Int(pX, pY), Obj.Chest_Normal);
                }
                break;
            case Obj.GunBench:
                {
                    gunBenchPos.Add(new Vector2Int(pX, pY));
                    EtcObjManager.CreateEtcObj(gunBenchPos[0], pObj);
                }
                break;
            case Obj.ShopObj:
                {
                    shopPos.Add(new Vector2Int(pX, pY));
                    EtcObjManager.CreateEtcObj(shopPos[0], pObj);
                }
                break;
            //���� ��ü ���
            case Obj.Slime:
            case Obj.Tentacle:
                {
                    monsterList.Add(new MapMonster(pObj, pX, pY));
                }
                break;
        }
    }
}
