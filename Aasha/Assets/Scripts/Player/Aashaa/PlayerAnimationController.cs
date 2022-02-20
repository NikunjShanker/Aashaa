using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController instance;
    private PlayerMovement pm;

    private SpriteRenderer player;
    private Color normalPlayerColor;

    private ParticleSystem bloodPS;

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

        fadeOut = GameObject.Find("/Timeline/Fade Out").GetComponentInChildren<PlayableDirector>();
        fadeIn = GameObject.Find("/Timeline/Fade In").GetComponentInChildren<PlayableDirector>();
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
    }
}
