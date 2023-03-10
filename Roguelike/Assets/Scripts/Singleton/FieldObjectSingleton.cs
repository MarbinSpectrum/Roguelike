using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class FieldObjectSingleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
    public static T instance = null;
    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = gameObject.GetComponent<T>();
        }
        else
        {
            if (gameObject.GetComponent<T>())
                Destroy(gameObject);
        }
    }
}