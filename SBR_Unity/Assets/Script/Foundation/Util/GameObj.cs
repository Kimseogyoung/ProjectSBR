
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

    }

}
