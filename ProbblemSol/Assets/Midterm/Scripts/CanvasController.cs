using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{

    public void Restart()
    {
        SceneManager.LoadScene("JailBreak");
        Time.timeScale = 1f;
    }

    
}
