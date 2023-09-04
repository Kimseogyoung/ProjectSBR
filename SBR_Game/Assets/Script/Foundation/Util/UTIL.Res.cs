
using UnityEngine;


    
static public partial class UTIL 
{
    static public T Instantiate<T>(string path, Vector3 pos, Transform parent = null) where T : Component
    {
        GameObject obj = Instantiate(path, pos, parent);
        return obj.AddGetComponent<T>();
    }

    static public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject obj = GameObject.Instantiate(UTIL.Load<GameObject>(path));
        if (parent != null)
            obj.transform.SetParent(parent);
        return obj;
    }
    static public GameObject Instantiate(string path, Vector3 pos, Transform parent = null)
    {
        GameObject obj = GameObject.Instantiate(UTIL.Load<GameObject>(path));
        if (parent != null)
            obj.transform.SetParent(parent);
        obj.transform.position = pos;

        return obj;
    }

    static public T Load<T>(string path) where T : Object
    {
        T obj = Resources.Load<T>(path);
        GameLogger.D("{0} Ÿ�� {1}��ο��� Load {2}", typeof(T), path, obj);
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

