using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator doorAnim;

    void Start()
    {
        doorAnim = GetComponent<Animator>();

        StartCoroutine(closeDoorSFX());
    }

    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doorAnim.SetBool("open", false);
        }
    }

    IEnumerator closeDoorSFX()
    {
        yield return new WaitForSeconds(0.3f);

        AudioManagerScript.instance.Play("door close");
    }
}
