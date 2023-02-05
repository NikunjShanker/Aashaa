﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class UniversalScript : MonoBehaviour
{
    public static UniversalScript instance;

    public Vector3 savedPos;
    public int savedSceneIndex;
    public int maxHealth;
    public int currentHealth;
    public int deathCounter;

    public int bestTime;

    public float time;
    public string timePlaying;
    public string minutes;
    public string seconds;
    public string milliseconds;

    public string bestMinutes;
    public string bestSeconds;
    public string bestMilliseconds;

    public bool countTime;

    public bool[] heartGained = new bool[9];
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canDash;
    public bool jumpBoost;
    public bool speedBoost;

    public TextMeshProUGUI timerText;

    private float interpolationTime;
    private float mouseTime;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

        bestTime = 0;
        bestMinutes = "";
        bestSeconds = "";
        bestMilliseconds = "";

        interpolationTime = 4.0f;
        mouseTime = 0.0f;

        if (File.Exists(Application.persistentDataPath + "/game.aashafile"))
        {
            LoadGameData();
        }
        else
        {
            ResetData();
        }
    }

    public void ResetData()
    {
        savedPos = new Vector3(0, -1.8f, 0);
        savedSceneIndex = 1;
        deathCounter = 0;
        countTime = false;
        time = 0f;
        maxHealth = 3;
        currentHealth = maxHealth;
        canDoubleJump = canWallJump = canDash = jumpBoost = speedBoost = false;

        for (int i = 0; i < heartGained.Length; i++)
        {
            heartGained[i] = false;
        }
    }

    public void SaveData()
    {
        savedSceneIndex = SceneManager.GetActiveScene().buildIndex;
        maxHealth = PlayerHealth.ph.maxHealth;
        currentHealth = PlayerHealth.ph.health;
        canDoubleJump = CharacterController2D.instance.canDoubleJump;
        canWallJump = CharacterController2D.instance.canWallJump;
        canDash = CharacterController2D.instance.canDash;
        jumpBoost = CharacterController2D.instance.jumpIncrease;
        speedBoost = CharacterController2D.instance.speedIncrease;

        SaveSystem.SaveGame(this);
    }

    public void LoadGameData()
    {
        GameData data = SaveSystem.LoadPlayer();

        savedPos = new Vector3(data.savedPos[0], data.savedPos[1], data.savedPos[2]);
        time = data.time;
        bestTime = data.bestTime;
        bestMinutes = data.bestMinutes;
        bestSeconds = data.bestSeconds;
        bestMilliseconds = data.bestMilliseconds;
        savedSceneIndex = data.savedSceneIndex;
        maxHealth = data.maxHealth;
        currentHealth = data.currentHealth;
        deathCounter = data.deathCounter;
        heartGained = data.heartGained;
        canDoubleJump = data.canDoubleJump;
        canWallJump = data.canWallJump;
        canDash = data.canDash;
        jumpBoost = data.jumpBoost;
        speedBoost = data.speedBoost;
    }

    public void startTimer()
    {
        countTime = true;
    }

    public void pauseTimer()
    {
        countTime = false;
    }

    public void compareTimes()
    {
        int currentTime = int.Parse(milliseconds) + int.Parse(seconds) * 1000 + int.Parse(minutes) * 60000;

        if (currentTime < bestTime || bestTime == 0)
        {
            bestTime = currentTime;
            bestMinutes = minutes;
            bestSeconds = seconds;
            bestMilliseconds = milliseconds;
        }

        SaveData();
    }

    public string getBestTime()
    {
        return string.Join(",", bestMinutes, bestSeconds, bestMilliseconds);
    }

    void Update()
    {
        // delete both if statements before release
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            bestTime = 0;
            SaveData();
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadSceneAsync(10);
        }

        if (SceneManager.GetActiveScene().buildIndex >= 3 && SceneManager.GetActiveScene().buildIndex <= 9)
        {
            if (!countTime)
            {
                startTimer();
            }
        }
        else
        {
            if (countTime)
            {
                pauseTimer();
            }
        }

        if(countTime)
        {
            time += Time.deltaTime;

            if(GameObject.Find("/Text Canvas/Timer Text") != null)
            {
                timerText = GameObject.Find("/Text Canvas/Timer Text").GetComponent<TextMeshProUGUI>();

                minutes = Mathf.FloorToInt(time / 60).ToString();
                seconds = Mathf.FloorToInt(time % 60).ToString();
                milliseconds = (Mathf.FloorToInt(time * 120) % 100).ToString();

                minutes = twoSigFigs(minutes);
                seconds = twoSigFigs(seconds);
                milliseconds = twoSigFigs(milliseconds);

                timerText.text = minutes + ":" + seconds + ":" + milliseconds;
            }
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Cursor.visible = true;
        }

        mouseTime += Time.deltaTime;

        if (mouseTime >= interpolationTime)
        {
            mouseTime = mouseTime - interpolationTime;

            if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
            {
                Cursor.visible = false;
            }
        }
    }

    private string twoSigFigs(string amountOfTime)
    {
        if(amountOfTime.Length == 1)
        {
            amountOfTime = "0" + amountOfTime;
        }

        return amountOfTime;
    }
}