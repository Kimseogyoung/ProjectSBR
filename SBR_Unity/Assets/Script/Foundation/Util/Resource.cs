using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

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

        static public Sprite Load(string imageName, string spriteName)
        {
            Sprite[] all = Resources.LoadAll<Sprite>(imageName);
            foreach (var s in all)
            {
                if (s.name == spriteName) return s;

            }
            return null;
        }
    }
}

