using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPointScript : MonoBehaviour
{
    private ParticleSystem respawnPS;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name != "Main Menu") respawnPS = GameObject.Find("Respawn PS").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(SceneManager.GetActiveScene().name != "Main Menu" && respawnPS.transform.position != this.transform.position)
            {
                respawnPS.transform.position = this.transform.position;
                respawnPS.Play();
            }

            PlayerMovement.instance.respawnPoint = this.transform.position;

            UniversalScript.instance.SaveData();
        }
    }
}
