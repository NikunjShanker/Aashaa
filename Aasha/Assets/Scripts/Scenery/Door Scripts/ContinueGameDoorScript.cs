using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueGameDoorScript : MonoBehaviour
{
    private Animator doorAnim;
    private bool inDoor;

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        inDoor = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && inDoor)
            if(UniversalScript.instance.savedSceneIndex != 1) SceneManager.LoadSceneAsync(UniversalScript.instance.savedSceneIndex);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorAnim.SetBool("open", true);
            inDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorAnim.SetBool("open", false);
            inDoor = false;
        }
    }
}
