using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class Level5LeverScript : MonoBehaviour
{
    private Animator leverAnim;

    private GameObject cutFocus;
    private CinemachineVirtualCamera vcam;
    private PlayableDirector leverDir;

    private PlayerMovement pmController;

    private bool playerEntered;

    void Start()
    {
        leverAnim = GetComponent<Animator>();

        pmController = GameObject.Find("Aashaa").GetComponent<PlayerMovement>();

        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        leverDir = GameObject.Find(this.name + " Cutscene").GetComponent<PlayableDirector>();
        cutFocus = GameObject.Find(this.name + " Focus");
    }

    void Update()
    {
        if (PlayerMovement.instance.interact && playerEntered && !leverAnim.GetBool("push"))
        {
            if (this.name.Substring(this.name.Length - 1) == "1")
            {
                StartCoroutine(playCutscene1());
            }
            else if (this.name.Substring(this.name.Length - 1) == "2")
            {
                StartCoroutine(playCutscene2());
            }

            AudioManagerScript.instance.Play("click");
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

    IEnumerator playCutscene1()
    {
        leverAnim.SetBool("push", true);

        Transform origFollow = vcam.Follow;
        vcam.Follow = cutFocus.transform;
        vcam.LookAt = cutFocus.transform;

        leverDir.Play();

        pmController.playerActive = false;

        yield return new WaitForSeconds(2.5f);

        CameraShakeScript.shake.shakeCamera(1.5f, 1.75f);

        yield return new WaitForSeconds(3f);

        vcam.Follow = origFollow;
        vcam.LookAt = origFollow;

        pmController.playerActive = true;
    }
    IEnumerator playCutscene2()
    {
        leverAnim.SetBool("push", true);

        Transform origFollow = vcam.Follow;
        vcam.Follow = cutFocus.transform;
        vcam.LookAt = cutFocus.transform;

        leverDir.Play();

        pmController.playerActive = false;

        yield return new WaitForSeconds(3f);

        CameraShakeScript.shake.shakeCamera(1.5f, 1.75f);

        yield return new WaitForSeconds(3.5f);

        vcam.Follow = origFollow;
        vcam.LookAt = origFollow;

        pmController.playerActive = true;
    }
}
