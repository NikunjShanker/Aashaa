using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalScript : MonoBehaviour
{
    public static UniversalScript instance;

    public Vector3 savedPos;
    public int savedSceneIndex;
    public int maxHealth;
    public bool[] heartGained = new bool[9];
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canDash;
    public bool jumpBoost;
    public bool speedBoost;

    void Awake()
    {
        if (instance == null) instance = this;

        DontDestroyOnLoad(this.gameObject);

        ResetData();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && SceneManager.GetActiveScene().name != "Main Menu") PlayerHealth.ph.removeHealth(PlayerHealth.ph.maxHealth);

        if (Input.GetKey(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                savedPos = PlayerMovement.instance.transform.position;
                SceneManager.LoadSceneAsync(1);
            }
        }

        // Remove the following when game is finished and ready to be released
        if (Input.GetKey(KeyCode.Alpha5)) SceneManager.LoadSceneAsync(5);
        if (Input.GetKey(KeyCode.Alpha6)) SceneManager.LoadSceneAsync(6);
        if (Input.GetKey(KeyCode.Alpha4)) SceneManager.LoadSceneAsync(4);
    }

    public void ResetData()
    {
        savedPos = new Vector3(0, -1.8f, 0);
        savedSceneIndex = 1;
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
    }
}