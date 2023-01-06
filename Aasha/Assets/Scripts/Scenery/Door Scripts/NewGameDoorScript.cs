using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameDoorScript : MonoBehaviour
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
            UniversalScript.instance.ResetData();
            SceneManager.LoadSceneAsync(2);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorAnim.SetBool("open", true);
            spaceBarObject.position = new Vector3(this.transform.position.x - 0.75f, this.transform.position.y - 1.75f, 0);
            southObject.position = new Vector3(this.transform.position.x + 1f, this.transform.position.y - 1.75f, 0);
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
            spaceBarObject.position = new Vector3(50, 50, 0);
            southObject.position = new Vector3(50, 50, 0);
            inDoor = false;

            AudioManagerScript.instance.End("door open");
            AudioManagerScript.instance.Play("door close");
        }
    }
}
