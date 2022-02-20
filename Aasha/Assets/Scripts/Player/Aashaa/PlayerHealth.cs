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

    void Awake()
    {
        if (ph == null) ph = this;

        playerSprite.color = new Color(1, 1, 1, 1);

        maxHealth = UniversalScript.instance.maxHealth;
        health = maxHealth;

        if (SceneManager.GetActiveScene().name == "Main Menu") maxHealth = 0;
    }

    public void removeHealth(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            StartCoroutine(PlayerAnimationController.instance.playerDied());
        }
        else
        {
            StartCoroutine(redTint());
            
            if(controller.jumpNum == 0) controller.jumpNum = 1;

            controller.PushBack();

            HeartContainerManager.instance.loseHeartPartSys();
        }
    }

    public void increaseHealth(int amount)
    {
        health += amount;
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

        if (maxHealth < 12) maxHealth++;

        yield return new WaitForSeconds(0.38f);

        health = maxHealth;
    }
}
