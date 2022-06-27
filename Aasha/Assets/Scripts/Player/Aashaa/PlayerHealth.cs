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

        if(!dieOnce)
        {
            if (health <= 0)
            {
                if(damage == 1) HeartContainerManager.instance.loseHeartPartSys();

                dieOnce = true;
                StartCoroutine(PlayerAnimationController.instance.playerDied());
            }
            else
            {
                StartCoroutine(redTint());

                if (controller.jumpNum == 0) controller.jumpNum = 1;

                controller.PushBack();

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
