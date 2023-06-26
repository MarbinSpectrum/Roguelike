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

////////////////////////////////////////////////////////////////////////////////
/// : �������� ������ �����Ѵ�.
////////////////////////////////////////////////////////////////////////////////

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


    public static IEnumerator LoadNextMap()
    {
        ScreenDark.AnimationState(true);

        yield return new WaitForSeconds(1f);

        MapManager mapManager = MapManager.instance;

        jarMgr.RemoveAll_JarObj();
        chestMgr.RemoveAll_ChestObj();
        torchMgr.RemoveAll_TorchObj();
        itemMgr.RemoveAll_Item();
        trapMgr.RemoveAll_Trap();

        if(mapManager != null)
        {
            StageData stageData = instance.stageData[nowStage];
            uint stageMax = stageData.stageLength;              //���������� �ִ� ��
            int nowRoomIdx = mapManager.GetNowRoomIdx();        //���� ���� �ε���
            mapManager.AddVisitRoomIdx(nowRoomIdx);
            int stageCnt = mapManager.GetVisitRoomIdx().Count;  //�湮�� �� ����
            GameManager.SavePlayData();

            if(stageCnt < stageMax)
            {
                StageData nowStageData = instance.stageData[nowStage];
                string stageName = nowStageData.isSceneName;

                SceneManager.LoadScene(stageName);
            }
            else
            {
                StageName nextStageName = stageData.nextStage;
                StageData nextStageData = instance.stageData[nextStageName];
                nowStage = nextStageName;

                SceneManager.LoadScene(nextStageData.isSceneName);
            }
        }
    }
}
