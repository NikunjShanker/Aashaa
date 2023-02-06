using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class Level3LeverScript : MonoBehaviour
{
    private Animator leverAnim;

    private GameObject cutFocus;
    private CinemachineVirtualCamera vcam;
    private PlayableDirector leverDir;
    private PlayableDirector leverPushedDir;

    private PlayerMovement pmController;

    private bool playerEntered;

    void Start()
    {
        leverAnim = GetComponent<Animator>();

        pmController = GameObject.Find("Aashaa").GetComponent<PlayerMovement>();

        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        leverDir = GameObject.Find("Lever 1 Cutscene").GetComponent<PlayableDirector>();
        leverPushedDir = GameObject.Find("Lever 1 Pushed").GetComponent<PlayableDirector>();
        cutFocus = GameObject.Find(this.name + " Focus");

        if(UniversalScript.instance.cutsceneProgress > 1)
        {
            leverPushedDir.Play();
        }
    }

    void Update()
    {
        if (PlayerMovement.instance.interact && playerEntered && !leverAnim.GetBool("push") && UniversalScript.instance.cutsceneProgress <= 1)
        {
            AudioManagerScript.instance.Play("click");
            UniversalScript.instance.cutsceneProgress = 2;
            StartCoroutine(playCutscene());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerEntered = false;
        }
    }

    IEnumerator playCutscene()
    {
        leverAnim.SetBool("push", true);

        Transform origFollow = vcam.Follow;
        vcam.Follow = cutFocus.transform;
        vcam.LookAt = cutFocus.transform;

        leverDir.Play();

        pmController.playerActive = false;

        yield return new WaitForSeconds(2f);

        CameraShakeScript.shake.shakeCamera(1.5f,1.75f);

        yield return new WaitForSeconds(3.5f);

        vcam.Follow = origFollow;
        vcam.LookAt = origFollow;

        pmController.playerActive = true;
    }
}
