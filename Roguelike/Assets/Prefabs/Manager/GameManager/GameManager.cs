using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
        StartCoroutine(runStartGamep());
    }
    public IEnumerator runStartGamep()
    {
        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowCreateMap(true);

        MapManager mapManager = MapManager.instance;
        yield return mapManager.runCreateTileMap(); //맵 생성

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int createPos = mapManager.GetRandomStartPos();
        characterManager.CreateCatGirl(createPos.x, createPos.y); //캐릭터 생성

        MonsterManager monsterManager = MonsterManager.instance;
        yield return monsterManager.runCreateMonster(mapManager.GetMonsterList()); //맵 생성

        JarManager jarManager = JarManager.instance;
        yield return jarManager.runCreateJarObj(); //항아리 생성

        totalUI.ShowCreateMap(false);
    }
}
