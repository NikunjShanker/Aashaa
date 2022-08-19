using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevelButtonManager : MonoBehaviour
{
    public void exitButton()
    {
        if(GameObject.Find("Aashaa") != null)
        {
            UniversalScript.instance.savedPos = GameObject.Find("Aashaa").transform.position;
        }

        UniversalScript.instance.SaveData();
        SceneManager.LoadSceneAsync(1);
    }
}
