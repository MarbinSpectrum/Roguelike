using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using System.IO;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 게임에 전체적인 요소를 관리한다.
////////////////////////////////////////////////////////////////////////////////
public class GameManager : FieldObjectSingleton<GameManager>
{
    ////////////////////////////////////////////////////////////////////////////////
    /// : Start
    ////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        StartGame();
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
        SoundManager.StopBGM();

        LanguageManager languageManager = LanguageManager.instance;
        yield return languageManager.runLoadData(); //언어 데이터 로드

        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);

        MapManager mapManager = MapManager.instance;
        yield return mapManager.runCreateTileMap(); //맵 생성

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterManager.CreateCatGirl(createPos.x, createPos.y); //캐릭터 생성

        EtcObjManager.CreateStartStairs(createPos);

        BulletManager bulletManager = BulletManager.instance;
        yield return bulletManager.runCreateObj(); //총알 객체 생성

        DamageEffect damageEffect = DamageEffect.instance;
        yield return damageEffect.runCreateObj(); //데미지 이펙트 생성

        GetGoldEffect getGoldEffect = GetGoldEffect.instance;
        yield return getGoldEffect.runCreateObj(); //골드획득 이펙트 생성

        MonsterManager monsterManager = MonsterManager.instance;
        yield return monsterManager.runCreateMonster(mapManager.GetMonsterList()); //몬스터 생성

        JarManager jarManager = JarManager.instance;
        yield return jarManager.runCreateJarObj(); //항아리 생성

        ChestManager chestManager = ChestManager.instance;
        yield return chestManager.runCreateChestObj(); //상자 생성

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
        PlayData playData = new PlayData(InventoryManager.instance, CharacterManager.instance);
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
            playData = new PlayData();
        }

        return playData;
    }
}
