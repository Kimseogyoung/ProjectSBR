
namespace Util
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityGameObject = UnityEngine.GameObject;
    static public class GameObj 
    {
        static public void Destroy(UnityGameObject gameObject)
        {
            UnityGameObject.Destroy(gameObject.gameObject);
        }

        static public T GetComponent<T>(UnityGameObject gameObject)
        {
            return gameObject.GetComponent<T>();
        }

        static public T GetOrAddComponent<T>(UnityGameObject gameObject)
        {
            T result;

            if(!gameObject.TryGetComponent<T>(out result))
            {
                gameObject.AddComponent(typeof(T));
                result = gameObject.GetComponent<T>();
            }

            return result;
        }

        static public T FindChild<T>(UnityGameObject gameObject, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (recursive)
            {
                foreach(T value in gameObject.GetComponentsInChildren<T>())
                {
                    if (value.name == name)
                        return value;
                }  
            }
            else
            {
                for(int i=0; i<gameObject.transform.childCount; i++)
                {
                    T value = GetComponent<T>(gameObject.transform.GetChild(i).gameObject);
                    if (value != null && value.name == name)
                        return value;
                }
            }
            return null;
        }

        static public UnityGameObject FindChild(UnityGameObject gameObject, string name = null, bool recursive = false)
        {
            UnityEngine.Transform transform = FindChild<Transform>(gameObject, name, recursive);
            if(transform == null)
                return null;
            
            return transform.gameObject;
        }

        static public T[] FindChildAll<T>(UnityGameObject gameObject, string name = null) where T : UnityEngine.Object
        {
            List<T> objects= new List<T>();
            foreach (T value in gameObject.GetComponentsInChildren<T>())
            {
                if (value.name.StartsWith(name))
                    objects.Add(value);
            }
            return objects.ToArray();
        }

        static public UnityGameObject[] FindChildAll(UnityGameObject gameObject, string name = null)
        { 
            Transform[] transforms = FindChildAll<Transform>(gameObject, name);
            if (transforms == null || transforms.Length == 0)
                return null;

            UnityGameObject[] objects = new UnityGameObject[transforms.Length];
            for(int i=0; i < transforms.Length; i++)
                objects[i] = transforms[i].gameObject;

            return objects;
        }

        static public bool TryFind<T>(out T result, string name) where T : UnityEngine.Object
        {
            result = Find<T>(name);

            if (result == null)
                return false;
            return true;
        }
        static public T Find<T>(string name) where T : UnityEngine.Object
        {
            return UnityGameObject.FindObjectsOfType<T>().Where(e => e.name == name).FirstOrDefault();
        }

    }

}
