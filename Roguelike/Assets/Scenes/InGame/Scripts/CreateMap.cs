using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 청크 값을 의미한다. //방 정보 //회전값
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
/// : 랜덤하게 맵을 생성하는 부분
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
        //주변 블록을 판독해서 어떤 모양의 블록을 만들지 정한다.
        //주변 8개의 블록을 가져온다.
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

        //알맞는 타일 모양을 배정한다.
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
                    //gunBenchPos.Add(new Vector2Int(pX, pY));
                    EtcObjManager.CreateEtcObj(new Vector2Int(pX, pY), pObj);
                }
                break;
            //몬스터 객체 등록
            case Obj.Slime:
            case Obj.Tentacle:
                {
                    monsterList.Add(new MapMonster(pObj, pX, pY));
                }
                break;
        }
    }
}
