﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SkipButtonManager : MonoBehaviour
{
    public void skipCutscene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Skip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            skipCutscene();
        }
    }
}
