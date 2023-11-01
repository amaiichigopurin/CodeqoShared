using System.Linq;
using UnityEngine;

namespace Codeqo
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                    if (_instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(T) + " is needed in the resources folder, but there is none.");
                    }
                }
                return _instance;
            }
        }

        protected virtual void OnEnable()
        {
            if (_instance == null)
            {
                _instance = (T)(object)this;
            }
            else if (_instance != this)
            {
                Debug.LogError("Multiple instances of " + typeof(T) + " found!");
            }
        }
    }
}