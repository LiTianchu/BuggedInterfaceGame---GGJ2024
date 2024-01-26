using UnityEngine;

//This class handles logic of creating singleton object
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    //load singleton into instance
                    _instance = new GameObject().AddComponent<T>();
                    _instance.name = typeof(T).ToString();
                    Debug.Log("Loaded New Manager");
                }
                else
                {
                    Debug.Log("Loaded Old Manager");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //if there is already a instance, destroy
        if (_instance != null)
        {
            Destroy(this.gameObject);

        }
        else
        {
            _instance = this as T;
        }


    }



}
