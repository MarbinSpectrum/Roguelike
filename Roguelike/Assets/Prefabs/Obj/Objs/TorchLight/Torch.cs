using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField]
    private GameObject upTorch;
    [SerializeField]
    private GameObject leftTorch;
    [SerializeField]
    private GameObject rightTorch;
    [SerializeField]
    private GameObject downTorch;

    public void ActUpTorch()
    {
        upTorch.SetActive(true);
        leftTorch.SetActive(false);
        rightTorch.SetActive(false);
        downTorch.SetActive(false);
    }

    public void ActDownTorch()
    {
        upTorch.SetActive(false);
        leftTorch.SetActive(false);
        rightTorch.SetActive(false);
        downTorch.SetActive(true);
    }

    public void ActRightTorch()
    {
        upTorch.SetActive(false);
        leftTorch.SetActive(false);
        rightTorch.SetActive(true);
        downTorch.SetActive(false);
    }

    public void ActLeftTorch()
    {
        upTorch.SetActive(false);
        leftTorch.SetActive(true);
        rightTorch.SetActive(false);
        downTorch.SetActive(false);
    }
}
