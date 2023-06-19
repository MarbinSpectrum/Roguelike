using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using MyLib;

public struct StageData
{
    public string isSceneName;          //�������� �̸�
    public uint stageLength;            //�������� ����
    public StageName nextStage;         //���� ��������
}

public class StageManager : DontDestroySingleton<StageManager>
{
    public StageName firstStage;

    public Dictionary<StageName, StageData> stageData = new Dictionary<StageName, StageData>();

    private bool initData = false;

    public void Init()
    {
        if (initData)
            return;
        initData = true;
        LoadFirstStage();
    }

    public void LoadFirstStage()
    {
        nowStage = firstStage;
        StageData stageData = instance.stageData[nowStage];
        SceneManager.LoadScene(stageData.isSceneName);
    }

    public static StageName nowStage;

    public static IEnumerator LoadNextScene()
    {
        GameManager.SavePlayData();
        ScreenDark.AnimationState(true);

        yield return new WaitForSeconds(1f);

        JarManager jarManager = JarManager.instance;
        ChestManager chestManager = ChestManager.instance;
        TorchManager torchManager = TorchManager.instance;

        jarManager.RemoveAll_JarObj();
        chestManager.RemoveAll_ChestObj();
        torchManager.RemoveAll_TorchObj();


        StageData stageData = instance.stageData[nowStage];
        StageName nextStageName = stageData.nextStage;
        StageData nextStageData = instance.stageData[nextStageName];
        nowStage = nextStageName;

        SceneManager.LoadScene(nextStageData.isSceneName);
    }


}
