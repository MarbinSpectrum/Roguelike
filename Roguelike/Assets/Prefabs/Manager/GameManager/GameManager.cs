using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using System.IO;
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
        jarManager.RemoveAll_JarObj();
        yield return jarManager.runCreateJarObj(); //�׾Ƹ� ����

        ChestManager chestManager = ChestManager.instance;
        chestManager.RemoveAll_ChestObj();
        yield return chestManager.runCreateChestObj(); //���� ����

        BGMObj.PlayBGM();

        totalUI.ShowCreateMap(false);

        mapManager.ShowMapName();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��� ����� Json���Ϸ� ��������.
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

        //�ƹ� ������ ������ 
        //�⺻ �����͸� ����
        return new PlayData();
    }
}
