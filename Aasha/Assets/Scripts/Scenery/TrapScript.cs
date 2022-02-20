using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    private bool hit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !hit)
        {
            PlayerHealth.ph.removeHealth(1);
            StartCoroutine(startTimer());
        }
    }

    IEnumerator startTimer()
    {
        hit = true;
        yield return new WaitForSeconds(0.2f);
        hit = false;
    }
}
