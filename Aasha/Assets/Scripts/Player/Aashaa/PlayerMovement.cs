using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public Vector3 respawnPoint;

    public float runSpeed = 32f;

    private float horizontalMove = 0f;
    private bool jump = false;
    public bool dashCooldown;

    public bool playerActive;

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
            // Get direction pressed on keyboard
            horizontalMove = runSpeed * Input.GetAxisRaw("Horizontal");
            jump = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);

            if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K)) && !dashCooldown)
            {
                CharacterController2D.instance.Dash();
                dashCooldown = true;
                StartCoroutine(dashCountdown());
            }
        }
        else
        {
            horizontalMove = 0f;
            jump = false;
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
