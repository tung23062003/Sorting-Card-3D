using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<T>();
            return;
        }
        if (_instance.GetInstanceID() != this.gameObject.GetInstanceID())
            Debug.LogError("Singleton is not unique!");
        Destroy(this.gameObject);
    }
}
