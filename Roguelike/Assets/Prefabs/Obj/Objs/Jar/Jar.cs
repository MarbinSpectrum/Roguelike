using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jar : MonoBehaviour
{
    public Vector2Int pos;
    public GameObject body;
    public GameObject broken;

    public void RemoveJarObj()
    {
        JarManager jarManager = JarManager.instance;
        body.SetActive(false);
        broken.SetActive(true);
        jarManager.RemoveJarObj(pos);
    }
}
