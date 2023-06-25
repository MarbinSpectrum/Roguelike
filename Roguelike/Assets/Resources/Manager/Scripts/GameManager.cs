using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using System.IO;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : ���ӿ� ��ü���� ��Ҹ� �����Ѵ�.
////////////////////////////////////////////////////////////////////////////////
public class GameManager : DontDestroySingleton<GameManager>
{
    public static bool gunBenchAct = false;

    private static PlayData PlayData;
    public static PlayData playData
    {
        get
        {
            if(PlayData == null)
            {
                PlayData = LoadPlayData();
            }
            return PlayData;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ӽ���
    ////////////////////////////////////////////////////////////////////////////////
    public void StartGame()
    {
        StartCoroutine(runStartGame());
    }

    public IEnumerator runStartGame()
    {
        PlayData = LoadPlayData();   //�÷��� ������ �ε�

        SoundManager.StopBGM();

        yield return languageMgr.runLoadData(); //��� ������ �ε�

        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);

        MapManager mapManager = MapManager.instance;
        mapManager.SetVisitRoomIdx(playData.visitRoomIdx);
        yield return mapManager.runCreateTileMap(); //�� ����

        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterMgr.CreateCatGirl(createPos.x, createPos.y); //ĳ���� ����

        //������ġ�� ����� ����
        EtcObjManager.CreateEtcObj(createPos, Obj.StartPos);

        yield return bulletMgr.runCreateObj(); //�Ѿ� ��ü ����

        DamageEffect damageEffect = uIEffectMgr.damageEffect;
        yield return damageEffect.runCreateObj(); //������ ����Ʈ ����

        GetGoldEffect getGoldEffect = uIEffectMgr.getGoldEffect;
        yield return getGoldEffect.runCreateObj(); //���ȹ�� ����Ʈ ����

        yield return monsterMgr.runCreateMonster(mapManager.GetMonsterList()); //���� ����

        yield return jarMgr.runCreateJarObj(); //�׾Ƹ� ����

        yield return chestMgr.runCreateChestObj(); //���� ����

        shopMgr.CreateShopItem();   //���� ������ ����

        ScreenDark.AnimationState(false); //ȭ���� ������ ��Ÿ��
        totalUI.ShowCreateMap(false);
        yield return new WaitForSeconds(0.5f);

        mapManager.ShowMapName();
        yield return new WaitForSeconds(1f);
        StageBGM.PlayBgm();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��� ����� Json���Ϸ� ��������.
    ////////////////////////////////////////////////////////////////////////////////
    public static void SavePlayData()
    {
        PlayData playData = new PlayData(0);
        string jsonData = Json.ObjectToJson(playData);

        if (Application.isEditor)
            Json.CreateJsonFile(Application.dataPath, "Resources/PlayData/PlayData", jsonData);
        else
            Json.CreateJsonFile(Application.persistentDataPath, "PlayData", jsonData);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : Json���Ϸ� ����� �÷��� ����� �д´�.
    ////////////////////////////////////////////////////////////////////////////////
    public static PlayData LoadPlayData()
    {
        PlayData playData = null;

        //�����Ϳ� ��⿡�� ����� ��ΰ� �ٸ���.
        //���� ó���Ѵ�.
        if (Application.isEditor)
        {
            playData = Json.LoadJsonFile<PlayData>(Application.dataPath, "Resources/PlayData/PlayData");
        }
        else
        {
            playData = Json.LoadJsonFile<PlayData>(Application.persistentDataPath, "PlayData");
        }

        if (playData == null)
        {
            //�ƹ� ������ ������ 
            //�⺻ �����͸� ����
            playData = new PlayData(true);
        }

        gunBenchAct = playData.gunBenchAct;

        Random.InitState(playData.randomSeed);

        return playData;
    }
}
