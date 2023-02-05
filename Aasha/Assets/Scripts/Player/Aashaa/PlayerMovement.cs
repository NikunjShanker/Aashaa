using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public Vector3 respawnPoint;

    public float runSpeed = 32f;

    private float horizontalMove = 0f;
    private bool jump = false;
    public bool dashCooldown;

    public bool playerActive;
    public bool interact;
    public bool restart;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        playerActive = true;

        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            respawnPoint = UniversalScript.instance.savedPos;
            SpawnPlayer();
        }
    }

    void Update()
    {
        // disable player controls if cutscene is playing
        if (GameObject.Find("Beginning Cutscene") != null)
        {
            if (GameObject.Find("Beginning Cutscene").GetComponent<PlayableDirector>().state == PlayState.Playing || GameObject.Find("Aashaa Armor Cutscene").GetComponent<PlayableDirector>().state == PlayState.Playing)
            {
                playerActive = false;
            }
            else
            {
                playerActive = true;
            }
        }

        if (playerActive)
        {
            // Get direction
            horizontalMove = runSpeed * Input.GetAxisRaw("Horizontal");
        }
        else
        {
            horizontalMove = 0f;
        }
    }

    void FixedUpdate()
    {
        if (horizontalMove != 0)
        {
            PlayerAnimationController.instance.setRun(true);
        }
        else
        {
            PlayerAnimationController.instance.setRun(false);
        }

        // Move Aasha
        CharacterController2D.instance.Move(horizontalMove * Time.fixedDeltaTime, jump);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && playerActive)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interact = true;
        }
        else
        {
            interact = false;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started && playerActive && !dashCooldown)
        {
            CharacterController2D.instance.Dash();
            dashCooldown = true;
            StartCoroutine(dashCountdown());
        }
    }

    public void Restart(InputAction.CallbackContext context)
    {
        if (context.performed && SceneManager.GetActiveScene().name != "Main Menu")
        {
            restart = true;
            PlayerHealth.ph.removeHealth(PlayerHealth.ph.maxHealth);
        }
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.performed && SceneManager.GetActiveScene().buildIndex != 1)
        {
            UniversalScript.instance.SaveData();
            SceneManager.LoadSceneAsync(1);
        }
    }

    public void SpawnPlayer()
    {
        this.transform.position = respawnPoint;
    }

    IEnumerator dashCountdown()
    {
        yield return new WaitForSeconds(0.5f);
        dashCooldown = false;
    }
}
