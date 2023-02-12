using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    
    static public class Resource 
    {
        static public GameObject Instantiate(string path)
        {
            GameObject obj = Resource.Load<GameObject>(path);
            return GameObject.Instantiate(obj);
        }
        static public T Load<T>(string path) where T : Object
        {
            T obj = Resources.Load<T>(path);
            GameLogger.NotImp("{0} 타입 {1}경로에서 Load", typeof(T), path);
            return obj;
        }
    }
}

