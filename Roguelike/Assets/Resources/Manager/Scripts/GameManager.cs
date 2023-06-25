using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using System.IO;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 게임에 전체적인 요소를 관리한다.
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
    /// : 게임시작
    ////////////////////////////////////////////////////////////////////////////////
    public void StartGame()
    {
        StartCoroutine(runStartGame());
    }

    public IEnumerator runStartGame()
    {
        PlayData = LoadPlayData();   //플레이 데이터 로드

        SoundManager.StopBGM();

        yield return languageMgr.runLoadData(); //언어 데이터 로드

        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);

        MapManager mapManager = MapManager.instance;
        mapManager.SetVisitRoomIdx(playData.visitRoomIdx);
        yield return mapManager.runCreateTileMap(); //맵 생성

        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterMgr.CreateCatGirl(createPos.x, createPos.y); //캐릭터 생성

        //시작위치에 계단을 생성
        EtcObjManager.CreateEtcObj(createPos, Obj.StartPos);

        yield return bulletMgr.runCreateObj(); //총알 객체 생성

        DamageEffect damageEffect = uIEffectMgr.damageEffect;
        yield return damageEffect.runCreateObj(); //데미지 이펙트 생성

        GetGoldEffect getGoldEffect = uIEffectMgr.getGoldEffect;
        yield return getGoldEffect.runCreateObj(); //골드획득 이펙트 생성

        yield return monsterMgr.runCreateMonster(mapManager.GetMonsterList()); //몬스터 생성

        yield return jarMgr.runCreateJarObj(); //항아리 생성

        yield return chestMgr.runCreateChestObj(); //상자 생성

        shopMgr.CreateShopItem();   //상점 아이템 생성

        ScreenDark.AnimationState(false); //화면이 서서히 나타남
        totalUI.ShowCreateMap(false);
        yield return new WaitForSeconds(0.5f);

        mapManager.ShowMapName();
        yield return new WaitForSeconds(1f);
        StageBGM.PlayBgm();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이 기록을 Json파일로 내보낸다.
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
    /// : Json파일로 저장된 플레이 기록을 읽는다.
    ////////////////////////////////////////////////////////////////////////////////
    public static PlayData LoadPlayData()
    {
        PlayData playData = null;

        //에디터와 기기에서 실행시 경로가 다르다.
        //따로 처리한다.
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
            //아무 정보도 없으니 
            //기본 데이터를 생성
            playData = new PlayData(true);
        }

        gunBenchAct = playData.gunBenchAct;

        Random.InitState(playData.randomSeed);

        return playData;
    }
}
