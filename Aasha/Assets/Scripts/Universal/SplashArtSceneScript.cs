using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashArtSceneScript : MonoBehaviour
{
    

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKey) SceneManager.LoadSceneAsync(1);
    }
}
