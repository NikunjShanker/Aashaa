using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainerManager : MonoBehaviour
{
    public static HeartContainerManager instance;
    public PlayerHealth ph;
    public int health;
    public int maxHealth;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite goldHeart;

    public ParticleSystem gainHeartPS;
    public ParticleSystem loseHeartPS;

    void Awake()
    {
        if (instance == null)
            instance = this;

        ph = GameObject.Find("Aashaa").GetComponent<PlayerHealth>();
        gainHeartPS = GameObject.Find("Gain Heart PS").GetComponent<ParticleSystem>();
        loseHeartPS = GameObject.Find("Lose Heart PS").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        health = ph.health;
        maxHealth = ph.maxHealth;
        numOfHearts = ph.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            // Sprites of hearts

            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            // Number of hearts

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    public void loseHeartPartSys()
    {
        if(health > 0)
        {
            loseHeartPS.transform.position = hearts[health - 1].transform.position;

            if (loseHeartPS.isPlaying)
            {
                loseHeartPS.Stop();
                loseHeartPS.Play();
            }
            else
            {
                loseHeartPS.Play();
            }
        }
    }

    public void gainHeartPartSys()
    {
        gainHeartPS.transform.position = hearts[maxHealth].transform.position;

        if (gainHeartPS.isPlaying)
        {
            gainHeartPS.Stop();
            gainHeartPS.Play();
        }
        else
        {
            gainHeartPS.Play();
        }
    }
}
