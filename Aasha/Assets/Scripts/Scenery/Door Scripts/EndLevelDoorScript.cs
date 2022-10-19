using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelDoorScript : MonoBehaviour
{
    private Animator doorAnim;
    private bool inDoor;

    private bool activateOnce;

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        inDoor = false;

        activateOnce = false;
    }

    void Update()
    {
        if (inDoor && PlayerMovement.instance.interact && !activateOnce)
        {
            if (SceneManager.GetActiveScene().buildIndex == 9) UniversalScript.instance.pauseTimer();

            activateOnce = true;
            UniversalScript.instance.savedPos = new Vector3(0, -1.8f, 0);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorAnim.SetBool("open", true);

            inDoor = true;

            AudioManagerScript.instance.End("door close");
            AudioManagerScript.instance.Play("door open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorAnim.SetBool("open", false);

            inDoor = false;

            AudioManagerScript.instance.End("door open");
            AudioManagerScript.instance.Play("door close");
        }
    }
}
