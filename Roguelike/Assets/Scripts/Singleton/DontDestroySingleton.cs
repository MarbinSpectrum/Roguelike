using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class DontDestroySingleton<T> : Mgr where T : Mgr
{
    private static T Instance;
    public static T instance
    {
        get
        {
            if (Instance == null)
            {
                Type type = typeof(T);
                string str = string.Format("Manager/{0}", type.Name);
                T loadScript = Resources.Load<T>(str);
                Instance = Instantiate(loadScript);
                DontDestroyOnLoad(Instance);
            }
            return Instance;
        }
    }
}