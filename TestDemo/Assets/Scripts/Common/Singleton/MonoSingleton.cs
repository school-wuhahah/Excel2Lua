
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T mInstance = null;
    private static bool _applicationIsQuitting = false;
    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                return null;
            }
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType(typeof(T)) as T;
                if(mInstance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    mInstance = go.AddComponent<T>();
                    GameObject parent = GameObject.Find("Boot");
                    if (parent == null)
                    {
                        parent = new GameObject("Boot");
                        GameObject.DontDestroyOnLoad(parent);
                    }
                    //设置 该gameobject 为 parent 的子对象
                    if (parent != null)
                    {
                        go.transform.parent = parent.transform;
                    }
                }
            }

            return mInstance;
        }
    }


    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as T;
        }
        DontDestroyOnLoad(gameObject);
        Init();
    }

    protected virtual void Init()
    {

    }

    public virtual void Dispose()
    {

    }

}
