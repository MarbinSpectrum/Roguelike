using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBGM : FieldObjectSingleton<StageBGM>
{
    public PlayBGMObj BGMObj;

    public static void PlayBgm()
    {
        if (instance == null || instance.BGMObj == null)
            return;
        instance.BGMObj.PlayBGM();
    }
}
