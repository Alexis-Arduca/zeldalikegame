using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private float fixedDeltaTime;
    public List<GameObject> objectToDeactivate;

    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;

        foreach (GameObject obj in objectToDeactivate) { obj.SetActive(false); }
    }

    public void QuitMenu()
    {
        foreach (GameObject obj in objectToDeactivate) { obj.SetActive(true); }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
