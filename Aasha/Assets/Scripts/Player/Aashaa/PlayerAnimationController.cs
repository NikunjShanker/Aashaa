using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController instance;
    private PlayerMovement pm;

    private SpriteRenderer player;
    private Color normalPlayerColor;

    private ParticleSystem bloodPS;

    private TextMeshProUGUI deathCounterText;

    private PlayableDirector fadeOut;
    private PlayableDirector fadeIn;

    [SerializeField] private Animator playerAnim;

    private void Start()
    {
        if(instance == null)
            instance = this;

        pm = this.gameObject.GetComponent<PlayerMovement>();

        player = GameObject.Find("Aashaa").GetComponent<SpriteRenderer>();
        normalPlayerColor = player.color;

        bloodPS = GameObject.Find("/Aashaa/Blood PS").GetComponent<ParticleSystem>();

        if(SceneManager.GetActiveScene().name.Substring(0,5) == "Level")
        {
            deathCounterText = GameObject.Find("Death Counter Text").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            deathCounterText = null;
        }

        fadeOut = GameObject.Find("/Timeline/Fade Out").GetComponentInChildren<PlayableDirector>();
        fadeIn = GameObject.Find("/Timeline/Fade In").GetComponentInChildren<PlayableDirector>();
    }

    private void Update()
    {
        if(deathCounterText!=null) deathCounterText.text = "Deaths: " + UniversalScript.instance.deathCounter;
    }

    public void setRun(bool r)
    {
        playerAnim.SetBool("run", r);
    }

    public void setJump(bool j)
    {
        playerAnim.SetBool("jump", j);
    }

    public IEnumerator playerDied()
    {
        fadeIn.Stop();
        fadeOut.Play();
        bloodPS.Play();

        pm.playerActive = false;
        player.color = new Color(0, 0, 0, 0);

        yield return new WaitForSeconds(1f);

        pm.playerActive = true;
        player.color = normalPlayerColor;
        pm.SpawnPlayer();

        PlayerHealth.ph.health = PlayerHealth.ph.maxHealth;
        fadeIn.Play();
        UniversalScript.instance.deathCounter++;
        PlayerHealth.ph.dieOnce = !PlayerHealth.ph.dieOnce;
    }
}
