using UnityEngine;

public class SceneLoadInfo : MonoBehaviour
{
    public static bool LoadedFromSave = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
