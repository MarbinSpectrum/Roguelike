using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
        StartCoroutine(runStartGamep());
    }
    public IEnumerator runStartGamep()
    {
        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);

        MapManager mapManager = MapManager.instance;
        yield return mapManager.runCreateTileMap(); //�� ����

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterManager.CreateCatGirl(createPos.x, createPos.y); //ĳ���� ����

        MonsterManager monsterManager = MonsterManager.instance;
        yield return monsterManager.runCreateMonster(mapManager.GetMonsterList()); //�� ����

        totalUI.ShowCreateMap(false);
    }
}
