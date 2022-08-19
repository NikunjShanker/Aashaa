using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth ph;
    public CharacterController2D controller;

    public SpriteRenderer playerSprite;

    public int health;
    public int maxHealth;
    public bool dieOnce;

    void Awake()
    {
        if (ph == null) ph = this;

        playerSprite.color = new Color(1, 1, 1, 1);

        maxHealth = UniversalScript.instance.maxHealth;
        health = maxHealth;
        dieOnce = false;

        if (SceneManager.GetActiveScene().name == "Main Menu") maxHealth = 0;
    }

    public void removeHealth(int damage)
    {
        health -= damage;

        if(!dieOnce && playerSprite.color != new Color(0, 0, 0, 0))
        {
            if (health <= 0)
            {
                if (damage == 1 || PlayerMovement.instance.restart)
                {
                    HeartContainerManager.instance.loseHeartPartSys();
                    AudioManagerScript.instance.Play("blood splash");
                    PlayerMovement.instance.restart = false;
                }
                else AudioManagerScript.instance.Play("lava");

                dieOnce = true;
                StartCoroutine(PlayerAnimationController.instance.playerDied());
            }
            else
            {
                StartCoroutine(redTint());

                if (controller.jumpNum == 0) controller.jumpNum = 1;

                controller.PushBack();

                AudioManagerScript.instance.End("ouch");
                AudioManagerScript.instance.sounds[5].pitch = Random.Range(0.8f, 1.0f);
                AudioManagerScript.instance.Play("ouch");

                HeartContainerManager.instance.loseHeartPartSys();
            }
        }
    }

    public void heartContainerCollected()
    {
        StartCoroutine(gainHeart());
    }

    IEnumerator redTint()
    {
        playerSprite.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.3f);
        playerSprite.color = new Color(1, 1, 1, 1);
    }
    IEnumerator gainHeart()
    {
        HeartContainerManager.instance.gainHeartPartSys();

        yield return new WaitForSeconds(0.38f);

        maxHealth = 3;

        for(int i = UniversalScript.instance.heartGained.Length-1; i >= 0; i--)
        {
            if (UniversalScript.instance.heartGained[i])
                maxHealth++;
        }

        health = maxHealth;
    }
}
