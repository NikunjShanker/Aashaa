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
    public int deathCounter;

    public float time;
    public string timePlaying;

    public bool countTime;

    public bool[] heartGained = new bool[9];
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canDash;
    public bool jumpBoost;
    public bool speedBoost;

    public TextMeshProUGUI timerText;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

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
        savedSceneIndex = data.savedSceneIndex;
        maxHealth = data.maxHealth;
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

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
            startTimer();

        if(countTime)
        {
            time += Time.deltaTime;
        }

        if(GameObject.Find("/Text Canvas/Timer Text") != null)
        {
            timerText = GameObject.Find("/Text Canvas/Timer Text").GetComponent<TextMeshProUGUI>();
            timerText.text = Mathf.FloorToInt(time / 60) + ":" + Mathf.FloorToInt(time % 60) + ":" + Mathf.FloorToInt(time * 120) % 100;
        }
    }
}