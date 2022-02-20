using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        if (instance == null) instance = this;

        playerActive = true;
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            respawnPoint = UniversalScript.instance.savedPos;
            SpawnPlayer();
        }
    }

    void Update()
    {
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
            PlayerAnimationController.instance.setRun(true);
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
        if (context.performed && playerActive && !dashCooldown)
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
            PlayerHealth.ph.removeHealth(PlayerHealth.ph.maxHealth);
        }
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.performed && SceneManager.GetActiveScene().buildIndex != 1)
        {
            UniversalScript.instance.savedPos = instance.transform.position;
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
