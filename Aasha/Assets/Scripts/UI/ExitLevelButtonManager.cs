using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevelButtonManager : MonoBehaviour
{
    public void exitButton()
    {
        UniversalScript.instance.SaveData();
        SceneManager.LoadSceneAsync(1);
    }
}
