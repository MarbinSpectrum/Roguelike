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
    [SerializeField]
    private PlayBGMObj BGMObj;

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
        LanguageManager languageManager = LanguageManager.instance;
        yield return languageManager.runLoadData(); //언어 데이터 로드

        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);

        MapManager mapManager = MapManager.instance;
        yield return mapManager.runCreateTileMap(); //맵 생성

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterManager.CreateCatGirl(createPos.x, createPos.y); //캐릭터 생성

        BulletManager bulletManager = BulletManager.instance;
        yield return bulletManager.runCreateObj(); //총알 객체 생성

        DamageEffect damageEffect = DamageEffect.instance;
        yield return damageEffect.runCreateObj(); //데미지 이펙트 생성

        GetGoldEffect getGoldEffect = GetGoldEffect.instance;
        yield return getGoldEffect.runCreateObj(); //골드획득 이펙트 생성

        MonsterManager monsterManager = MonsterManager.instance;
        yield return monsterManager.runCreateMonster(mapManager.GetMonsterList()); //몬스터 생성

        JarManager jarManager = JarManager.instance;
        jarManager.RemoveAll_JarObj();
        yield return jarManager.runCreateJarObj(); //항아리 생성

        ChestManager chestManager = ChestManager.instance;
        chestManager.RemoveAll_ChestObj();
        yield return chestManager.runCreateChestObj(); //상자 생성

        BGMObj.PlayBGM();

        totalUI.ShowCreateMap(false);

        mapManager.ShowMapName();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이 기록을 Json파일로 내보낸다.
    ////////////////////////////////////////////////////////////////////////////////
    [Button("SaveData", ButtonSizes.Large)]
    public static void SavePlayData()
    {
        PlayData playData = new PlayData(InventoryManager.instance, CharacterManager.instance);
        string jsonData = Json.ObjectToJson(playData);

        if (Application.isEditor)
            Json.CreateJsonFile(Application.dataPath, "Resources/PlayData/PlayData", jsonData);
        else
            Json.CreateJsonFile(Application.persistentDataPath, "PlayData", jsonData);
    }

    public static PlayData LoadPlayData()
    {
        if (Application.isEditor)
        {
            string filePath = Application.dataPath + "/Resources/PlayData/PlayData.json";
            if (File.Exists(filePath))
            {
                PlayData playData = Json.LoadJsonFile<PlayData>(Application.dataPath, "Resources/PlayData/PlayData");
                return playData;
            }
        }
        else
        {
            string filePath = Application.persistentDataPath + "/PlayData.json";
            if (File.Exists(filePath))
            {
                PlayData playData = Json.LoadJsonFile<PlayData>(Application.persistentDataPath, "PlayData");
                return playData;
            }
        }

        //아무 정보도 없으니 
        //기본 데이터를 생성
        return new PlayData();
    }
}
