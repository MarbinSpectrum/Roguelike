using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : ���ӿ� ��ü���� ��Ҹ� �����Ѵ�.
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
    /// : ���ӽ���
    ////////////////////////////////////////////////////////////////////////////////
    public void StartGame()
    {
        StartCoroutine(runStartGame());
    }
    public IEnumerator runStartGame()
    {
        LanguageManager languageManager = LanguageManager.instance;
        yield return languageManager.runLoadData(); //��� ������ �ε�

        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);


        MapManager mapManager = MapManager.instance;
        yield return mapManager.runCreateTileMap(); //�� ����

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterManager.CreateCatGirl(createPos.x, createPos.y); //ĳ���� ����

        BulletManager bulletManager = BulletManager.instance;
        yield return bulletManager.runCreateObj(); //�Ѿ� ��ü ����

        DamageEffect damageEffect = DamageEffect.instance;
        yield return damageEffect.runCreateObj(); //������ ����Ʈ ����

        GetGoldEffect getGoldEffect = GetGoldEffect.instance;
        yield return getGoldEffect.runCreateObj(); //���ȹ�� ����Ʈ ����

        MonsterManager monsterManager = MonsterManager.instance;
        yield return monsterManager.runCreateMonster(mapManager.GetMonsterList()); //���� ����

        JarManager jarManager = JarManager.instance;
        yield return jarManager.runCreateJarObj(); //�׾Ƹ� ����

        ChestManager chestManager = ChestManager.instance;
        yield return chestManager.runCreateChestObj(); //���� ����

        BGMObj.PlayBGM();

        totalUI.ShowCreateMap(false);

        mapManager.ShowMapName();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��� ����� Json���Ϸ� ��������.
    ////////////////////////////////////////////////////////////////////////////////
    [Button("Export Data", ButtonSizes.Large)]
    public void SaveGame()
    {
        PlayData playData = new PlayData();
        string jsonData = Json.ObjectToJson(playData);
        Json.CreateJsonFile(Application.dataPath,"Resources/PlayData/PlayData", jsonData);
    }
}
