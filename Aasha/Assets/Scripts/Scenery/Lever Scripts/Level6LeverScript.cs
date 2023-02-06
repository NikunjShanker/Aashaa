using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class Level6LeverScript : MonoBehaviour
{
    public static Level6LeverScript instance;

    private Animator leverAnim;

    private GameObject cutFocus;
    private CinemachineVirtualCamera vcam;
    private PlayableDirector leverDir;
    private PlayableDirector leverPushedDir;

    private PlayerMovement pmController;

    private bool playerEntered;
    public int cutSceneProgress;

    void Start()
    {
        if (instance == null) instance = this;

        leverAnim = GetComponent<Animator>();

        pmController = GameObject.Find("Aashaa").GetComponent<PlayerMovement>();
        cutSceneProgress = UniversalScript.instance.level6CutsceneProgress;

        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        leverDir = GameObject.Find(this.name + " Cutscene").GetComponent<PlayableDirector>();
        cutFocus = GameObject.Find(this.name + " Focus");

        if (cutSceneProgress % 2 == 1)
        {
            leverPushedDir = GameObject.Find("Lever 1 Pushed").GetComponent<PlayableDirector>();
            leverPushedDir.Play();
        }
        else if (cutSceneProgress / 10 % 2 == 1)
        {
            leverPushedDir = GameObject.Find("Lever 2 Pushed").GetComponent<PlayableDirector>();
            leverPushedDir.Play();
        }
        else if (cutSceneProgress / 100 % 2 == 1)
        {
            leverPushedDir = GameObject.Find("Lever 3 Pushed").GetComponent<PlayableDirector>();
            leverPushedDir.Play();
        }
        else if (cutSceneProgress / 1000 % 2 == 1)
        {
            leverPushedDir = GameObject.Find("Lever 4 Pushed").GetComponent<PlayableDirector>();
            leverPushedDir.Play();
        }
    }

    void Update()
    {
        if (PlayerMovement.instance.interact && playerEntered && !leverAnim.GetBool("push"))
        {
            if (this.name.Substring(this.name.Length - 1) == "1" && cutSceneProgress % 2 == 0)
            {
                StartCoroutine(playCutscene1());
                cutSceneProgress = cutSceneProgress + 1;
                AudioManagerScript.instance.Play("click");
            }
            else if (this.name.Substring(this.name.Length - 1) == "2" && cutSceneProgress / 10 % 2 == 0)
            {
                StartCoroutine(playCutscene2());
                cutSceneProgress = cutSceneProgress + 10;
                AudioManagerScript.instance.Play("click");
            }
            else if (this.name.Substring(this.name.Length - 1) == "3" && cutSceneProgress / 100 % 2 == 0)
            {
                StartCoroutine(playCutscene3());
                cutSceneProgress = cutSceneProgress + 100;
                AudioManagerScript.instance.Play("click");
            }
            else if (this.name.Substring(this.name.Length - 1) == "4" && cutSceneProgress / 1000 % 2 == 0)
            {
                StartCoroutine(playCutscene4());
                cutSceneProgress = cutSceneProgress + 1000;
                AudioManagerScript.instance.Play("click");
            }

            UniversalScript.instance.SaveData();
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

        yield return new WaitForSeconds(1.5f);

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

        yield return new WaitForSeconds(1.5f);

        CameraShakeScript.shake.shakeCamera(1.5f, 1.75f);

        yield return new WaitForSeconds(3f);

        vcam.Follow = origFollow;
        vcam.LookAt = origFollow;

        pmController.playerActive = true;
    }

    IEnumerator playCutscene3()
    {
        leverAnim.SetBool("push", true);

        Transform origFollow = vcam.Follow;
        vcam.Follow = cutFocus.transform;
        vcam.LookAt = cutFocus.transform;

        leverDir.Play();

        pmController.playerActive = false;

        yield return new WaitForSeconds(1.5f);

        CameraShakeScript.shake.shakeCamera(1.5f, 1.75f);

        yield return new WaitForSeconds(3f);

        vcam.Follow = origFollow;
        vcam.LookAt = origFollow;

        pmController.playerActive = true;
    }

    IEnumerator playCutscene4()
    {
        leverAnim.SetBool("push", true);

        Transform origFollow = vcam.Follow;
        vcam.Follow = cutFocus.transform;
        vcam.LookAt = cutFocus.transform;

        leverDir.Play();

        pmController.playerActive = false;

        yield return new WaitForSeconds(1.5f);

        CameraShakeScript.shake.shakeCamera(1.5f, 1.75f);

        yield return new WaitForSeconds(3f);

        vcam.Follow = origFollow;
        vcam.LookAt = origFollow;

        pmController.playerActive = true;
    }
}
