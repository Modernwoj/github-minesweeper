using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonFromPrefab<T> : MonoBehaviour where T : Component
{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if (!_Instance)
            {
                string path = "Prefabs/Singletons/" + typeof(T).ToString();
                Debug.Log(path);
                var prefab = Resources.Load<GameObject>(path);
                Debug.Log(prefab);
                var obj = Instantiate(prefab);
                _Instance = obj.GetComponentInChildren<T>();
                obj.name = typeof(T).ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    public virtual void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
