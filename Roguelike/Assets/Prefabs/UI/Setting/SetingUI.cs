using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetingUI : MonoBehaviour
{
    private bool showLetterbox = true;
    [SerializeField]
    private GameObject letterBox0;
    [SerializeField]
    private GameObject letterBox1;
    [SerializeField]
    private GameObject checkObj;

    [SerializeField]
    private CanvasGroup keyPadGroup;

    private void Awake()
    {
        ActLetterBox(showLetterbox);
    }

    public void ActLetterBox()
    {
        showLetterbox = !showLetterbox;
        ActLetterBox(showLetterbox);
    }

    public void ActLetterBox(bool pState)
    {
        if (pState)
        {
            Camera.main.transform.localPosition = new Vector3(0, -3.55f, -10);
            letterBox0.SetActive(true);
            letterBox1.SetActive(true);
            checkObj.SetActive(true);
            keyPadGroup.alpha = 1f;
        }
        else
        {
            Camera.main.transform.localPosition = new Vector3(0, 0, -10);
            letterBox0.SetActive(false);
            letterBox1.SetActive(false);
            checkObj.SetActive(false);
            keyPadGroup.alpha = 0.2f;
        }
    }
}
