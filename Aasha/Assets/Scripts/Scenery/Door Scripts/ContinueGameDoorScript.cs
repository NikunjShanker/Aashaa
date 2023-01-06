using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueGameDoorScript : MonoBehaviour
{
    private Transform spaceBarObject;
    private Transform southObject;
    private Animator doorAnim;
    private bool inDoor;

    private bool activateOnce;

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        spaceBarObject = GameObject.Find("Space Bar").GetComponent<Transform>();
        southObject = GameObject.Find("East").GetComponent<Transform>();
        inDoor = false;
        activateOnce = false;
    }

    void Update()
    {
        if (inDoor && PlayerMovement.instance.interact && !activateOnce)
        {
            activateOnce = true;
            if (UniversalScript.instance.savedSceneIndex != 1) SceneManager.LoadSceneAsync(UniversalScript.instance.savedSceneIndex);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (UniversalScript.instance.savedSceneIndex != 1 && UniversalScript.instance.savedSceneIndex != 10)
            {
                doorAnim.SetBool("open", true);
                spaceBarObject.position = new Vector3(this.transform.position.x - 0.75f, this.transform.position.y - 1.75f, 0);
                southObject.position = new Vector3(this.transform.position.x + 1f, this.transform.position.y - 1.75f, 0);
                inDoor = true;

                AudioManagerScript.instance.End("door close");
                AudioManagerScript.instance.Play("door open");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(inDoor) AudioManagerScript.instance.Play("door close"); AudioManagerScript.instance.End("door open");

            doorAnim.SetBool("open", false);
            spaceBarObject.position = new Vector3(50, 50, 0);
            southObject.position = new Vector3(50, 50, 0);
            inDoor = false;
        }
    }
}
