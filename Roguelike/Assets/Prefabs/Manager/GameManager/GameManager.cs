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
        SoundManager.StopBGM();

        LanguageManager languageManager = LanguageManager.instance;
        yield return languageManager.runLoadData(); //��� ������ �ε�

        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);

        MapManager mapManager = MapManager.instance;
        yield return mapManager.runCreateTileMap(); //�� ����

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterManager.CreateCatGirl(createPos.x, createPos.y); //ĳ���� ����

        EtcObjManager.CreateStartStairs(createPos);

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
        PlayData playData = new PlayData(InventoryManager.instance, CharacterManager.instance);
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
            playData = new PlayData();
        }

        return playData;
    }
}
