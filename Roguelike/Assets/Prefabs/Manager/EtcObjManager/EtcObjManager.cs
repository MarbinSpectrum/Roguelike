using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtcObjManager : FieldObjectSingleton<EtcObjManager>
{
    [SerializeField]
    protected GameObject startStairsObj;
    [SerializeField]
    protected GameObject exitStairsObj;
    [SerializeField]
    protected GameObject gunBench;

    public static void CreateStartStairs(Vector2Int pPos)
    {
        GameObject stairs = Instantiate(instance.startStairsObj);
        stairs.transform.position =
            new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);
    }

    public static void CreateExitStairs(Vector2Int pPos)
    {
        GameObject stairs = Instantiate(instance.exitStairsObj);
        stairs.transform.position =
            new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);
    }

    public static void CreateGunbench(Vector2Int pPos)
    {
        GameObject gunBench = Instantiate(instance.gunBench);
        gunBench.transform.position =
            new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);
    }
}
