using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameDoorScript : MonoBehaviour
{
    private Transform spaceBarObject;
    private Animator doorAnim;
    private bool inDoor;

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        spaceBarObject = GameObject.Find("Space Bar").GetComponent<Transform>();
        inDoor = false;
    }

    void Update()
    {
        if (inDoor && PlayerMovement.instance.interact)
        {
            UniversalScript.instance.ResetData();
            SceneManager.LoadSceneAsync(3);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorAnim.SetBool("open", true);
            spaceBarObject.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.5f, 0);
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
            inDoor = false;

            AudioManagerScript.instance.End("door open");
            AudioManagerScript.instance.Play("door close");
        }
    }
}
