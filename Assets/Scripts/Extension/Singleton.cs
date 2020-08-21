using UnityEngine;

namespace Extension
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null) return _instance;
                    if ((_instance = FindObjectOfType<T>()) != null) return _instance;
                    var gameObject = new GameObject("SINGLETON " + typeof(T));
                    _instance = gameObject.AddComponent<T>();
                    return _instance;
                }
            }
        }
    }
}