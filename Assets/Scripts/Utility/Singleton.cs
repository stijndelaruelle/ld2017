using UnityEngine;
using System.Collections;

namespace Sjabloon
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T m_Instance;
        public static T Instance
        {
            get
            {
                //if (_instance == null)
                //{
                //    Debug.LogError("The singleton " + typeof(T).FullName + " doesn't have an instance yet!");
                //}

                return m_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_Instance != null)
            {
                Debug.LogError("Trying to create 2 instances of the " + typeof(T).FullName + " singleton! Existing Object: " + m_Instance.gameObject.name + " Failed object: " + gameObject.name);
                Destroy(gameObject);
                return;
            }

            m_Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (m_Instance == this)
                m_Instance = null;
        }
    }
}
