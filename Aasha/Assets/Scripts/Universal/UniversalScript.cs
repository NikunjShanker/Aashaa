using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;
using UnityEngine.SceneManagement;

public class UniversalScript : MonoBehaviour
{
    public static UniversalScript instance;

    public Vector3 savedPos;
    public int savedSceneIndex;
    public int maxHealth;
    public int deathCounter;
    public bool[] heartGained = new bool[9];
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canDash;
    public bool jumpBoost;
    public bool speedBoost;

    private PlayableDirector fadeIn;

    void Awake()
    {
        if (instance == null) instance = this;

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

    private void Update()
    {
        // Remove the following when game is finished and ready to be released
        if (Input.GetKey(KeyCode.Alpha4)) SceneManager.LoadSceneAsync(4); savedPos = new Vector3(0, -1.8f, 0);
        if (Input.GetKey(KeyCode.Alpha5)) SceneManager.LoadSceneAsync(5); savedPos = new Vector3(0, -1.8f, 0);
        if (Input.GetKey(KeyCode.Alpha6)) SceneManager.LoadSceneAsync(6); savedPos = new Vector3(0, -1.8f, 0);
        if (Input.GetKey(KeyCode.Alpha7)) SceneManager.LoadSceneAsync(7); savedPos = new Vector3(0, -1.8f, 0);
        if (Input.GetKey(KeyCode.Alpha8)) SceneManager.LoadSceneAsync(8); savedPos = new Vector3(0, -1.8f, 0);
        if (Input.GetKey(KeyCode.Alpha9)) SceneManager.LoadSceneAsync(9); savedPos = new Vector3(0, -1.8f, 0);
    }

    public void ResetData()
    {
        savedPos = new Vector3(0, -1.8f, 0);
        savedSceneIndex = 1;
        deathCounter = 0;
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
}