
namespace Util
{
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

        static public T GetOrAddComponent<T>(UnityGameObject gameObject) where T : UnityEngine.Component
        {
            T result;

            if(!gameObject.TryGetComponent<T>(out result))
                result = gameObject.AddComponent<T>();

            return result;
        }

        public static T FindChild<T>(UnityGameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null)
                return null;

            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    UnityEngine.Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }

            return null;
        }

        public static UnityGameObject FindChild(UnityGameObject go, string name = null, bool recursive = false)
        {
            UnityEngine.Transform result = FindChild<UnityEngine.Transform>(go, name, recursive);
            if (result == null)
                return null;

            return result.gameObject;
        }


        public static T[] FindChildren<T>(UnityGameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<T>();
        }
    }

}
