using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LoadingUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI loadText;

    public void ActLoading(bool pState)
    {
        string loadStr = LanguageManager.GetText("LOAD");
        loadText.text = loadStr;
        gameObject.SetActive(pState);
    }
}
