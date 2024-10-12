using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum StartFrom
{
    start,
    end
}

public class ChangeScene : MonoBehaviour
{
    public static StartFrom startFrom = StartFrom.start;
    public string scene;

    public void StartNormal()
    {
        startFrom = StartFrom.start;
        SceneManager.LoadScene(scene);
    }

    public void StartAtEnd()
    {
        startFrom = StartFrom.end;
        SceneManager.LoadScene(scene);
    }
}
