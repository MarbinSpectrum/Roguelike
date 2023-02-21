using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
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
        yield return jarManager.runCreateJarObj(); //항아리 생성

        ChestManager chestManager = ChestManager.instance;
        yield return chestManager.runCreateChestObj(); //상자 생성

        BGMObj.PlayBGM();

        totalUI.ShowCreateMap(false);

        mapManager.ShowMapName();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이 기록을 Json파일로 내보낸다.
    ////////////////////////////////////////////////////////////////////////////////
    [Button("Export Data", ButtonSizes.Large)]
    public void SaveGame()
    {
        PlayData playData = new PlayData();
        string jsonData = Json.ObjectToJson(playData);
        Json.CreateJsonFile(Application.dataPath,"Resources/PlayData/PlayData", jsonData);
    }
}
